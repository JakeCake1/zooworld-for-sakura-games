using System;

namespace ZooWorld.Domain
{
    public sealed class DeathStatistics
    {
        public int PreyDeaths { get; private set; }
        public int PredatorDeaths { get; private set; }

        public event Action Changed;

        public void Record(AnimalDiet diet)
        {
            if (diet == AnimalDiet.Prey)
            {
                PreyDeaths++;
            }
            else
            {
                PredatorDeaths++;
            }

            Changed?.Invoke();
        }
    }
}
