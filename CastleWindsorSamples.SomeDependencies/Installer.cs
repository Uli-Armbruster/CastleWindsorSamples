using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples.SomeDependencies
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
            yield return Component
                .For<IDependency1>()
                .ImplementedBy<Dependency1>()
                .LifestyleTransient();
            yield return Component
                .For<IDependency2>()
                .ImplementedBy<Dependency2>()
                .LifestyleTransient();
            yield return Component
                .For<IDependency3>()
                .ImplementedBy<Dependency3>()
                .LifestyleTransient();
            yield return Component
                .For<IUniversalBusinessLogic>()
                .ImplementedBy<UniversalBusinessLogic>()
                .LifestyleTransient();
            yield return Component
                .For<IAmASample>()
                .ImplementedBy<DependenciesSample>()
                .Named("DepSample")
                .LifestyleTransient();
        }
    }
}
