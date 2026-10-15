using UnityEditor;

namespace ZooWorld.Editor.AnimalScaffolding
{
    internal static class AnimalScaffoldingContinuation
    {
        private const string AnimalNameKey = "ZooWorld.AnimalScaffolding.AnimalName";
        private const string FeatureFolderKey = "ZooWorld.AnimalScaffolding.FeatureFolder";

        public static void Schedule(string animalName, string featureFolder)
        {
            SessionState.SetString(AnimalNameKey, animalName);
            SessionState.SetString(FeatureFolderKey, featureFolder);
        }

        [InitializeOnLoadMethod]
        private static void ResumeAfterCompilation()
        {
            var animalName = SessionState.GetString(AnimalNameKey, string.Empty);
            if (string.IsNullOrEmpty(animalName))
            {
                return;
            }

            CreateContent(animalName);
        }

        private static void CreateContent(string animalName)
        {
            var featureFolder = SessionState.GetString(FeatureFolderKey, string.Empty);
            ClearRequest();
            new AnimalScaffoldingContentBuilder().Create(animalName, featureFolder);
        }

        private static void ClearRequest()
        {
            SessionState.EraseString(AnimalNameKey);
            SessionState.EraseString(FeatureFolderKey);
        }
    }
}
