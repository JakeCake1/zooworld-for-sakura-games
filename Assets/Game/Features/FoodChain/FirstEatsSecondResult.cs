using System;

namespace ZooWorld.Game.Features.FoodChain
{
    internal sealed class FirstEatsSecondResult : EncounterResult
    {
        public override void Apply<TParticipant>(
            TParticipant first,
            TParticipant second,
            Action<TParticipant, TParticipant> feed)
        {
            feed(first, second);
        }
    }
}
