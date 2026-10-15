using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public static class FoodChainRules
    {
        public static EncounterResult Resolve(
            AnimalDiet firstDiet,
            int firstSpawnSequence,
            AnimalDiet secondDiet,
            int secondSpawnSequence)
        {
            if (BothArePrey(firstDiet, secondDiet))
            {
                return EncounterResult.None;
            }

            if (firstDiet == AnimalDiet.Predator && secondDiet == AnimalDiet.Prey)
            {
                return EncounterResult.FirstEatsSecond;
            }

            if (firstDiet == AnimalDiet.Prey && secondDiet == AnimalDiet.Predator)
            {
                return EncounterResult.SecondEatsFirst;
            }

            return OlderAnimalWins(firstSpawnSequence, secondSpawnSequence);
        }

        private static bool BothArePrey(AnimalDiet first, AnimalDiet second)
        {
            return first == AnimalDiet.Prey && second == AnimalDiet.Prey;
        }

        private static EncounterResult OlderAnimalWins(int firstSequence, int secondSequence)
        {
            return firstSequence <= secondSequence
                ? EncounterResult.FirstEatsSecond
                : EncounterResult.SecondEatsFirst;
        }
    }
}
