using UnityEngine;

namespace ZooWorld.Game.Features.Animals.Common.Movement
{
    public interface IAnimalMovement
    {
        void Step(float deltaTime);
        void Redirect(Vector3 direction);
        void Stop();
    }
}
