using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using ZooWorld.Composition;
using ZooWorld.Presentation;

namespace ZooWorld.Tests
{
    public sealed class ZooWorldSceneTests
    {
        [UnityTest]
        public IEnumerator MainScene_ResolvesDependenciesAndSpawnsAddressableAnimal()
        {
            yield return SceneManager.LoadSceneAsync("Main");
            yield return new WaitForSeconds(2.6f);

            Assert.That(Object.FindFirstObjectByType<HudPresenter>(), Is.Not.Null);
            Assert.That(
                Object.FindObjectsByType<AnimalActor>(FindObjectsSortMode.None).Length,
                Is.GreaterThan(0));
        }
    }
}
