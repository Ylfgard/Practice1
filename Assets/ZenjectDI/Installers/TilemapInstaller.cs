using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class TilemapInstaller : MonoInstaller
{
    [SerializeField] private Tilemap _tilemap;

    public override void InstallBindings()
    {
        Container.Bind<Tilemap>().FromInstance(_tilemap).AsSingle();
        Container.QueueForInject(_tilemap);
    }
}