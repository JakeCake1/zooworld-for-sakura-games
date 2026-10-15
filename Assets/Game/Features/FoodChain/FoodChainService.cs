using System.Collections.Generic;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.FoodChain
{
    public sealed class FoodChainService
    {
        private readonly HashSet<ulong> _processedPairs = new HashSet<ulong>();
        private int _processedFrame = -1;

        public void TryResolve(AnimalActor first, AnimalActor second, int frame)
        {
            if (!CanResolve(first, second))
            {
                return;
            }

            PrepareFrame(frame);
            if (!RegisterPair(first, second))
            {
                return;
            }

            ResolveEncounter(first, second);
        }

        private static bool CanResolve(AnimalActor first, AnimalActor second)
        {
            return first != null && second != null && !first.IsDead && !second.IsDead;
        }

        private void PrepareFrame(int frame)
        {
            if (_processedFrame == frame)
            {
                return;
            }

            _processedFrame = frame;
            _processedPairs.Clear();
        }

        private bool RegisterPair(AnimalActor first, AnimalActor second)
        {
            return _processedPairs.Add(CreatePairKey(first.GetInstanceID(), second.GetInstanceID()));
        }

        private static void ResolveEncounter(AnimalActor first, AnimalActor second)
        {
            var result = FoodChainRules.Resolve(first.Diet, first.SpawnSequence, second.Diet, second.SpawnSequence);
            ApplyResult(result, first, second);
        }

        private static void ApplyResult(EncounterResult result, AnimalActor first, AnimalActor second)
        {
            switch (result)
            {
                case EncounterResult.FirstEatsSecond:
                    Feed(first, second);
                    break;
                case EncounterResult.SecondEatsFirst:
                    Feed(second, first);
                    break;
            }
        }

        private static void Feed(AnimalActor eater, AnimalActor food)
        {
            food.Die();
            (eater as IPredator)?.NotifyFed();
        }

        private static ulong CreatePairKey(int firstId, int secondId)
        {
            var first = unchecked((uint)firstId);
            var second = unchecked((uint)secondId);
            var min = first < second ? first : second;
            var max = first < second ? second : first;
            return ((ulong)min << 32) | max;
        }
    }
}
