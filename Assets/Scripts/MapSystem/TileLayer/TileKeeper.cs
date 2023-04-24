using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace MapSystem.TileLayer
{
    internal class TileKeeper : MonoBehaviour
    {
        [SerializeField] private TileGenerateParameters[] _tilesParameters;

        private Dictionary<int, TileGenerateParameters> _tiles;

        private void Awake()
        {
            _tiles = new Dictionary<int, TileGenerateParameters>();
            GenerateNewTileParameters();    
        }

        public int TryGetTileMaxWeight(int weight)
        {
            if (_tiles.TryGetValue(weight, out TileGenerateParameters parameter))
                return parameter.MaxWeight;

            Debug.LogError("Wrong weight! " + weight);
            return 0;
        }

        public bool TryGetTile(TileData tileData, out bool weightChanged)
        {
            if (_tiles.TryGetValue(tileData.Weight, out TileGenerateParameters parameter) == false)
            {
                Debug.LogError("Wrong weight " + tileData.Weight);
                weightChanged = false;
                return false;
            }

            var rules = tileData.ChoiceRules;
            int connectedTiles = Convert.ToByte(rules.Top) + Convert.ToByte(rules.Left) +
                Convert.ToByte(rules.Right) + Convert.ToByte(rules.Bottom);

            if (connectedTiles < 2)
            {
                int nextTileWeight = parameter.MaxWeight + 1;
                if (_tiles.TryGetValue(nextTileWeight, out TileGenerateParameters nextParameter))
                {
                    SetMainTile(tileData, nextParameter);
                    weightChanged = true;
                    return true;
                }
                weightChanged = false;
                return false;
            }
            else
            {
                if (tileData.FirstRun == false)
                {
                    connectedTiles += Convert.ToByte(rules.TopLeft) + Convert.ToByte(rules.TopRight) +
                        Convert.ToByte(rules.BottomLeft) + Convert.ToByte(rules.BottomRight);
                    if (connectedTiles != 8 && parameter.TryGetTileConnecting(tileData.ChoiceRules, out Tile tile))
                    {
                        tileData.SetTile(tile, parameter.Cost);
                        weightChanged = false;
                    }
                    else
                    {
                        SetMainTile(tileData, parameter);
                        weightChanged = true;
                    }
                    return true;
                }
                else
                {
                    weightChanged = false;
                    return false;
                } 
            }
        }

        private void SetMainTile(TileData tileData, TileGenerateParameters parameter)
        {
            int i = Random.Range(0, parameter.MainTiles.Length);
            tileData.SetTile(parameter.MinWeight, parameter.MainTiles[i], parameter.Cost);
        }

        [ContextMenu("Generate New Tile Parameters")]
        private void GenerateNewTileParameters()
        {
            _tiles.Clear();

            foreach (var parameter in _tilesParameters)
            {
                parameter.InitializeTiles();
                for (int i = parameter.MinWeight; i <= parameter.MaxWeight; i++)
                {
                    if (_tiles.ContainsKey(i))
                        Debug.LogError("Key " + i + " is taken! Wrong " + parameter.MainTiles[0].name + "generate weight!");
                    else
                        _tiles.Add(i, parameter);
                }
            }
        }
    }
}