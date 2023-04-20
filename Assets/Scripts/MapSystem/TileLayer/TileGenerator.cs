using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

namespace MapSystem.TileLayer
{
    internal class TileGenerator : MonoBehaviour
    {
        public Action<int, int> GenerationStarted;
        public Action<int, int, int> SendCost;

        [SerializeField] private TileConnectionsGenerator _connectionsGenerator;
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
            List<Vector2Int> connectingTiles = new List<Vector2Int>();
            GenerationStarted?.Invoke(width, hight);
            int[,] mapWeight = new int[width, hight];
            Vector3Int pos = Vector3Int.zero;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < hight; y++)
                {
                    mapWeight[x, y] = _dataGenerator.GetTileNoize(x, y);

                    TileData tileData = new TileData(mapWeight[x, y]);
                    if (_tileKeeper.TryGetTile(tileData))
                    {
                        pos.x = x;
                        pos.y = y;
                        _tilemap.SetTile(pos, tileData.Tile);
                        SendCost?.Invoke(x, y, tileData.Cost);
                    }
                    else
                    {
                        connectingTiles.Add(new Vector2Int(x, y));
                    }
                }
            }

            MapData mapData = new MapData(width, hight, mapWeight, _tileKeeper, _tilemap, connectingTiles);
            _connectionsGenerator.GenerateConnections(mapData);
        }
    }

    internal struct MapData
    {
        public int Width { get; }
        public int Hight { get; }
        public int[,] MapWeight { get; }
        public TileKeeper TileKeeper { get; }
        public Tilemap Tilemap { get; }
        public List<Vector2Int> ConnectingTiles { get; }

        public MapData(int width, int hight, int[,] mapWeight, TileKeeper tileKeeper,
            Tilemap tilemap, List<Vector2Int> connectingTiles)
        {
            Width = width;
            Hight = hight;
            MapWeight = mapWeight;
            TileKeeper = tileKeeper;
            Tilemap = tilemap;
            ConnectingTiles = connectingTiles;
        }
    }
}