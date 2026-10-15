using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools.Utils;
using ZooWorld.Editor.AnimalScaffolding;

namespace ZooWorld.Tests.EditMode
{
    public sealed class AnimalAuthoringTests
    {
        private const string FrogPrefabPath = "Assets/Game/Features/Animals/Frog/Content/Frog.prefab";
        private const string SnakePrefabPath = "Assets/Game/Features/Animals/Snake/Content/Snake.prefab";

        [Test]
        public void GeneratedDefinition_HasCreateAssetMenu()
        {
            var source = AnimalScaffoldingTemplates.Definition("Bear");
            Assert.That(source, Does.Contain("[CreateAssetMenu"));
            Assert.That(source, Does.Contain("Zoo World/Animals/Bear Definition"));
        }

        [Test]
        public void Snake_IsFrogShapeScaledByOnePointTwo()
        {
            var frogScale = LoadScale(FrogPrefabPath);
            var snakeScale = LoadScale(SnakePrefabPath);
            Assert.That(snakeScale, Is.EqualTo(frogScale * 1.2f).Using(Vector3ComparerWithEqualsOperator.Instance));
        }

        private static Vector3 LoadScale(string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Assert.That(prefab, Is.Not.Null, $"Prefab not found: {path}");
            return prefab.transform.localScale;
        }
    }
}
