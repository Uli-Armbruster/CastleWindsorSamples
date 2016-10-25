using System;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples.SomeDependencies
{
    internal class DependenciesSample : IAmASample
    {
        private readonly IUniversalBusinessLogic _ubl;

        public DependenciesSample(IUniversalBusinessLogic ubl)
        {
            _ubl = ubl;
        }

        public void ShowMe()
        {
            //all dependencies resolved
            Console.WriteLine("DependenciesSample works");
        }
    }
}
