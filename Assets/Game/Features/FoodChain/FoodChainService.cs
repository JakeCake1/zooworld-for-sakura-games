using System.Collections.Generic;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Game.Features.FoodChain
{
    public sealed class FoodChainService
    {
        private readonly HashSet<ulong> _processedPairs = new HashSet<ulong>();
        private int _processedFrame = -1;

        public void TryResolve(Animal first, Animal second, int frame)
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

        private bool CanResolve(Animal first, Animal second)
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

        private bool RegisterPair(Animal first, Animal second)
        {
            return _processedPairs.Add(CreatePairKey(first.GetInstanceID(), second.GetInstanceID()));
        }

        private void ResolveEncounter(Animal first, Animal second)
        {
            FoodChainRules
                .Resolve(first.FeedingType, first.SpawnSequence, second.FeedingType, second.SpawnSequence)
                .Apply(first, second, Feed);
        }

        private void Feed(Animal eater, Animal food)
        {
            food.Die();
            (eater as IPredator)?.NotifyFed();
        }

        private ulong CreatePairKey(int firstId, int secondId)
        {
            var first = unchecked((uint)firstId);
            var second = unchecked((uint)secondId);
            var min = first < second ? first : second;
            var max = first < second ? second : first;
            return ((ulong)min << 32) | max;
        }
    }
}
