using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public static class FoodChainRules
    {
        public static EncounterResult Resolve(
            FeedingType firstFeedingType,
            int firstSpawnSequence,
            FeedingType secondFeedingType,
            int secondSpawnSequence)
        {
            if (BothArePrey(firstFeedingType, secondFeedingType))
            {
                return EncounterResult.None;
            }

            if (firstFeedingType == FeedingType.Predator && secondFeedingType == FeedingType.Prey)
            {
                return EncounterResult.FirstEatsSecond;
            }

            if (firstFeedingType == FeedingType.Prey && secondFeedingType == FeedingType.Predator)
            {
                return EncounterResult.SecondEatsFirst;
            }

            return OlderAnimalWins(firstSpawnSequence, secondSpawnSequence);
        }

        private static bool BothArePrey(FeedingType first, FeedingType second)
        {
            return first == FeedingType.Prey && second == FeedingType.Prey;
        }

        private static EncounterResult OlderAnimalWins(int firstSequence, int secondSequence)
        {
            return firstSequence <= secondSequence
                ? EncounterResult.FirstEatsSecond
                : EncounterResult.SecondEatsFirst;
        }
    }
}
