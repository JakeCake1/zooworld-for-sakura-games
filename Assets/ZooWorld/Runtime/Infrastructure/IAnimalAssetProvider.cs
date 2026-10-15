using System;
using UnityEngine;

namespace ZooWorld.Infrastructure
{
    public interface IAnimalAssetProvider
    {
        void InstantiateAsync(string address, Vector3 position, Transform parent, Action<GameObject> onLoaded);
        void Release(GameObject instance);
    }
}
