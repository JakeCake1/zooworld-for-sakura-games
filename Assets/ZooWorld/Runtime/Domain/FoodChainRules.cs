namespace ZooWorld.Domain
{
    public static class FoodChainRules
    {
        public static EncounterResult Resolve(
            AnimalDiet firstDiet,
            int firstSpawnSequence,
            AnimalDiet secondDiet,
            int secondSpawnSequence)
        {
            if (firstDiet == AnimalDiet.Prey && secondDiet == AnimalDiet.Prey)
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

            // A deterministic rule keeps predator encounters testable: the older animal survives.
            return firstSpawnSequence <= secondSpawnSequence
                ? EncounterResult.FirstEatsSecond
                : EncounterResult.SecondEatsFirst;
        }
    }
}
