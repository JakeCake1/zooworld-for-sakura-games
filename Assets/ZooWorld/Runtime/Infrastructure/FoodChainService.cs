using System.Collections.Generic;
using ZooWorld.Domain;
using ZooWorld.Presentation;

namespace ZooWorld.Infrastructure
{
    public sealed class FoodChainService
    {
        private readonly HashSet<ulong> _processedPairs = new HashSet<ulong>();
        private int _processedFrame = -1;

        public void TryResolve(AnimalActor first, AnimalActor second, int frame)
        {
            if (first == null || second == null || first.IsDead || second.IsDead)
            {
                return;
            }

            if (_processedFrame != frame)
            {
                _processedFrame = frame;
                _processedPairs.Clear();
            }

            var pairKey = CreatePairKey(first.GetInstanceID(), second.GetInstanceID());
            if (!_processedPairs.Add(pairKey))
            {
                return;
            }

            var result = FoodChainRules.Resolve(
                first.Diet,
                first.SpawnSequence,
                second.Diet,
                second.SpawnSequence);

            switch (result)
            {
                case EncounterResult.FirstEatsSecond:
                    second.Die();
                    first.NotifyFed();
                    break;
                case EncounterResult.SecondEatsFirst:
                    first.Die();
                    second.NotifyFed();
                    break;
            }
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
