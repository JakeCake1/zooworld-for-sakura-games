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
    internal sealed class SceneContentBuilder
    {
        public void Build(Material groundMaterial)
        {
            var scene = CreateEmptyScene();
            CreateWorld(groundMaterial);
            var animalRoot = new GameObject("Animals").transform;
            CreateSpawner();
            CreateHud();
            CreateCompositionRoot(animalRoot);
            EditorSceneManager.SaveScene(scene, ZooWorldProjectPaths.MainScene);
        }

        private Scene CreateEmptyScene()
        {
            return EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private void CreateWorld(Material groundMaterial)
        {
            CreateCamera();
            CreateLight();
            CreateGround(groundMaterial);
        }

        private void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            ConfigureCameraTransform(cameraObject.transform);
            ConfigureCamera(cameraObject.AddComponent<Camera>());
            cameraObject.tag = "MainCamera";
        }

        private void ConfigureCameraTransform(Transform cameraTransform)
        {
            cameraTransform.position = new Vector3(0f, 15f, 0f);
            cameraTransform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        private void ConfigureCamera(Camera camera)
        {
            camera.orthographic = true;
            camera.orthographicSize = 6.5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.06f, 0.09f, 0.12f);
        }

        private void CreateLight()
        {
            var lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            ConfigureLight(lightObject.AddComponent<Light>());
        }

        private void ConfigureLight(Light light)
        {
            light.type = LightType.Directional;
            light.intensity = 1.2f;
        }

        private void CreateGround(Material material)
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.position = new Vector3(0f, -0.35f, 0f);
            ground.transform.localScale = new Vector3(18f, 0.5f, 11f);
            ground.GetComponent<Renderer>().sharedMaterial = material;
        }

        private void CreateSpawner()
        {
            new GameObject("AnimalSpawner").AddComponent<AnimalSpawner>();
        }

        private void CreateCompositionRoot(Transform animalRoot)
        {
            var scopeObject = new GameObject("ZooWorldLifetimeScope");
            var scope = scopeObject.AddComponent<ZooWorldLifetimeScope>();
            scope.SetSceneReferences(animalRoot);
            EditorUtility.SetDirty(scope);
            Selection.activeGameObject = scopeObject;
        }

        private void CreateHud()
        {
            var canvas = CreateCanvas();
            var view = canvas.gameObject.AddComponent<HudView>();
            AssignHudTexts(view, canvas.transform);
            CreateEventSystem();
        }

        private Canvas CreateCanvas()
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

        private void ConfigureScaler(CanvasScaler scaler)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        private void AssignHudTexts(HudView view, Transform parent)
        {
            var serialized = new SerializedObject(view);
            serialized.FindProperty("_preyDeaths").objectReferenceValue = CreateHudText(parent, "PreyDeaths", new Vector2(-30f, -30f));
            serialized.FindProperty("_predatorDeaths").objectReferenceValue = CreateHudText(parent, "PredatorDeaths", new Vector2(-30f, -75f));
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private Text CreateHudText(Transform parent, string name, Vector2 position)
        {
            var textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            ConfigureRect(textObject.GetComponent<RectTransform>(), position);
            return ConfigureText(textObject.GetComponent<Text>());
        }

        private void ConfigureRect(RectTransform rect, Vector2 position)
        {
            rect.anchorMin = Vector2.one;
            rect.anchorMax = Vector2.one;
            rect.pivot = Vector2.one;
            rect.anchoredPosition = position;
            rect.sizeDelta = new Vector2(340f, 42f);
        }

        private Text ConfigureText(Text text)
        {
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 28;
            text.fontStyle = FontStyle.Bold;
            text.alignment = TextAnchor.MiddleRight;
            text.color = Color.white;
            return text;
        }

        private void CreateEventSystem()
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }
    }
}
