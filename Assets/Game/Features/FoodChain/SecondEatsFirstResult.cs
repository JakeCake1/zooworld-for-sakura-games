using System;

namespace ZooWorld.Game.Features.FoodChain
{
    internal sealed class SecondEatsFirstResult : EncounterResult
    {
        public override void Apply<TParticipant>(
            TParticipant first,
            TParticipant second,
            Action<TParticipant, TParticipant> feed)
        {
            feed(second, first);
        }
    }
}
