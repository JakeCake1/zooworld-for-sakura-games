using UnityEditor;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal static class EditorFolderUtility
    {
        public static void Ensure(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                Create(path);
            }
        }

        private static void Create(string path)
        {
            var separator = path.LastIndexOf('/');
            var parent = path.Substring(0, separator);
            Ensure(parent);
            AssetDatabase.CreateFolder(parent, path.Substring(separator + 1));
        }
    }
}
