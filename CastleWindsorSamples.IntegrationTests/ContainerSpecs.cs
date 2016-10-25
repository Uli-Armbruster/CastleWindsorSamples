using System;
using System.Collections.Generic;
using System.Diagnostics;

using Castle.Windsor;

using CastleWindsorSamples.Infrastructure;

using FluentAssertions;

using Machine.Specifications;

namespace CastleWindsorSamples.IntegrationTests
{
    public class ContainerSpecs
    {
        [Subject("Container Integrationtests")]
        [Tags("Integrationtests")]
        public class When_container_was_created
        {
            private static IEnumerable<Exception> _errors = new List<Exception>();

            private static IWindsorContainer _sut;

            private Establish context = () =>
            {
                Console.WriteLine("Arrange start");
                _sut = new IoCInitializer().RegisterComponents().Instance();
                Console.WriteLine("Arrange end");
            };

            private Because of = () =>
            {
                Console.WriteLine("Act start");
                //depending on your registrations, try to take the value as high as possible
                _errors = _sut.AssertConfigurationIsValidAndDispose(5); 
                Console.WriteLine("Act end");
            };

            private It should_could_resolve_all_dependencies = () =>
            {
                Console.WriteLine("Assert start");
                _errors.Should().BeEmpty();
                Console.WriteLine("Assert end");
            };
        }
    }
}
