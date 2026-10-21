using System.Linq;
using NUnit.Framework;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Frog;
using ZooWorld.Game.Features.Animals.Snake;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Tests.EditMode
{
    public sealed class AnimalDataDecompositionTests
    {
        [Test]
        public void CommonDto_HasNoSpeciesMovementFields()
        {
            var properties = typeof(AnimalConfigDto).GetProperties().Select(property => property.Name);
            Assert.That(properties, Does.Not.Contain("Speed"));
            Assert.That(properties, Does.Not.Contain("JumpDistance"));
        }

        [Test]
        public void SpeciesDto_AcceptsReusableMovementConfig()
        {
            var movement = new LinearMovementConfigDto(2f);
            var frog = new FrogConfigDto("frog", FeedingType.Prey, 1f, "animals/frog", movement);
            Assert.That(frog.Movement, Is.SameAs(movement));
            Assert.That(typeof(FrogConfigDto).GetProperty("Speed"), Is.Null);
        }

        [Test]
        public void MovementDtos_KeepStrategySpecificDataSeparate()
        {
            var linear = new LinearMovementConfigDto(2f);
            var jump = new JumpMovementConfigDto(1f, 2f, 0.3f);
            Assert.That(linear.Speed, Is.EqualTo(2f));
            Assert.That(jump.Distance, Is.EqualTo(2f));
        }
    }
}
