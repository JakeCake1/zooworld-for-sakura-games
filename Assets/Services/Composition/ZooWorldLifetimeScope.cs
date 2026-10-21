using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.FoodChain;
using ZooWorld.Game.Features.Spawning;
using ZooWorld.Services.Assets;
using ZooWorld.Services.Configuration;
using ZooWorld.Ui.Hud;

namespace ZooWorld.Services.Composition
{
    public sealed class ZooWorldLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _animalRoot;

        public void SetSceneReferences(Transform animalRoot)
        {
            _animalRoot = animalRoot;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfiguration(builder);
            RegisterGameServices(builder);
            RegisterSceneComponents(builder);
        }

        private void RegisterConfiguration(IContainerBuilder builder)
        {
            IAnimalConfigSource source = new ScriptableObjectAnimalConfigSource();
            builder.RegisterInstance(source);
            builder.RegisterInstance(new AnimalCatalog(source));
        }

        private void RegisterGameServices(IContainerBuilder builder)
        {
            builder.Register<DeathStatistics>(Lifetime.Singleton);
            builder.Register<FoodChainService>(Lifetime.Singleton);
            builder.Register<IAnimalAssetProvider, AddressableAnimalAssetProvider>(Lifetime.Singleton);
            builder.Register<AnimalPool>(Lifetime.Singleton);
            RegisterMovement(builder);
            builder.RegisterInstance(_animalRoot);
            builder.Register<AnimalFactory>(Lifetime.Singleton);
        }

        private void RegisterMovement(IContainerBuilder builder)
        {
            IReadOnlyList<IAnimalMovementBuilder> builders = new IAnimalMovementBuilder[]
            {
                new LinearMovementBuilder(),
                new JumpMovementBuilder()
            };
            builder.RegisterInstance(builders);
            builder.Register<AnimalMovementFactory>(Lifetime.Singleton);
        }

        private void RegisterSceneComponents(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AnimalSpawner>();
            builder.RegisterComponentInHierarchy<HudView>();
        }
    }
}
