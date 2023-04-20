using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

namespace MapSystem
{
    internal class MapConnectionsGenerator : MonoBehaviour
    {
        public Action<int, int, int> SendCost;

        private TileKeeper _tileKeeper;
        private Tilemap _tilemap;

        public void GenerateConnections(int width, int hight, int[,] mapWeight, TileKeeper tileKeeper,
            Tilemap tilemap, List<ConnectingTile> connectingTiles)
        {
            _tileKeeper = tileKeeper;
            _tilemap = tilemap;
            ConnectTiles(width, hight, mapWeight, connectingTiles, true);
        }

        private void ConnectTiles(int width, int hight, int[,] mapWeight, List<ConnectingTile> connectingTiles, bool firstRun)
        {
            bool topLeftSame, topSame, topRightSame,
                leftSame, rightSame,
                bottomLeftSame, bottomSame, bottomRightSame;
            int x, y, weight;
            Vector3Int pos = Vector3Int.zero;
            List<ConnectingTile> restOfConnectingTiles = new List<ConnectingTile>();
            foreach (var tile in connectingTiles)
            {
                x = tile.X;
                y = tile.Y;
                pos.x = x;
                pos.y = y;
                weight = _tileKeeper.TryGetConnectingTileMaxWeight(mapWeight[x, y]);

                leftSame = x <= 0;
                rightSame = x >= width - 1;
                bottomSame = y <= 0;
                topSame = y >= hight - 1;
                topLeftSame = leftSame || topSame;
                topRightSame = rightSame || topSame;
                bottomLeftSame = leftSame || bottomSame;
                bottomRightSame = rightSame || bottomSame;

                if (leftSame == false) leftSame = weight >= mapWeight[x - 1, y];
                if (rightSame == false) rightSame = weight >= mapWeight[x + 1, y];
                if (bottomSame == false) bottomSame = weight >= mapWeight[x, y - 1];
                if (topSame == false) topSame = weight >= mapWeight[x, y + 1];
                if (topLeftSame == false) topLeftSame = weight >= mapWeight[x - 1, y + 1];
                if (topRightSame == false) topRightSame = weight >= mapWeight[x + 1, y + 1];
                if (bottomLeftSame == false) bottomLeftSame = weight >= mapWeight[x - 1, y - 1];
                if (bottomRightSame == false) bottomRightSame = weight >= mapWeight[x + 1, y - 1];

                if (_tileKeeper.TryGetConnectingTile(out Tile tileData, out TileGenerateParameterNode parameter, ref weight,
                    firstRun, topLeftSame, topSame, topRightSame, leftSame, rightSame, bottomLeftSame, bottomSame, bottomRightSame))
                {
                    _tilemap.SetTile(pos, tileData);
                    SendCost?.Invoke(x, y, parameter.Cost);
                    if (firstRun)
                        mapWeight[x, y] = weight;
                }
                else
                {
                    restOfConnectingTiles.Add(tile);
                }
            }

            if (firstRun)
                ConnectTiles(width, hight, mapWeight, restOfConnectingTiles, false);
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