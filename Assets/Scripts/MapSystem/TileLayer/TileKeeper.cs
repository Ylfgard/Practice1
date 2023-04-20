using UnityEngine;
using System.Collections.Generic;
using System;

using Random = UnityEngine.Random;

namespace MapSystem.TileLayer
{
    internal class TileKeeper : MonoBehaviour
    {
        [SerializeField] private TileGenerateParametersSO[] _tileParameters;
        [SerializeField] private ConnectingTileGenerateParameters[] _connectingTilesParameters;

        private Dictionary<int, TileGenerateParametersSO> _tiles;
        private Dictionary<int, ConnectingTileGenerateParameters> _connectingTiles;

        private void Awake()
        {
            _tiles = new Dictionary<int, TileGenerateParametersSO>();
            _connectingTiles = new Dictionary<int, ConnectingTileGenerateParameters>();

            GenerateNewTileParameters();    
        }

        [ContextMenu("Generate New Tile Parameters")]
        private void GenerateNewTileParameters()
        {
            _tiles.Clear();
            _connectingTiles.Clear();

            foreach (var parameter in _tileParameters)
            {
                for (int i = parameter.MinWeight; i <= parameter.MaxWeight; i++)
                {
                    if (_tiles.ContainsKey(i))
                        Debug.LogError("Key " + i + " is taken! Wrong " + parameter.name + "generate weight!");
                    else
                        _tiles.Add(i, parameter);
                }
            }

            foreach (var connectingTile in _connectingTilesParameters)
            {
                connectingTile.InitializeConnectingTiles();
                for (int i = connectingTile.Parameters.MinWeight; i <= connectingTile.Parameters.MaxWeight; i++)
                {
                    if (_tiles.ContainsKey(i))
                        Debug.LogError("Key " + i + " is taken! Wrong " + connectingTile.Parameters.name + "generate weight!");
                    else
                        _connectingTiles.Add(i, connectingTile);
                }
            }
        }

        public bool TryGetTile(TileData tileData)
        {
            if (_tiles.TryGetValue(tileData.Weight, out TileGenerateParametersSO parameters))
            {
                int i = Random.Range(0, parameters.Tiles.Length);
                tileData.SetTile(parameters.Tiles[i], parameters.Cost);
                return true;
            }
            return false;
        }

        public int TryGetConnectingTileMaxWeight(int weight)
        {
            if (_connectingTiles.TryGetValue(weight, out ConnectingTileGenerateParameters connectingTile))
                return connectingTile.Parameters.MaxWeight;

            Debug.LogError("Wrong weight! " + weight);
            return 0;
        }

        public bool TryGetConnectingTile(ConnectingTileData tileData)
        {
            _connectingTiles.TryGetValue(tileData.Weight, out ConnectingTileGenerateParameters connectingTile);

            var rules = tileData.ChoiceRules;
            int connectedTiles = Convert.ToByte(rules.Top) + Convert.ToByte(rules.Left) +
                Convert.ToByte(rules.Right) + Convert.ToByte(rules.Bottom);

            if (connectedTiles < 2)
            {
                tileData.Weight = connectingTile.Parameters.MaxWeight + 1;
                TryGetTile(tileData);
                return true;
            }
            else
            {
                connectedTiles += Convert.ToByte(rules.TopLeft) + Convert.ToByte(rules.TopRight) +
                    Convert.ToByte(rules.BottomLeft) + Convert.ToByte(rules.BottomRight);
                if (connectedTiles == 8)
                {
                    tileData.Weight = connectingTile.Parameters.MinWeight - 1;
                    TryGetTile(tileData);
                    return true;
                }
                else if (tileData.FirstRun == false)
                {
                    tileData.SetTile(connectingTile.GetTile(tileData.ChoiceRules), connectingTile.Parameters.Cost);
                    return true;
                }
                else
                {
                    return false;
                } 
            }
        }
    }
}