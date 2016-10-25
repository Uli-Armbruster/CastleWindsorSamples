using System.Collections.Generic;

using Castle.Core.Internal;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples.Collection
{
    internal class CollectionSample : IAmASample
    {
        private readonly IEnumerable<ICanLog> _loggerCollection;

        public CollectionSample(IEnumerable<ICanLog> loggerCollection)
        {
            _loggerCollection = loggerCollection;
        }
        public void ShowMe()
        {
            //should contain 3 loggers
            _loggerCollection
                .ForEach(logger => logger.Log("Collection Sample works"));
        }
    }
}