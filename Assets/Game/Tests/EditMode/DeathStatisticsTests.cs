using System;
using NUnit.Framework;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.FoodChain;

namespace ZooWorld.Tests.EditMode
{
    public sealed class DeathStatisticsTests
    {
        [Test]
        public void Record_HasRecorderForEveryFeedingType()
        {
            var statistics = new DeathStatistics();

            foreach (FeedingType feedingType in Enum.GetValues(typeof(FeedingType)))
            {
                Assert.DoesNotThrow(() => statistics.Record(feedingType), feedingType.ToString());
            }
        }

        [Test]
        public void Record_UpdatesSeparateCounters()
        {
            var statistics = new DeathStatistics();
            statistics.Record(FeedingType.Prey);
            statistics.Record(FeedingType.Predator);
            Assert.That(statistics.PreyDeaths, Is.EqualTo(1));
            Assert.That(statistics.PredatorDeaths, Is.EqualTo(1));
        }

        [Test]
        public void Record_RejectsUnmappedFeedingType()
        {
            var statistics = new DeathStatistics();
            Assert.Throws<ArgumentOutOfRangeException>(() => statistics.Record((FeedingType)999));
        }
    }
}
