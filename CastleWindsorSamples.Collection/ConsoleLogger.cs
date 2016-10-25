using System;

namespace CastleWindsorSamples.Collection
{
    internal class ConsoleLogger : ICanLog
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
