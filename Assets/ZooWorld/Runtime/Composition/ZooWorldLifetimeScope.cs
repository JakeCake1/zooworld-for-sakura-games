using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZooWorld.Domain;
using ZooWorld.Infrastructure;

namespace ZooWorld.Composition
{
    public sealed class ZooWorldLifetimeScope : LifetimeScope
    {
        [SerializeField] private AnimalDefinition[] _animalDefinitions;
        [SerializeField] private Transform _animalRoot;

        public void SetSceneReferences(AnimalDefinition[] definitions, Transform animalRoot)
        {
            _animalDefinitions = definitions;
            _animalRoot = animalRoot;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            IAnimalConfigSource configSource = new ScriptableObjectAnimalConfigSource(_animalDefinitions);
            builder.RegisterInstance(configSource);
            builder.RegisterInstance(new AnimalCatalog(configSource));
            builder.Register<DeathStatistics>(Lifetime.Singleton);
            builder.Register<FoodChainService>(Lifetime.Singleton);
            builder.Register<IAnimalAssetProvider, AddressableAnimalAssetProvider>(Lifetime.Singleton);
            builder.Register<AnimalFactory>(Lifetime.Singleton)
                .WithParameter("animalRoot", _animalRoot);
            builder.RegisterComponentInHierarchy<AnimalSpawner>();
            builder.RegisterComponentInHierarchy<HudPresenter>();
        }
    }
}
