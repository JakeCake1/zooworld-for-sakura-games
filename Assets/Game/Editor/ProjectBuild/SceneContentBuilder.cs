using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZooWorld.Game.Features.Animals.Common.Data;
using ZooWorld.Game.Features.Spawning;
using ZooWorld.Services.Composition;
using ZooWorld.Ui.Hud;

namespace ZooWorld.Editor.ProjectBuild
{
    // Builds only scene-owned objects; gameplay and infrastructure live in their own assemblies.
    internal static class SceneContentBuilder
    {
        public static void Build(Material groundMaterial, AnimalDefinition frog, AnimalDefinition snake)
        {
            var scene = CreateEmptyScene();
            CreateWorld(groundMaterial);
            var animalRoot = new GameObject("Animals").transform;
            CreateSpawner();
            CreateHud();
            CreateCompositionRoot(new[] { frog, snake }, animalRoot);
            EditorSceneManager.SaveScene(scene, ZooWorldProjectPaths.MainScene);
        }

        private static Scene CreateEmptyScene()
        {
            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private static void CreateWorld(Material groundMaterial)
        {
            CreateCamera();
            CreateLight();
            CreateGround(groundMaterial);
        }

        private static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            ConfigureCameraTransform(cameraObject.transform);
            ConfigureCamera(cameraObject.AddComponent<Camera>());
            cameraObject.tag = "MainCamera";
        }

        private static void ConfigureCameraTransform(Transform cameraTransform)
        {
            cameraTransform.position = new Vector3(0f, 15f, 0f);
            cameraTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        private static void ConfigureCamera(Camera camera)
        {
            camera.orthographic = true;
            camera.orthographicSize = 6.5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.09f, 0.12f);
        }

        private static void CreateLight()
        {
            var lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            ConfigureLight(lightObject.AddComponent<Light>());
        }

        private static void ConfigureLight(Light light)
        {
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

        private static void CreateSpawner()
        {
            new GameObject("AnimalSpawner").AddComponent<AnimalSpawner>();
        }

        private static void CreateCompositionRoot(AnimalDefinition[] definitions, Transform animalRoot)
        {
            var scopeObject = new GameObject("ZooWorldLifetimeScope");
            var scope = scopeObject.AddComponent<ZooWorldLifetimeScope>();
            scope.SetSceneReferences(definitions, animalRoot);
            EditorUtility.SetDirty(scope);
            Selection.activeGameObject = scopeObject;
        }

        private static void CreateHud()
        {
            var canvas = CreateCanvas();
            var presenter = canvas.gameObject.AddComponent<HudPresenter>();
            AssignHudTexts(presenter, canvas.transform);
            CreateEventSystem();
        }

        private static Canvas CreateCanvas()
        {
            var canvasObject = new GameObject(
                "HUD",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            ConfigureScaler(canvasObject.GetComponent<CanvasScaler>());
            return canvas;
        }

        private static void ConfigureScaler(CanvasScaler scaler)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        private static void AssignHudTexts(HudPresenter presenter, Transform parent)
        {
            var serialized = new SerializedObject(presenter);
            serialized.FindProperty("_preyDeaths").objectReferenceValue = CreateHudText(parent, "PreyDeaths", new Vector2(-30f, -30f));
            serialized.FindProperty("_predatorDeaths").objectReferenceValue = CreateHudText(parent, "PredatorDeaths", new Vector2(-30f, -75f));
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Text CreateHudText(Transform parent, string name, Vector2 position)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            ConfigureRect(textObject.GetComponent<RectTransform>(), position);
            return ConfigureText(textObject.GetComponent<Text>());
        }

        private static void ConfigureRect(RectTransform rect, Vector2 position)
        {
            rect.anchorMin = Vector2.one;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one;
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(340f, 42f);
        }

        private static Text ConfigureText(Text text)
        {
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 28;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleRight;
            text.color = Color.white;
            return text;
        }

        private static void CreateEventSystem()
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
}
