using System.Diagnostics;

namespace CastleWindsorSamples.Collection
{
    internal class DebugLogger : ICanLog
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
