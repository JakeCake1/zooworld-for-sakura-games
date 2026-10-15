using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using ZooWorld.Services.Composition;
using ZooWorld.Game.Features.Animals.Common.Presentation;

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
            yield return WaitForSpawn();
            Assert.That(Object.FindFirstObjectByType<Animal>(), Is.Not.Null);
        }

        private void AssertGroundIsBrown()
        {
            var ground = GameObject.Find("Ground");
            Assert.That(ground, Is.Not.Null);
            var color = ground.GetComponent<Renderer>().sharedMaterial.color;
            Assert.That(color.r, Is.GreaterThan(color.g));
            Assert.That(color.g, Is.GreaterThan(color.b));
        }

        private IEnumerator WaitForSpawn()
        {
            var timeout = Time.time + 5f;
            while (Object.FindFirstObjectByType<Animal>() == null && Time.time < timeout)
            {
                yield return null;
            }
        }
    }
}
