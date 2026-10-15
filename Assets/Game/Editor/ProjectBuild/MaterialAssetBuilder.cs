using UnityEditor;
using UnityEngine;

namespace ZooWorld.Editor.ProjectBuild
{
    internal sealed class MaterialAssetBuilder
    {
        public Material Create(string path, Color color)
        {
            var material = LoadOrCreate(path);
            ApplyColor(material, color);
            return material;
        }

        private Material LoadOrCreate(string path)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            return material != null ? material : CreateAsset(path);
        }

        private Material CreateAsset(string path)
        {
            var material = new Material(FindShader());
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private Shader FindShader()
        {
            return Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        }

        private void ApplyColor(Material material, Color color)
        {
            material.color = color;
            EditorUtility.SetDirty(material);
        }
    }
}
