using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public sealed class DeathStatistics
    {
        public int PreyDeaths { get; private set; }
        public int PredatorDeaths { get; private set; }
        public event Action Changed;

        public void Record(AnimalDiet diet)
        {
            Increment(diet);
            Changed?.Invoke();
        }

        private void Increment(AnimalDiet diet)
        {
            if (diet == AnimalDiet.Prey)
            {
                PreyDeaths++;
                return;
            }

            PredatorDeaths++;
        }
    }
}
