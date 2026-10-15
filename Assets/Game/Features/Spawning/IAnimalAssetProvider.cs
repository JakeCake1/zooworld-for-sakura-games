using System;
using UnityEngine;

namespace ZooWorld.Game.Features.Spawning
{
    public interface IAnimalAssetProvider
    {
        void InstantiateAsync(string address, Vector3 position, Transform parent, Action<GameObject> onLoaded);
        void Release(GameObject instance);
    }
}
