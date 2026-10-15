using System;
using ZooWorld.Game.Features.Animals.Common.Data;

namespace ZooWorld.Game.Features.FoodChain
{
    internal readonly struct FeedingPair : IEquatable<FeedingPair>
    {
        private readonly FeedingType _first;
        private readonly FeedingType _second;

        public FeedingPair(FeedingType first, FeedingType second)
        {
            _first = first;
            _second = second;
        }

        public bool Equals(FeedingPair other)
        {
            return _first == other._first && _second == other._second;
        }

        public override bool Equals(object obj)
        {
            return obj is FeedingPair other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ((int)_first * 397) ^ (int)_second;
        }
    }
}
