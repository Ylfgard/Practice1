using UnityEngine;
using MapSystem;
using Zenject;

public class MapKeeperInstaller : MonoInstaller
{
    [SerializeField] private MapKeeper _mapKeeper;
    
    public override void InstallBindings()
    {
        Container.Bind<MapKeeper>().FromInstance(_mapKeeper).AsSingle();
        Container.QueueForInject(_mapKeeper);
    }
}