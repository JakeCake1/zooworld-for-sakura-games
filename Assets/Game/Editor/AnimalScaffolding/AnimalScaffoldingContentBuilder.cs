using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Presentation;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal sealed class AnimalScaffoldingContentBuilder
    {
        private readonly AnimalDefinitionAddressables _addressables = new AnimalDefinitionAddressables();
        private readonly Vector3 _defaultScale = new Vector3(0.8f, 0.5f, 0.8f);
        private readonly Color _defaultColor = new Color(0.30f, 0.62f, 0.86f);

        public void Create(string animalName, string featureFolder)
        {
            var paths = new AnimalContentPaths(animalName, featureFolder);
            var material = CreateMaterial(paths.Material);
            CreatePrefab(animalName, paths, material);
            var definition = CreateDefinition(animalName, paths);
            CompleteCreation(definition);
        }

        private Material CreateMaterial(string path)
        {
            var material = new Material(FindShader());
            material.color = _defaultColor;
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private Shader FindShader()
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            return shader != null ? shader : Shader.Find("Standard");
        }

        private void CreatePrefab(string animalName, AnimalContentPaths paths, Material material)
        {
            var root = CreateCube(animalName, material);
            ConfigureBody(root.AddComponent<Rigidbody>());
            root.AddComponent(FindAnimalType(animalName));
            SavePrefab(root, paths.Prefab);
            _addressables.RegisterPrefab(paths.Prefab, paths.PrefabAddress);
        }

        private GameObject CreateCube(string animalName, Material material)
        {
            var root = GameObject.CreatePrimitive(PrimitiveType.Cube);
            root.name = animalName;
            root.transform.localScale = _defaultScale;
            root.GetComponent<Renderer>().sharedMaterial = material;
            return root;
        }

        private void ConfigureBody(Rigidbody body)
        {
            body.useGravity = false;
            body.mass = 1f;
            body.linearDamping = 0.15f;
            body.angularDamping = 0f;
            body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private Type FindAnimalType(string animalName)
        {
            return FindSpeciesType<Animal>(animalName, animalName);
        }

        private Type FindDefinitionType(string animalName)
        {
            return FindSpeciesType<AnimalDefinition>(animalName, animalName + "Definition");
        }

        private Type FindSpeciesType<TBase>(string animalName, string typeName)
        {
            var expectedNamespace = $"ZooWorld.Game.Features.Animals.{animalName}";
            var type = TypeCache.GetTypesDerivedFrom<TBase>()
                .FirstOrDefault(candidate => candidate.Name == typeName && candidate.Namespace == expectedNamespace);
            return type ?? throw MissingType(typeName);
        }

        private Exception MissingType(string typeName)
        {
            return new InvalidOperationException($"Compiled animal type was not found: {typeName}.");
        }

        private void SavePrefab(GameObject root, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(root, path);
            UnityEngine.Object.DestroyImmediate(root);
        }

        private AnimalDefinition CreateDefinition(string animalName, AnimalContentPaths paths)
        {
            var definition = (AnimalDefinition)ScriptableObject.CreateInstance(FindDefinitionType(animalName));
            AssetDatabase.CreateAsset(definition, paths.Definition);
            ConfigureDefinition(definition, paths);
            _addressables.Register(paths.Definition);
            return definition;
        }

        private void ConfigureDefinition(AnimalDefinition definition, AnimalContentPaths paths)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty("_id").stringValue = paths.Id;
            serialized.FindProperty("_feedingType").enumValueIndex = (int)FeedingType.Prey;
            serialized.FindProperty("_spawnWeight").floatValue = 1f;
            serialized.FindProperty("_prefabAddress").stringValue = paths.PrefabAddress;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private void CompleteCreation(AnimalDefinition definition)
        {
            AssetDatabase.SaveAssets();
            Selection.activeObject = definition;
            EditorGUIUtility.PingObject(definition);
            Debug.Log($"Created animal content and Addressables entries for {definition.name}.");
        }

        private sealed class AnimalContentPaths
        {
            public AnimalContentPaths(string animalName, string featureFolder)
            {
                Id = animalName.ToLowerInvariant();
                PrefabAddress = "animals/" + Id;
                var content = featureFolder + "/Content";
                Material = $"{content}/{animalName}.mat";
                Prefab = $"{content}/{animalName}.prefab";
                Definition = $"{content}/{animalName}Definition.asset";
            }

            public string Id { get; }
            public string PrefabAddress { get; }
            public string Material { get; }
            public string Prefab { get; }
            public string Definition { get; }
        }
    }
}
