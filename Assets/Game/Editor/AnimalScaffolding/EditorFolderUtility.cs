using UnityEditor;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal sealed class EditorFolderUtility
    {
        public void Ensure(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                Create(path);
            }
        }

        private void Create(string path)
        {
            var separator = path.LastIndexOf('/');
            var parent = path.Substring(0, separator);
            Ensure(parent);
            AssetDatabase.CreateFolder(parent, path.Substring(separator + 1));
        }
    }
}
