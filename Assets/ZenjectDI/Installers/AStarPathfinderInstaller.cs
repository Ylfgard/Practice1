using UnityEngine;
using Zenject;
using MapSystem.Pathfinding;

public class AStarPathfinderInstaller : MonoInstaller
{
    [SerializeField] private AStarPathfinder _pathfinder;

    public override void InstallBindings()
    {
        Container.Bind<AStarPathfinder>().FromInstance(_pathfinder).AsSingle();
        Container.QueueForInject(_pathfinder);
    }
}