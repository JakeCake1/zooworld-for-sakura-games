using System;

namespace ZooWorld.Game.Features.FoodChain
{
    internal sealed class NoEncounterResult : EncounterResult
    {
        public override void Apply<TParticipant>(
            TParticipant first,
            TParticipant second,
            Action<TParticipant, TParticipant> feed)
        {
        }
    }
}
