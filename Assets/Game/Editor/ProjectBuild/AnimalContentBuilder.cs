using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using ZooWorld.Game.Features.Animals.Frog;
using ZooWorld.Game.Features.Animals.Snake;
using FrogActor = ZooWorld.Game.Features.Animals.Frog.Frog;
using SnakeActor = ZooWorld.Game.Features.Animals.Snake.Snake;

namespace ZooWorld.Editor.ProjectBuild
{
    internal static class AnimalContentBuilder
    {
        public static FrogDefinition BuildFrog()
        {
            var material = MaterialAssetBuilder.Create(
                ZooWorldProjectPaths.FrogContent + "/Frog.mat",
                new Color(0.25f, 0.75f, 0.38f));
            var prefab = BuildFrogPrefab(material);
            ConfigureAddress(prefab, "animals/frog");
            return BuildFrogDefinition();
        }

        public static SnakeDefinition BuildSnake()
        {
            var material = MaterialAssetBuilder.Create(
                ZooWorldProjectPaths.SnakeContent + "/Snake.mat",
                new Color(0.82f, 0.26f, 0.20f));
            var prefab = BuildSnakePrefab(material);
            ConfigureAddress(prefab, "animals/snake");
            return BuildSnakeDefinition();
        }

        private static GameObject BuildFrogPrefab(Material material)
        {
            var root = CreateCube("Frog", material, new Vector3(0.8f, 0.5f, 0.8f));
            ConfigureBody(root.AddComponent<Rigidbody>());
            root.AddComponent<FrogActor>();
            return SavePrefab(root, ZooWorldProjectPaths.FrogPrefab);
        }

        private static GameObject BuildSnakePrefab(Material material)
        {
            var root = CreateCube("Snake", material, new Vector3(0.48f, 0.52f, 1.15f));
            ConfigureBody(root.AddComponent<Rigidbody>());
            var actor = root.AddComponent<SnakeActor>();
            var feedback = ConfigureTastyFeedback(root);
            AssignFeedback(actor, feedback);
            return SavePrefab(root, ZooWorldProjectPaths.SnakePrefab);
        }

        private static GameObject CreateCube(string name, Material material, Vector3 scale)
        {
            var root = GameObject.CreatePrimitive(PrimitiveType.Cube);
            root.name = name;
            root.transform.localScale = scale;
            root.GetComponent<Renderer>().sharedMaterial = material;
            return root;
        }

        private static void ConfigureBody(Rigidbody body)
        {
            body.useGravity = false;
            body.mass = 1f;
            body.linearDamping = 0.15f;
            body.angularDamping = 0f;
            body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        private static TastyFeedback ConfigureTastyFeedback(GameObject root)
        {
            var feedback = root.AddComponent<TastyFeedback>();
            var serialized = new SerializedObject(feedback);
            serialized.FindProperty("_label").objectReferenceValue = CreateTastyLabel(root.transform);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return feedback;
        }

        private static TextMesh CreateTastyLabel(Transform parent)
        {
            var label = CreateLabelObject(parent).AddComponent<TextMesh>();
            ConfigureLabelText(label);
            return label;
        }

        private static GameObject CreateLabelObject(Transform parent)
        {
            var labelObject = new GameObject("TastyLabel");
            labelObject.transform.SetParent(parent, false);
            labelObject.transform.localPosition = new Vector3(0f, 0.05f, -1.1f);
            labelObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            return labelObject;
        }

        private static void ConfigureLabelText(TextMesh label)
        {
            label.text = "Tasty!";
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.09f;
            label.fontSize = 48;
            label.fontStyle = FontStyle.Bold;
            label.color = new Color(1f, 0.85f, 0.18f);
        }

        private static void AssignFeedback(SnakeActor actor, TastyFeedback feedback)
        {
            var serialized = new SerializedObject(actor);
            serialized.FindProperty("_tastyFeedback").objectReferenceValue = feedback;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static GameObject SavePrefab(GameObject root, string path)
        {
            DeleteExistingAsset(path);
            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static void DeleteExistingAsset(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }
        }

        private static FrogDefinition BuildFrogDefinition()
        {
            var definition = LoadOrCreate<FrogDefinition>(ZooWorldProjectPaths.FrogDefinition);
            SetCommonFields(definition, "frog", 0, 1.25f, "animals/frog");
            SetFloat(definition, "_jumpInterval", 1.2f);
            SetFloat(definition, "_jumpDistance", 2.1f);
            SetFloat(definition, "_jumpDuration", 0.35f);
            return definition;
        }

        private static SnakeDefinition BuildSnakeDefinition()
        {
            var definition = LoadOrCreate<SnakeDefinition>(ZooWorldProjectPaths.SnakeDefinition);
            SetCommonFields(definition, "snake", 1, 0.75f, "animals/snake");
            SetFloat(definition, "_speed", 2.1f);
            return definition;
        }

        private static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            return asset != null ? asset : Create<T>(path);
        }

        private static T Create<T>(string path) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void SetCommonFields(Object definition, string id, int diet, float weight, string address)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty("_id").stringValue = id;
            serialized.FindProperty("_diet").enumValueIndex = diet;
            serialized.FindProperty("_spawnWeight").floatValue = weight;
            serialized.FindProperty("_prefabAddress").stringValue = address;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetFloat(Object definition, string field, float value)
        {
            var serialized = new SerializedObject(definition);
            serialized.FindProperty(field).floatValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureAddress(GameObject prefab, string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.address = address;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
        }
    }
}
