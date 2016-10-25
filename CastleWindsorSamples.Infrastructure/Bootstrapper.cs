namespace CastleWindsorSamples.Infrastructure
{
    public class Bootstrapper
    {
        
        public Bootstrapper()
        {
            
        }

        /// <summary>
        /// Return your entry point for the application by requesting your Root Aggregate
        /// Have a look at this Bootstrapping Tutorial
#pragma warning disable 1570
        /// https://www.youtube.com/watch?v=KuveJ-LJS8E&index=1&list=PLT5x3ZgXSEb8vbp4LW7TkuQVRerB08jZv
#pragma warning restore 1570
        /// </summary>
        /// <typeparam name="T">Root Aggregate</typeparam>
        /// <returns></returns>
        public static T Execute<T>()
        {
            //do something
            var container = new IoCInitializer()
                .RegisterComponents()
                .Instance();
            //do something

            return container.Resolve<T>();
        }
    }
}