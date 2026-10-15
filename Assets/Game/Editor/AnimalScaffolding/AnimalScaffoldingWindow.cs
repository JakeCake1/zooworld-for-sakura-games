using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.AnimalScaffolding
{
    public sealed class AnimalScaffoldingWindow : EditorWindow
    {
        private const string DefaultRoot = "Assets/Game/Features/Animals";
        private readonly Regex _identifierPattern = new Regex("^[A-Za-z_][A-Za-z0-9_]*$");
        private readonly AnimalScaffoldingTemplates _templates = new AnimalScaffoldingTemplates();
        private readonly EditorFolderUtility _folderUtility = new EditorFolderUtility();
        private string _animalName = "NewAnimal";
        private string _rootFolder = DefaultRoot;

        [MenuItem("Assets/Create/Zoo World/Animal...", false, 80)]
        private static void Open()
        {
            var window = GetWindow<AnimalScaffoldingWindow>(true, "Create Animal");
            window.InitializeRootFolder();
            window.Show();
        }

        private void OnGUI()
        {
            DrawFields();
            DrawCreateButton();
        }

        private void DrawFields()
        {
            EditorGUILayout.LabelField("Animal feature", EditorStyles.boldLabel);
            _animalName = EditorGUILayout.TextField("Class name", _animalName.Trim());
            _rootFolder = EditorGUILayout.TextField("Root folder", _rootFolder);
            EditorGUILayout.HelpBox("Creates species scripts, material, cube prefab, Definition asset, and Addressables entries.", MessageType.Info);
        }

        private void DrawCreateButton()
        {
            using (new EditorGUI.DisabledScope(!IsValid()))
            {
                if (GUILayout.Button("Create Animal Feature"))
                {
                    CreateFeature();
                }
            }
        }

        private bool IsValid()
        {
            return _identifierPattern.IsMatch(_animalName) && AssetDatabase.IsValidFolder(_rootFolder);
        }

        private void CreateFeature()
        {
            var featureFolder = $"{_rootFolder}/{_animalName}";
            EnsureFeatureDoesNotExist(featureFolder);
            CreateFolders(featureFolder);
            WriteBoilerplate(featureFolder);
            AnimalScaffoldingContinuation.Schedule(_animalName, featureFolder);
            FinishCreation(featureFolder);
        }

        private void EnsureFeatureDoesNotExist(string featureFolder)
        {
            if (AssetDatabase.IsValidFolder(featureFolder))
            {
                throw new IOException($"Animal feature already exists: {featureFolder}");
            }
        }

        private void CreateFolders(string featureFolder)
        {
            _folderUtility.Ensure(featureFolder + "/Scripts");
            _folderUtility.Ensure(featureFolder + "/Content");
        }

        private void WriteBoilerplate(string featureFolder)
        {
            var scripts = featureFolder + "/Scripts";
            Write(scripts, _animalName + "ConfigDto.cs", _templates.ConfigDto(_animalName));
            Write(scripts, _animalName + "Definition.cs", _templates.Definition(_animalName));
            Write(scripts, _animalName + "Movement.cs", _templates.Movement(_animalName));
            Write(scripts, _animalName + ".cs", _templates.Animal(_animalName));
        }

        private void Write(string folder, string fileName, string content)
        {
            File.WriteAllText(Path.GetFullPath($"{folder}/{fileName}"), content);
        }

        private void FinishCreation(string featureFolder)
        {
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(featureFolder);
            Close();
        }

        private void InitializeRootFolder()
        {
            _rootFolder = SelectedRoot();
        }

        private string SelectedRoot()
        {
            var selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            return AssetDatabase.IsValidFolder(selectedPath) ? selectedPath : DefaultRoot;
        }
    }
}
