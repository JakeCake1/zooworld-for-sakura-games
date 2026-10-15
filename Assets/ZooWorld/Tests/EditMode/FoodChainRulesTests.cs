using NUnit.Framework;
using ZooWorld.Domain;

namespace ZooWorld.Tests
{
    public sealed class FoodChainRulesTests
    {
        [Test]
        public void TwoPrey_DoNotEatEachOther()
        {
            var result = FoodChainRules.Resolve(AnimalDiet.Prey, 1, AnimalDiet.Prey, 2);

            Assert.That(result, Is.EqualTo(EncounterResult.None));
        }

        [TestCase(AnimalDiet.Predator, AnimalDiet.Prey, EncounterResult.FirstEatsSecond)]
        [TestCase(AnimalDiet.Prey, AnimalDiet.Predator, EncounterResult.SecondEatsFirst)]
        public void Predator_EatsPrey(
            AnimalDiet first,
            AnimalDiet second,
            EncounterResult expected)
        {
            var result = FoodChainRules.Resolve(first, 1, second, 2);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TwoPredators_OlderAnimalSurvives()
        {
            var result = FoodChainRules.Resolve(AnimalDiet.Predator, 12, AnimalDiet.Predator, 7);

            Assert.That(result, Is.EqualTo(EncounterResult.SecondEatsFirst));
        }
    }
}
