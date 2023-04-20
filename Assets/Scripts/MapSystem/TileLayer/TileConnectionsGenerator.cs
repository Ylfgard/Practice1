using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

namespace MapSystem.TileLayer
{
    internal class TileConnectionsGenerator : MonoBehaviour
    {
        public Action<int, int, int> SendCost;

        private TileKeeper _tileKeeper;
        private Tilemap _tilemap;

        public void GenerateConnections(MapData mapData)
        {
            _tileKeeper = mapData.TileKeeper;
            _tilemap = mapData.Tilemap;
            ConnectTiles(mapData.Width, mapData.Hight, mapData.MapWeight, mapData.ConnectingTiles, true);
        }

        private void ConnectTiles(int width, int hight, int[,] mapWeight, List<Vector2Int> connectingTiles, bool firstRun)
        {
            bool topLeftSame, topSame, topRightSame,
                leftSame, rightSame,
                bottomLeftSame, bottomSame, bottomRightSame;
            int x, y, weight;
            Vector3Int pos = Vector3Int.zero;
            List<Vector2Int> restOfConnectingTiles = new List<Vector2Int>();

            foreach (var tile in connectingTiles)
            {
                x = tile.x;
                y = tile.y;
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

                ChoiceRules choiceRules = new ChoiceRules(topLeftSame, topSame, topRightSame, leftSame,
                    rightSame, bottomLeftSame, bottomSame, bottomRightSame);
                ConnectingTileData tileData = new ConnectingTileData(weight, choiceRules, firstRun);
                if (_tileKeeper.TryGetConnectingTile(tileData))
                {
                    _tilemap.SetTile(pos, tileData.Tile);
                    SendCost?.Invoke(x, y, tileData.Cost);
                    if (firstRun)
                        mapWeight[x, y] = tileData.Weight;
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
}