using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CastleWindsorSamples
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
                .For<AggregateRoot>()
                .LifestyleTransient();
        }
    }
}
