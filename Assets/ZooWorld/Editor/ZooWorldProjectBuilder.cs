using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZooWorld.Composition;
using ZooWorld.Domain;
using ZooWorld.Presentation;

namespace ZooWorld.Editor
{
    public static class ZooWorldProjectBuilder
    {
        private const string Root = "Assets/ZooWorld";
        private const string Generated = Root + "/Generated";
        private const string Materials = Generated + "/Materials";
        private const string Prefabs = Generated + "/Prefabs";
        private const string Configs = Generated + "/Configs";
        private const string Scenes = Root + "/Scenes";
        private const string MainScenePath = Scenes + "/Main.unity";

        [MenuItem("Zoo World/Rebuild Demo Content")]
        public static void Build()
        {
            EnsureFolders();

            var groundMaterial = CreateMaterial(Materials + "/Ground.mat", new Color(0.11f, 0.33f, 0.18f));
            var frogMaterial = CreateMaterial(Materials + "/Frog.mat", new Color(0.25f, 0.75f, 0.38f));
            var snakeMaterial = CreateMaterial(Materials + "/Snake.mat", new Color(0.82f, 0.26f, 0.20f));

            var frogPrefab = CreateAnimalPrefab(
                Prefabs + "/Frog.prefab",
                PrimitiveType.Sphere,
                frogMaterial,
                new Vector3(0.8f, 0.5f, 0.8f));
            var snakePrefab = CreateAnimalPrefab(
                Prefabs + "/Snake.prefab",
                PrimitiveType.Capsule,
                snakeMaterial,
                new Vector3(0.48f, 0.52f, 1.15f));

            var frogDefinition = CreateDefinition(
                Configs + "/Frog.asset",
                "frog",
                AnimalDiet.Prey,
                MovementKind.Jump,
                2.2f,
                1.2f,
                2.1f,
                0.35f,
                1.25f,
                "animals/frog");
            var snakeDefinition = CreateDefinition(
                Configs + "/Snake.asset",
                "snake",
                AnimalDiet.Predator,
                MovementKind.Linear,
                2.1f,
                1f,
                2f,
                0.3f,
                0.75f,
                "animals/snake");

            // Reload newly created assets before storing scene references. Without this import
            // boundary a clean batch build can serialize fresh ScriptableObjects as fileID 0.
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            frogDefinition = AssetDatabase.LoadAssetAtPath<AnimalDefinition>(Configs + "/Frog.asset");
            snakeDefinition = AssetDatabase.LoadAssetAtPath<AnimalDefinition>(Configs + "/Snake.asset");
            if (frogDefinition == null || snakeDefinition == null)
            {
                throw new System.InvalidOperationException("Generated animal definitions failed to import.");
            }

            ConfigureAddressable(frogPrefab, "animals/frog");
            ConfigureAddressable(snakePrefab, "animals/snake");
            CreateMainScene(groundMaterial, frogDefinition, snakeDefinition);

            PlayerSettings.productName = "Zoo World";
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(MainScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Zoo World demo content rebuilt successfully.");
        }

        [MenuItem("Zoo World/Build Windows Verification Player")]
        public static void BuildVerificationPlayer()
        {
            Build();
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult contentResult);
            if (!string.IsNullOrEmpty(contentResult.Error))
            {
                throw new System.InvalidOperationException(contentResult.Error);
            }

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { MainScenePath },
                locationPathName = "Build/Windows/ZooWorld.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development
            });

            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new System.InvalidOperationException(
                    $"Player build failed: {report.summary.result}, {report.summary.totalErrors} error(s).");
            }

            Debug.Log($"Verification player built: {report.summary.totalSize} bytes.");
        }

        private static void EnsureFolders()
        {
            EnsureFolder(Root);
            EnsureFolder(Root + "/Generated");
            EnsureFolder(Materials);
            EnsureFolder(Prefabs);
            EnsureFolder(Configs);
            EnsureFolder(Scenes);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var slash = path.LastIndexOf('/');
            var parent = path.Substring(0, slash);
            var name = path.Substring(slash + 1);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, name);
        }

        private static Material CreateMaterial(string path, Color color)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.color = color;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static GameObject CreateAnimalPrefab(
            string path,
            PrimitiveType primitiveType,
            Material material,
            Vector3 scale)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            var root = GameObject.CreatePrimitive(primitiveType);
            root.name = System.IO.Path.GetFileNameWithoutExtension(path);
            root.transform.localScale = scale;
            root.GetComponent<Renderer>().sharedMaterial = material;

            var body = root.AddComponent<Rigidbody>();
            body.useGravity = false;
            body.mass = 1f;
            body.linearDamping = 0.15f;
            body.angularDamping = 0f;
            body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var actor = root.AddComponent<AnimalActor>();
            var feedback = root.AddComponent<TastyFeedback>();
            var label = CreateTastyLabel(root.transform);

            var feedbackObject = new SerializedObject(feedback);
            feedbackObject.FindProperty("_label").objectReferenceValue = label;
            feedbackObject.ApplyModifiedPropertiesWithoutUndo();

            var actorObject = new SerializedObject(actor);
            actorObject.FindProperty("_tastyFeedback").objectReferenceValue = feedback;
            actorObject.ApplyModifiedPropertiesWithoutUndo();

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return prefab;
        }

        private static TextMesh CreateTastyLabel(Transform parent)
        {
            var labelObject = new GameObject("TastyLabel");
            labelObject.transform.SetParent(parent, false);
            labelObject.transform.localPosition = new Vector3(0f, 0.05f, -1.1f);
            labelObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            labelObject.transform.localScale = Vector3.one;

            var label = labelObject.AddComponent<TextMesh>();
            label.text = "Tasty!";
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.09f;
            label.fontSize = 48;
            label.fontStyle = FontStyle.Bold;
            label.color = new Color(1f, 0.85f, 0.18f);
            return label;
        }

        private static AnimalDefinition CreateDefinition(
            string path,
            string id,
            AnimalDiet diet,
            MovementKind movement,
            float speed,
            float jumpInterval,
            float jumpDistance,
            float jumpDuration,
            float spawnWeight,
            string prefabAddress)
        {
            var definition = AssetDatabase.LoadAssetAtPath<AnimalDefinition>(path);
            if (definition == null)
            {
                definition = ScriptableObject.CreateInstance<AnimalDefinition>();
                AssetDatabase.CreateAsset(definition, path);
            }

            var serialized = new SerializedObject(definition);
            serialized.FindProperty("_id").stringValue = id;
            serialized.FindProperty("_diet").enumValueIndex = (int)diet;
            serialized.FindProperty("_movementKind").enumValueIndex = (int)movement;
            serialized.FindProperty("_moveSpeed").floatValue = speed;
            serialized.FindProperty("_jumpInterval").floatValue = jumpInterval;
            serialized.FindProperty("_jumpDistance").floatValue = jumpDistance;
            serialized.FindProperty("_jumpDuration").floatValue = jumpDuration;
            serialized.FindProperty("_spawnWeight").floatValue = spawnWeight;
            serialized.FindProperty("_prefabAddress").stringValue = prefabAddress;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return definition;
        }

        private static void ConfigureAddressable(GameObject prefab, string address)
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var path = AssetDatabase.GetAssetPath(prefab);
            var guid = AssetDatabase.AssetPathToGUID(path);
            var entry = settings.CreateOrMoveEntry(guid, settings.DefaultGroup);
            entry.address = address;
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, entry, true);
        }

        private static void CreateMainScene(
            Material groundMaterial,
            AnimalDefinition frog,
            AnimalDefinition snake)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera();
            CreateLight();
            CreateGround(groundMaterial);

            var animalRoot = new GameObject("Animals").transform;
            var spawnerObject = new GameObject("AnimalSpawner");
            spawnerObject.AddComponent<AnimalSpawner>();

            var hud = CreateHud();
            var scopeObject = new GameObject("ZooWorldLifetimeScope");
            var scope = scopeObject.AddComponent<ZooWorldLifetimeScope>();
            scope.SetSceneReferences(new[] { frog, snake }, animalRoot);
            EditorUtility.SetDirty(scope);

            Selection.activeGameObject = scopeObject;
            EditorSceneManager.SaveScene(scene, MainScenePath);
        }

        private static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 15f, 0f);
            cameraObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6.5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.09f, 0.12f);
        }

        private static void CreateLight()
        {
            var lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
        }

        private static void CreateGround(Material material)
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.position = new Vector3(0f, -0.35f, 0f);
            ground.transform.localScale = new Vector3(18f, 0.5f, 11f);
            ground.GetComponent<Renderer>().sharedMaterial = material;
        }

        private static HudPresenter CreateHud()
        {
            var canvasObject = new GameObject(
                "HUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            var presenter = canvasObject.AddComponent<HudPresenter>();
            var preyText = CreateHudText(canvasObject.transform, "PreyDeaths", new Vector2(-30f, -30f));
            var predatorText = CreateHudText(canvasObject.transform, "PredatorDeaths", new Vector2(-30f, -75f));
            var presenterData = new SerializedObject(presenter);
            presenterData.FindProperty("_preyDeaths").objectReferenceValue = preyText;
            presenterData.FindProperty("_predatorDeaths").objectReferenceValue = predatorText;
            presenterData.ApplyModifiedPropertiesWithoutUndo();

            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.SetAsLastSibling();
            return presenter;
        }

        private static Text CreateHudText(Transform parent, string name, Vector2 position)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            var rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.one;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one;
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(340f, 42f);

            var text = textObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 28;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleRight;
            text.color = Color.white;
            text.text = name;
            return text;
        }
    }
}
