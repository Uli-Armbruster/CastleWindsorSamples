using System;
using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel;
using Castle.Windsor;

using FluentAssertions;

namespace CastleWindsorSamples.IntegrationTests
{
    internal static class ContainerExtensions
    {
        private static Queue<string> Messages { get; set; }

        public static IEnumerable<Exception> AssertConfigurationIsValidAndDispose(this IWindsorContainer container,
                                                                                  int expectAtLeastHandlers)
        {
            try
            {
                Messages = new Queue<string>();

                var handlers = GetTestableHandlers(container, expectAtLeastHandlers);
                var count = handlers.Count();

                var resolvingExceptions = handlers
                    .Select((handler, i) => ResolveServices(container, handler, i, count))
                    .SelectMany(x => x.Where(y => y != null))
                    .ToList();

                if (resolvingExceptions.Any())
                {
                    Messages.ToList().ForEach(Console.WriteLine);
                }

                return resolvingExceptions;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while integrating all components: {0}", ex.Message);
                throw;
            }
            finally
            {
                Messages = null;
                Dipose(container);
            }
        }

        private static bool Is(this Type self, Type type)
        {
            return self == type ||
                   self.GetInterfaces().Contains(type) ||
                   self.IsSubclassOf(type);
        }

        private static List<IHandler> GetTestableHandlers(IWindsorContainer container, int expectAtLeastHandlers)
        {
            var handlers = container.Kernel.GetAssignableHandlers(typeof(object)).ToList();

            var testableHandlers = handlers
                .Where(handler => !handler.ComponentModel.Implementation.Is(typeof(IAmNotTestable)))
                .ToList();

            testableHandlers.Should().NotBeEmpty("No testable components found");
            testableHandlers
                .Count
                .Should()
                .BeGreaterOrEqualTo(expectAtLeastHandlers,
                                    $"Expected at least {expectAtLeastHandlers} but found only {testableHandlers.Count} components"
                                   );

            return testableHandlers;
        }

        private static IEnumerable<Exception> ResolveServices(IWindsorContainer container,
                                                              IHandler handler,
                                                              int index,
                                                              int count)
        {
            Messages.Enqueue($"\r\n\t[{index + 1:000}/{count:000}]: {handler.ComponentModel.Name}");

            return handler
                .ComponentModel
                .Services
                .Select(service => TryResolve(container, handler, service));
        }

        private static Exception TryResolve(IWindsorContainer container, IHandler handler, Type service)
        {
            try
            {
                Messages.Enqueue($"\t\t\t {handler.ComponentModel.Implementation.Name} implements {service.Name}");
                Resolve(container, service);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static void Resolve(IWindsorContainer container, Type service)
        {
            var typeToResolve = GetTypeToResolve(service);

            object instance = null;

            try
            {
                instance = container.Resolve(typeToResolve);
            }
            finally
            {
                if (instance != null)
                {
                    container.Release(instance);
                }
            }
        }

        private static Type GetTypeToResolve(Type service)
        {
            if (!service.ContainsGenericParameters)
            {
                return service;
            }

            var constraintTypes = service.GetGenericArguments()
                                         .Select(
                                                 genericArgument =>
                                                 {
                                                     var constraints = genericArgument.GetGenericParameterConstraints();
                                                     if (constraints.Length > 1)
                                                     {
                                                         throw new NotSupportedException(
                                                                                         "More than one constraint not supported");
                                                     }

                                                     return constraints;
                                                 })
                                         .Select(ConstrainedTypeOrObject)
                                         .ToArray();

            return service.MakeGenericType(constraintTypes);
        }

        private static Type ConstrainedTypeOrObject(Type[] constraints)
        {
            return constraints.Length == 0 ? typeof(object) : constraints[0];
        }

        private static void Dipose(IDisposable container)
        {
            try
            {
                container.Dispose();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Container could not be disposed", ex);
            }
        }
    }
}
