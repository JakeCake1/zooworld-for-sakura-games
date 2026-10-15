using System;

namespace ZooWorld.Game.Features.FoodChain
{
    public abstract class EncounterResult
    {
        public static EncounterResult None { get; } = new NoEncounterResult();
        public static EncounterResult FirstEatsSecond { get; } = new FirstEatsSecondResult();
        public static EncounterResult SecondEatsFirst { get; } = new SecondEatsFirstResult();

        public abstract void Apply<TParticipant>(
            TParticipant first,
            TParticipant second,
            Action<TParticipant, TParticipant> feed);
    }
}
