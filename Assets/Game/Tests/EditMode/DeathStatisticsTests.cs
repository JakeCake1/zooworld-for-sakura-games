using NUnit.Framework;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Tests.EditMode
{
    public sealed class DeathStatisticsTests
    {
        [Test]
        public void Record_UpdatesSeparateCounters()
        {
            var statistics = new DeathStatistics();
            statistics.Record(AnimalDiet.Prey);
            statistics.Record(AnimalDiet.Predator);
            Assert.That(statistics.PreyDeaths, Is.EqualTo(1));
            Assert.That(statistics.PredatorDeaths, Is.EqualTo(1));
        }
    }
}
