using System.Collections.Generic;

using Castle.Core.Internal;

using CastleWindsorSamples.Infrastructure;

namespace CastleWindsorSamples
{
    internal class AggregateRoot
    {
        private readonly IEnumerable<IAmASample> _samples;

        public AggregateRoot(IEnumerable<IAmASample> samples)
        {
            _samples = samples;
        }

        public void ShowAll()
        {
            _samples.ForEach(sample => sample.ShowMe());
        }
    }
}
