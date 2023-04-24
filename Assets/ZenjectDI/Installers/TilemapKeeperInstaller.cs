using UnityEngine;
using Zenject;
using MapSystem;

public class TilemapKeeperInstaller : MonoInstaller
{
    [SerializeField] private TilemapKeeper _tilemapKeeper;

    public override void InstallBindings()
    {
        Container.Bind<TilemapKeeper>().FromInstance(_tilemapKeeper).AsSingle();
        Container.QueueForInject(_tilemapKeeper);
    }
}