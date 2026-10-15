using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using ZooWorld.Services.Composition;

namespace ZooWorld.Tests.PlayMode
{
    public sealed class ZooWorldSceneTests
    {
        [UnityTest]
        public IEnumerator MainScene_ContainsCompositionRootAndBrownGround()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            Assert.That(Object.FindFirstObjectByType<ZooWorldLifetimeScope>(), Is.Not.Null);
            AssertGroundIsBrown();
        }

        private static void AssertGroundIsBrown()
        {
            var ground = GameObject.Find("Ground");
            Assert.That(ground, Is.Not.Null);
            var color = ground.GetComponent<Renderer>().sharedMaterial.color;
            Assert.That(color.r, Is.GreaterThan(color.g));
            Assert.That(color.g, Is.GreaterThan(color.b));
        }
    }
}
