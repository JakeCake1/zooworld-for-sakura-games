using System.Collections.Generic;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    public static class FoodChainRules
    {
        private delegate EncounterResult EncounterResolver(int firstSequence, int secondSequence);

        private static readonly IReadOnlyDictionary<FeedingPair, EncounterResolver> Resolvers = CreateResolvers();

        public static EncounterResult Resolve(
            FeedingType firstFeedingType,
            int firstSpawnSequence,
            FeedingType secondFeedingType,
            int secondSpawnSequence)
        {
            return Resolvers[new FeedingPair(firstFeedingType, secondFeedingType)](firstSpawnSequence, secondSpawnSequence);
        }

        private static IReadOnlyDictionary<FeedingPair, EncounterResolver> CreateResolvers()
        {
            return new Dictionary<FeedingPair, EncounterResolver>
            {
                [new FeedingPair(FeedingType.Prey, FeedingType.Prey)] = NoEncounter,
                [new FeedingPair(FeedingType.Predator, FeedingType.Prey)] = FirstEatsSecond,
                [new FeedingPair(FeedingType.Prey, FeedingType.Predator)] = SecondEatsFirst,
                [new FeedingPair(FeedingType.Predator, FeedingType.Predator)] = OlderAnimalWins
            };
        }

        private static EncounterResult NoEncounter(int firstSequence, int secondSequence)
        {
            return EncounterResult.None;
        }

        private static EncounterResult FirstEatsSecond(int firstSequence, int secondSequence)
        {
            return EncounterResult.FirstEatsSecond;
        }

        private static EncounterResult SecondEatsFirst(int firstSequence, int secondSequence)
        {
            return EncounterResult.SecondEatsFirst;
        }

        private static EncounterResult OlderAnimalWins(int firstSequence, int secondSequence)
        {
            return firstSequence <= secondSequence
                ? EncounterResult.FirstEatsSecond
                : EncounterResult.SecondEatsFirst;
        }

    }
}
