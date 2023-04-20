using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

namespace MapSystem
{
    internal class TileKeeper : MonoBehaviour
    {
        [SerializeField] private TileGenerateParametersSO[] _tileParameters;
        [SerializeField] private ConnectingTileGenerateParametersSO[] _connectingTilesParameters;

        private Dictionary<int, TileGenerateParametersSO> _tiles;
        private Dictionary<int, ConnectingTileGenerateParametersSO> _connectingTiles;

        private void Awake()
        {
            _tiles = new Dictionary<int, TileGenerateParametersSO>();
            _connectingTiles = new Dictionary<int, ConnectingTileGenerateParametersSO>();

            GenerateNewTileParameters();    
        }

        [ContextMenu("Generate New Tile Parameters")]
        private void GenerateNewTileParameters()
        {
            _tiles.Clear();
            _connectingTiles.Clear();

            foreach (TileGenerateParametersSO parameter in _tileParameters)
            {
                for (int i = parameter.MinWeight; i <= parameter.MaxWeight; i++)
                {
                    if (_tiles.ContainsKey(i))
                        Debug.LogError("Key " + i + " is taken! Wrong " + parameter.name + "generate weight!");
                    else
                        _tiles.Add(i, parameter);
                }
            }

            foreach (ConnectingTileGenerateParametersSO parameter in _connectingTilesParameters)
            {
                parameter.InitializeConnectingTiles();
                for (int i = parameter.MinWeight; i <= parameter.MaxWeight; i++)
                {
                    if (_tiles.ContainsKey(i))
                        Debug.LogError("Key " + i + " is taken! Wrong " + parameter.name + "generate weight!");
                    else
                        _connectingTiles.Add(i, parameter);
                }
            }
        }

        public bool TryGetTile(int value, out Tile tile, out TileGenerateParameterNode parameter)
        {
            tile = null;
            parameter = null;
            if (_tiles.TryGetValue(value, out TileGenerateParametersSO parameters))
            {
                int i = Random.Range(0, parameters.Tiles.Length);
                tile = parameters.Tiles[i];
                parameter = parameters;
                return true;
            }
            return false;
        }

        public int TryGetConnectingTileMaxWeight(int value)
        {
            if (_connectingTiles.TryGetValue(value, out ConnectingTileGenerateParametersSO parameters))
                return parameters.MaxWeight;

            Debug.LogError("Wrong value! " + value);
            return 0;
        }

        public bool TryGetConnectingTile(out Tile tile, out TileGenerateParameterNode parameter, ref int weight, bool firstRun,
            bool topLeft, bool top, bool topRight, bool left, bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            _connectingTiles.TryGetValue(weight, out ConnectingTileGenerateParametersSO parameters);
            
            int connectedTiles = Convert.ToInt16(top) + Convert.ToInt16(left) +
                Convert.ToInt16(right) + Convert.ToInt16(bottom);

            if (connectedTiles < 2)
            {
                weight = parameters.MaxWeight + 1;
                TryGetTile(weight, out tile, out parameter);
                return true;
            }
            else
            {
                connectedTiles += Convert.ToInt16(topLeft) + Convert.ToInt16(topRight) +
                    Convert.ToInt16(bottomLeft) + Convert.ToInt16(bottomRight);
                if (connectedTiles == 8)
                {
                    weight = parameters.MinWeight - 1;
                    TryGetTile(weight, out tile, out parameter);
                    return true;
                }
                else if (firstRun == false)
                {
                    tile = parameters.GetTile(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);
                    parameter = parameters;
                    return true;
                }
                else
                {
                    tile = null;
                    parameter = null;
                    return false;
                } 
            }
        }
    }
}