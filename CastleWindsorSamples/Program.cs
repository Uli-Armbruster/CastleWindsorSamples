using System;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //in this case I created a root aggregate that executes all samples in the solution
            var allSamples = Bootstrapper.Execute<AggregateRoot>();
            allSamples.ShowAll();
            Console.ReadLine();
        }
    }
}
