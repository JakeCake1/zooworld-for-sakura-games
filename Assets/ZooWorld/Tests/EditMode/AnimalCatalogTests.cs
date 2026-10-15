using System.Collections.Generic;
using NUnit.Framework;
using ZooWorld.Domain;

namespace ZooWorld.Tests
{
    public sealed class AnimalCatalogTests
    {
        [Test]
        public void Catalog_DependsOnConfigSource_NotScriptableObject()
        {
            var expected = new AnimalConfigDto(
                "json-backed-animal",
                AnimalDiet.Prey,
                MovementKind.Linear,
                1f,
                1f,
                1f,
                1f,
                1f,
                "animals/json-backed");
            var catalog = new AnimalCatalog(new FakeConfigSource(expected));

            var selected = catalog.GetRandom();

            Assert.That(selected, Is.SameAs(expected));
        }

        private sealed class FakeConfigSource : IAnimalConfigSource
        {
            private readonly IReadOnlyList<AnimalConfigDto> _configs;

            public FakeConfigSource(params AnimalConfigDto[] configs)
            {
                _configs = configs;
            }

            public IReadOnlyList<AnimalConfigDto> Load()
            {
                return _configs;
            }
        }
    }
}
