namespace CastleWindsorSamples.SomeDependencies
{
    internal class Dependency3 : IDependency3
    {
        private readonly IDependency1 _dep1;
        private readonly IDependency2 _dep2;

        public Dependency3(IDependency1 dep1, IDependency2 dep2)
        {
            _dep1 = dep1;
            _dep2 = dep2;
        }
    }
}
