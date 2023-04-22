using UnityEngine;
using Zenject;
using MapSystem.Pathfinding;

public class AStarPathfinderInstaller : MonoInstaller
{
    [SerializeField] private Pathfinder _pathfinder;

    public override void InstallBindings()
    {
        Container.Bind<Pathfinder>().FromInstance(_pathfinder).AsSingle();
        Container.QueueForInject(_pathfinder);
    }
}