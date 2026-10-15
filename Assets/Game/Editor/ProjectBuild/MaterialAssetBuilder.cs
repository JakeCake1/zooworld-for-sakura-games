using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.ProjectBuild
{
    internal static class MaterialAssetBuilder
    {
        public static Material Create(string path, Color color)
        {
            var material = LoadOrCreate(path);
            ApplyColor(material, color);
            return material;
        }

        private static Material LoadOrCreate(string path)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            return material != null ? material : CreateAsset(path);
        }

        private static Material CreateAsset(string path)
        {
            var material = new Material(FindShader());
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static Shader FindShader()
        {
            return Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        }

        private static void ApplyColor(Material material, Color color)
        {
            material.color = color;
            EditorUtility.SetDirty(material);
        }
    }
}
