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

        [Test]
        public void Result_AppliesParticipantsInEatingOrder()
        {
            string eater = null;
            string food = null;
            EncounterResult.SecondEatsFirst.Apply("first", "second", (selectedEater, selectedFood) =>
            {
                eater = selectedEater;
                food = selectedFood;
            });

            Assert.That(eater, Is.EqualTo("second"));
            Assert.That(food, Is.EqualTo("first"));
        }

        [Test]
        public void None_DoesNotApplyFeedAction()
        {
            var applied = false;
            EncounterResult.None.Apply("first", "second", (eater, food) => applied = true);
            Assert.That(applied, Is.False);
        }
    }
}
