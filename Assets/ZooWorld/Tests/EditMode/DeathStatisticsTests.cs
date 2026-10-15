using NUnit.Framework;
using ZooWorld.Domain;

namespace ZooWorld.Tests
{
    public sealed class DeathStatisticsTests
    {
        [Test]
        public void Record_TracksEachDietSeparately()
        {
            var statistics = new DeathStatistics();

            statistics.Record(AnimalDiet.Prey);
            statistics.Record(AnimalDiet.Prey);
            statistics.Record(AnimalDiet.Predator);

            Assert.That(statistics.PreyDeaths, Is.EqualTo(2));
            Assert.That(statistics.PredatorDeaths, Is.EqualTo(1));
        }
    }
}
