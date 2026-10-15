using System.Linq;
using NUnit.Framework;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Frog;
using ZooWorld.Game.Features.Animals.Snake;

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
        public void FrogDto_OwnsJumpDataOnly()
        {
            var frog = new FrogConfigDto("frog", FeedingType.Prey, 1f, "animals/frog", 1f, 2f, 0.3f);
            Assert.That(frog.JumpDistance, Is.EqualTo(2f));
            Assert.That(typeof(FrogConfigDto).GetProperty("Speed"), Is.Null);
        }

        [Test]
        public void SnakeDto_OwnsLinearSpeedOnly()
        {
            var snake = new SnakeConfigDto("snake", FeedingType.Predator, 1f, "animals/snake", 2f);
            Assert.That(snake.Speed, Is.EqualTo(2f));
            Assert.That(typeof(SnakeConfigDto).GetProperty("JumpInterval"), Is.Null);
        }
    }
}
