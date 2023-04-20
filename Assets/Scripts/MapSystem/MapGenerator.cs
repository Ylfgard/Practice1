using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

namespace MapSystem
{
    public class MapGenerator : MonoBehaviour
    {
        public Action<int, int> GenerationStarted;
        public Action<int, int, int> SendCost;

        [SerializeField] private MapConnectionsGenerator _connectionsGenerator;
        [SerializeField] private MapDataGenerator _dataGenerator;
        [SerializeField] private TileKeeper _tileKeeper;

        [Inject] private Tilemap _tilemap;

        private void Start()
        {
            GenerateNewMap();
        }

        [ContextMenu("Generate New Map")]
        private void GenerateNewMap()
        {
            Random.InitState(_dataGenerator.Seed);
            GenerateMap(_dataGenerator.Width, _dataGenerator.Hight);
        }

        private void GenerateMap(int width, int hight)
        {
            _tilemap.ClearAllTiles();
            List<ConnectingTile> connectingTiles = new List<ConnectingTile>();
            GenerationStarted?.Invoke(width, hight);
            int[,] mapWeight = new int[width, hight];
            Vector3Int pos = Vector3Int.zero;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < hight; y++)
                {
                    mapWeight[x, y] = _dataGenerator.GetTileNoize(x, y);
                    if (_tileKeeper.TryGetTile(mapWeight[x, y], out Tile tileData, out TileGenerateParameterNode parameter))
                    {
                        pos.x = x;
                        pos.y = y;
                        _tilemap.SetTile(pos, tileData);
                        SendCost?.Invoke(x, y, parameter.Cost);
                    }
                    else
                    {
                        connectingTiles.Add(new ConnectingTile(x, y));
                    }
                }
            }
            _connectionsGenerator.GenerateConnections(width, hight, mapWeight, _tileKeeper, _tilemap, connectingTiles);
        }
    }
}