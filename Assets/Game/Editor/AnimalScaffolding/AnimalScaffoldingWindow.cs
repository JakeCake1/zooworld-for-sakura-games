using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.AnimalScaffolding
{
    public sealed class AnimalScaffoldingWindow : EditorWindow
    {
        private const string DefaultRoot = "Assets/Game/Features/Animals";
        private static readonly Regex IdentifierPattern = new Regex("^[A-Za-z_][A-Za-z0-9_]*$");
        private string _animalName = "NewAnimal";
        private string _rootFolder = DefaultRoot;

        [MenuItem("Assets/Create/Zoo World/Animal...", false, 80)]
        private static void Open()
        {
            var window = GetWindow<AnimalScaffoldingWindow>(true, "Create Animal");
            window._rootFolder = SelectedRoot();
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
            EditorGUILayout.HelpBox("Creates Scripts and Content folders plus DTO, Definition, Movement, and Actor boilerplate.", MessageType.Info);
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
            return IdentifierPattern.IsMatch(_animalName) && AssetDatabase.IsValidFolder(_rootFolder);
        }

        private void CreateFeature()
        {
            var featureFolder = $"{_rootFolder}/{_animalName}";
            EnsureFeatureDoesNotExist(featureFolder);
            CreateFolders(featureFolder);
            WriteBoilerplate(featureFolder);
            FinishCreation(featureFolder);
        }

        private static void EnsureFeatureDoesNotExist(string featureFolder)
        {
            if (AssetDatabase.IsValidFolder(featureFolder))
            {
                throw new IOException($"Animal feature already exists: {featureFolder}");
            }
        }

        private static void CreateFolders(string featureFolder)
        {
            EditorFolderUtility.Ensure(featureFolder + "/Scripts");
            EditorFolderUtility.Ensure(featureFolder + "/Content");
        }

        private void WriteBoilerplate(string featureFolder)
        {
            var scripts = featureFolder + "/Scripts";
            Write(scripts, _animalName + "ConfigDto.cs", AnimalScaffoldingTemplates.ConfigDto(_animalName));
            Write(scripts, _animalName + "Definition.cs", AnimalScaffoldingTemplates.Definition(_animalName));
            Write(scripts, _animalName + "Movement.cs", AnimalScaffoldingTemplates.Movement(_animalName));
            Write(scripts, _animalName + ".cs", AnimalScaffoldingTemplates.Actor(_animalName));
        }

        private static void Write(string folder, string fileName, string content)
        {
            File.WriteAllText(Path.GetFullPath($"{folder}/{fileName}"), content);
        }

        private void FinishCreation(string featureFolder)
        {
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(featureFolder);
            Close();
        }

        private static string SelectedRoot()
        {
            var selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            return AssetDatabase.IsValidFolder(selectedPath) ? selectedPath : DefaultRoot;
        }
    }
}
