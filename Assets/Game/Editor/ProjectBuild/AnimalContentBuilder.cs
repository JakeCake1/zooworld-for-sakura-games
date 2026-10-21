using UnityEditor;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Animals.Common.Movement;
using ZooWorld.Game.Features.Animals.Frog;
using ZooWorld.Game.Features.Animals.Snake;
using ZooWorld.Editor.AnimalScaffolding;
using FrogAnimal = ZooWorld.Game.Features.Animals.Frog.Frog;
using SnakeAnimal = ZooWorld.Game.Features.Animals.Snake.Snake;

namespace ZooWorld.Editor.ProjectBuild
{
    internal sealed class AnimalContentBuilder
    {
        private readonly MaterialAssetBuilder _materials = new MaterialAssetBuilder();
        private readonly AnimalDefinitionAddressables _addressables = new AnimalDefinitionAddressables();

        public FrogDefinition BuildFrog()
        {
            var material = _materials.Create(
                ZooWorldProjectPaths.FrogContent + "/Frog.mat",
                new Color(0.25f, 0.75f, 0.38f));
            var prefab = BuildFrogPrefab(material);
            ConfigureAddress(prefab, "animals/frog");
            return BuildFrogDefinition();
        }

        public SnakeDefinition BuildSnake()
        {
            var material = _materials.Create(
                ZooWorldProjectPaths.SnakeContent + "/Snake.mat",
                new Color(0.82f, 0.26f, 0.20f));
            var prefab = BuildSnakePrefab(material);
            ConfigureAddress(prefab, "animals/snake");
            return BuildSnakeDefinition();
        }

        private GameObject BuildFrogPrefab(Material material)
        {
            var root = CreateCube("Frog", material, new Vector3(0.8f, 0.5f, 0.8f));
            ConfigureBody(root.AddComponent<Rigidbody>());
            root.AddComponent<FrogAnimal>();
            return SavePrefab(root, ZooWorldProjectPaths.FrogPrefab);
        }

        private GameObject BuildSnakePrefab(Material material)
        {
            var root = CreateCube("Snake", material, new Vector3(0.96f, 0.6f, 0.96f));
            ConfigureBody(root.AddComponent<Rigidbody>());
            var animal = root.AddComponent<SnakeAnimal>();
            var feedback = ConfigureTastyFeedback(root);
            AssignFeedback(animal, feedback);
            return SavePrefab(root, ZooWorldProjectPaths.SnakePrefab);
        }

        private GameObject CreateCube(string name, Material material, Vector3 scale)
        {
            var root = GameObject.CreatePrimitive(PrimitiveType.Cube);
            root.name = name;
            root.transform.localScale = scale;
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

        private TastyFeedback ConfigureTastyFeedback(GameObject root)
        {
            var feedback = root.AddComponent<TastyFeedback>();
            var serialized = new SerializedObject(feedback);
            serialized.FindProperty("_label").objectReferenceValue = CreateTastyLabel(root.transform);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return feedback;
        }

        private TextMesh CreateTastyLabel(Transform parent)
        {
            var label = CreateLabelObject(parent).AddComponent<TextMesh>();
            ConfigureLabelText(label);
            return label;
        }

        private GameObject CreateLabelObject(Transform parent)
        {
            var labelObject = new GameObject("TastyLabel");
            labelObject.transform.SetParent(parent, false);
            labelObject.transform.localPosition = new Vector3(0f, 0.05f, -1.1f);
            labelObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return labelObject;
        }

        private void ConfigureLabelText(TextMesh label)
        {
            label.text = "Tasty!";
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.09f;
            label.fontSize = 48;
            label.fontStyle = FontStyle.Bold;
            label.color = new Color(1f, 0.85f, 0.18f);
        }

        private void AssignFeedback(SnakeAnimal animal, TastyFeedback feedback)
        {
            var serialized = new SerializedObject(animal);
            serialized.FindProperty("_tastyFeedback").objectReferenceValue = feedback;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private GameObject SavePrefab(GameObject root, string path)
        {
            DeleteExistingAsset(path);
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private void DeleteExistingAsset(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

        private FrogDefinition BuildFrogDefinition()
        {
            var movement = LoadOrCreate<JumpMovementDefinition>(
                ZooWorldProjectPaths.FrogContent + "/JumpMovement.asset");
            var definition = LoadOrCreate<FrogDefinition>(ZooWorldProjectPaths.FrogDefinition);
            SetCommonFields(definition, "frog", 0, 1.25f, "animals/frog");
            SetFloat(movement, "_interval", 1.2f);
            SetFloat(movement, "_distance", 2.1f);
            SetFloat(movement, "_duration", 0.35f);
            SetMovement(definition, movement);
            _addressables.Register(ZooWorldProjectPaths.FrogDefinition);
            return definition;
        }

        private SnakeDefinition BuildSnakeDefinition()
        {
            var movement = LoadOrCreate<LinearMovementDefinition>(
                ZooWorldProjectPaths.SnakeContent + "/LinearMovement.asset");
            var definition = LoadOrCreate<SnakeDefinition>(ZooWorldProjectPaths.SnakeDefinition);
            SetCommonFields(definition, "snake", 1, 0.75f, "animals/snake");
            SetFloat(movement, "_speed", 2.1f);
            SetMovement(definition, movement);
            _addressables.Register(ZooWorldProjectPaths.SnakeDefinition);
            return definition;
        }

        private T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset != null ? asset : Create<T>(path);
        }

        private T Create<T>(string path) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private void SetCommonFields(Object definition, string id, int feedingType, float weight, string address)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty("_id").stringValue = id;
            serialized.FindProperty("_feedingType").enumValueIndex = feedingType;
            serialized.FindProperty("_spawnWeight").floatValue = weight;
            serialized.FindProperty("_prefabAddress").stringValue = address;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private void SetFloat(Object definition, string field, float value)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty(field).floatValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private void SetMovement(AnimalDefinition definition, MovementDefinition movement)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty("_movement").objectReferenceValue = movement;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private void ConfigureAddress(GameObject prefab, string address)
        {
            _addressables.RegisterPrefab(AssetDatabase.GetAssetPath(prefab), address);
        }
    }
}
