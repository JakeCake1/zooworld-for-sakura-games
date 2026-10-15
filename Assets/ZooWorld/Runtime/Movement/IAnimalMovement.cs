using UnityEngine;

namespace ZooWorld.Movement
{
    public interface IAnimalMovement
    {
        void Tick(float deltaTime);
        void Redirect(Vector3 direction);
        void Stop();
    }
}
