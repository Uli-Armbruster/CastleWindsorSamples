namespace CastleWindsorSamples.SomeDependencies
{
    internal class UniversalBusinessLogic : IUniversalBusinessLogic
    {
        private readonly IDependency2 _dep2;
        private readonly IDependency3 _dep3;

        public UniversalBusinessLogic(IDependency2 dep2, IDependency3 dep3)
        {
            _dep2 = dep2;
            _dep3 = dep3;
        }
    }
}
