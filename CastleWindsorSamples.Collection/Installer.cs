using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples.Collection
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //have a look at the AddResolvers method in the IoCInitializer
            var components = Components().ToArray();
            container.Register(components);
        }

        private static IEnumerable<IRegistration> Components()
        {
            //Register all components that implement ICanLog
            //so you can resolve later all logger
            yield return Classes
                .FromThisAssembly()
                .IncludeNonPublicTypes()
                .BasedOn<ICanLog>()
                .WithService
                .AllInterfaces()
                .LifestyleTransient();

            yield return Component
                .For<IAmASample>()
                .ImplementedBy<CollectionSample>()
                .Named("CollectionSample")
                .LifestyleTransient();
        }
    }
}
