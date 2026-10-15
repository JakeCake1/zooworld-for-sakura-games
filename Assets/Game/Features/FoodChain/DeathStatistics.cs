using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public sealed class DeathStatistics
    {
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
            if (feedingType == FeedingType.Prey)
            {
                PreyDeaths++;
                return;
            }

            PredatorDeaths++;
        }
    }
}
