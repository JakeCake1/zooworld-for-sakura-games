using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Movement;

namespace ZooWorld.Tests.EditMode
{
    public sealed class AnimalMovementFactoryTests
    {
        [Test]
        public void Create_SelectsBuilderFromMovementConfig()
        {
            var owner = new GameObject("Movement test");
            var body = owner.AddComponent<Rigidbody>();
            var factory = CreateFactory();

            var movement = factory.Create(body, new LinearMovementConfigDto(2f));

            Assert.That(movement, Is.TypeOf<LinearMovement>());
            Object.DestroyImmediate(owner);
        }

        private AnimalMovementFactory CreateFactory()
        {
            IReadOnlyList<IAnimalMovementBuilder> builders = new IAnimalMovementBuilder[]
            {
                new LinearMovementBuilder(),
                new JumpMovementBuilder()
            };
            return new AnimalMovementFactory(builders);
        }
    }
}
