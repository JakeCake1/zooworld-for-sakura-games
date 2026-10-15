using NUnit.Framework;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Tests.EditMode
{
    public sealed class FoodChainRulesTests
    {
        [Test]
        public void TwoPrey_DoNotEatEachOther()
        {
            var result = FoodChainRules.Resolve(FeedingType.Prey, 1, FeedingType.Prey, 2);
            Assert.That(result, Is.EqualTo(EncounterResult.None));
        }

        [Test]
        public void Predator_EatsPrey()
        {
            var result = FoodChainRules.Resolve(FeedingType.Predator, 2, FeedingType.Prey, 1);
            Assert.That(result, Is.EqualTo(EncounterResult.FirstEatsSecond));
        }

        [Test]
        public void OlderPredator_Wins()
        {
            var result = FoodChainRules.Resolve(FeedingType.Predator, 3, FeedingType.Predator, 4);
            Assert.That(result, Is.EqualTo(EncounterResult.FirstEatsSecond));
        }
    }
}
