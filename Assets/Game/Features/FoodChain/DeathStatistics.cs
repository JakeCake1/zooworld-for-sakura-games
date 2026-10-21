using System;
using System.Collections.Generic;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public sealed class DeathStatistics
    {
        private readonly IReadOnlyDictionary<FeedingType, Action> _recorders;

        public DeathStatistics()
        {
            _recorders = new Dictionary<FeedingType, Action>
            {
                [FeedingType.Prey] = RecordPrey,
                [FeedingType.Predator] = RecordPredator
            };
        }

        public int PreyDeaths { get; private set; }
        public int PredatorDeaths { get; private set; }
        public event Action Changed;

        public void Record(FeedingType feedingType)
        {
            Increment(feedingType);
            Changed?.Invoke();
        }

        private void Increment(FeedingType feedingType)
        {
            GetRecorder(feedingType).Invoke();
        }

        private Action GetRecorder(FeedingType feedingType)
        {
            return _recorders.TryGetValue(feedingType, out var recorder)
                ? recorder
                : throw new ArgumentOutOfRangeException(nameof(feedingType), feedingType, "Unsupported feeding type.");
        }

        private void RecordPrey()
        {
            PreyDeaths++;
        }

        private void RecordPredator()
        {
            PredatorDeaths++;
        }
    }
}
