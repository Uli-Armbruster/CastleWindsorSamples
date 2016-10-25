namespace CastleWindsorSamples.SomeDependencies
{
    internal class Dependency2 : IDependency2
    {
        private readonly IDependency1 _dep1;

        public Dependency2(IDependency1 dep1)
        {
            _dep1 = dep1;
        }
    }
}
