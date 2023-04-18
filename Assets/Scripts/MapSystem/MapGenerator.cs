using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using System.Collections.Generic;

namespace MapSystem
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private MapDataGenerator _dataGenerator;
        [SerializeField] private TilesKeeper _tileKeeper;
        
        [Inject] private Tilemap _tilemap;

        private List<ConnectingTile> _connectingTiles;
        private int[,] _mapValues;

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
            _connectingTiles = new List<ConnectingTile>();
            _mapValues = new int[width, hight];
            Vector3Int pos = Vector3Int.zero;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < hight; y++)
                {
                    _mapValues[x, y] = _dataGenerator.GetTileNoize(x, y);
                    if (_tileKeeper.TryGetTile(_mapValues[x, y], out Tile tile))
                    { 
                        pos.x = x;
                        pos.y = y;
                        _tilemap.SetTile(pos, tile);
                    }
                    else
                    {
                        _connectingTiles.Add(new ConnectingTile(x, y));
                    }
                }
            }

            ConnectTiles(width, hight);
        }
        
        private void ConnectTiles(int width, int hight)
        {
            bool topLeftSame, topSame, topRightSame,
                leftSame, rightSame,
                bottomLeftSame, bottomSame, bottomRightSame;
            int x, y, value;
            Vector3Int pos = Vector3Int.zero;
            foreach (var tile in _connectingTiles)
            {
                x = tile.X;
                y = tile.Y;
                pos.x = x;
                pos.y = y;
                value = _mapValues[x, y];

                if (x > 0)
                {
                    leftSame = value >= _mapValues[x - 1, y];

                    if (y > 0)
                    {
                        bottomLeftSame = value >= _mapValues[x - 1, y - 1];
                        bottomSame = value >= _mapValues[x, y - 1];

                        if (y < hight - 1)
                        {
                            topLeftSame = value >= _mapValues[x - 1, y + 1];
                            topSame = value >= _mapValues[x, y + 1];

                            if (x < width - 1)
                            {
                                topRightSame = value >= _mapValues[x + 1, y + 1];
                                rightSame = value >= _mapValues[x + 1, y];
                                bottomRightSame = value >= _mapValues[x + 1, y - 1];
                            }
                            else
                            {
                                topRightSame = true;
                                rightSame = true;
                                bottomRightSame = true;
                            }

                            _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                                leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                            continue;
                        }
                        else
                        {
                            topLeftSame = true;
                            topSame = true;
                            topRightSame = true;

                            if (x < width - 1)
                            {
                                rightSame = value >= _mapValues[x + 1, y];
                                bottomRightSame = value >= _mapValues[x + 1, y - 1];
                            }
                            else
                            {
                                rightSame = true;
                                bottomRightSame = true;
                            }

                            _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                                leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                            continue;
                        }
                    }
                    else
                    {
                        bottomLeftSame = true;
                        bottomSame = true;
                        bottomRightSame = true;

                        topLeftSame = value >= _mapValues[x - 1, y + 1];
                        topSame = value >= _mapValues[x, y + 1];

                        if (x < width - 1)
                        {
                            topRightSame = value >= _mapValues[x + 1, y + 1];
                            rightSame = value >= _mapValues[x + 1, y];
                        }
                        else
                        {
                            topRightSame = true;
                            rightSame = true;
                        }

                        _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                            leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                        continue;
                    }
                }
                else
                {
                    topLeftSame = true;
                    leftSame = true;
                    bottomLeftSame = true;
                    
                    rightSame = value >= _mapValues[x + 1, y];

                    if (y > 0)
                    {
                        bottomSame = value >= _mapValues[x, y - 1];

                        if (y < hight - 1)
                        {
                            topSame = value >= _mapValues[x, y + 1];
                            topRightSame = value >= _mapValues[x + 1, y + 1];
                            bottomRightSame = value >= _mapValues[x + 1, y - 1];

                            _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                                leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                            continue;
                        }
                        else
                        {
                            topSame = true;
                            topRightSame = true;
                                
                            bottomRightSame = value >= _mapValues[x + 1, y - 1];

                            _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                                leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                            continue;
                        }
                    }
                    else
                    {
                        bottomSame = true;
                        bottomRightSame = true;

                        topSame = value >= _mapValues[x, y + 1];
                        topRightSame = value >= _mapValues[x + 1, y + 1];

                        _tilemap.SetTile(pos, _tileKeeper.GetConnectingTile(value, topLeftSame, topSame, topRightSame,
                            leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame));
                        continue;
                    }
                }
            }
        }
    }

    public class ConnectingTile
    {
        private int _x;
        private int _y;

        public int X => _x;
        public int Y => _y;

        public ConnectingTile(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}