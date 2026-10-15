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
            var result = FoodChainRules.Resolve(AnimalDiet.Prey, 1, AnimalDiet.Prey, 2);
            Assert.That(result, Is.EqualTo(EncounterResult.None));
        }

        [Test]
        public void Predator_EatsPrey()
        {
            var result = FoodChainRules.Resolve(AnimalDiet.Predator, 2, AnimalDiet.Prey, 1);
            Assert.That(result, Is.EqualTo(EncounterResult.FirstEatsSecond));
        }

        [Test]
        public void OlderPredator_Wins()
        {
            var result = FoodChainRules.Resolve(AnimalDiet.Predator, 3, AnimalDiet.Predator, 4);
            Assert.That(result, Is.EqualTo(EncounterResult.FirstEatsSecond));
        }
    }
}
