using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
    
namespace MapSystem.TileLayer
{
    [Serializable]
    internal class TileGenerateParameters
    {
        [SerializeField] private TileGenerateParametersSO _parameters;

        private Dictionary<int, Tile> _tiles;

        public Tile[] MainTiles => _parameters.MainTiles;
        public int MaxWeight => _parameters.MaxWeight;
        public int MinWeight => _parameters.MinWeight;
        public int Cost => _parameters.Cost;

        public void InitializeTiles()
        {
            _tiles = new Dictionary<int, Tile>();

            List<int> keys;
            foreach (var tileChoiceRules in _parameters.ConnectingTiles)
            {
                keys = tileChoiceRules.ChoiceRules.GetKeys();

                foreach(int key in keys)
                {
                    if (_tiles.ContainsKey(key)) Debug.LogError("Wrong rules! " + tileChoiceRules.name);
                    else _tiles.Add(key, tileChoiceRules.Tile);
                }
            }
        }

        public bool TryGetTileConnecting(ChoiceRules choiceRules, out Tile tile)
        {
            int key = choiceRules.GetKey();

            if (_tiles.TryGetValue(key, out tile))
            {
                return true;
            }
            else
            {
                string binaryKey = "";
                TileRule[] tileRules = choiceRules.GetKeyRulesArray();
                foreach (var rule in tileRules)
                {
                    if (rule == TileRule.Same) binaryKey += "1";
                    else binaryKey += "0";
                }
                    
                Debug.Log("Missing key: " + binaryKey + " in " + _parameters.name);
                return false;
            }
        }
    }
}