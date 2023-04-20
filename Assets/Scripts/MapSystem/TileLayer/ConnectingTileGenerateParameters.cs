using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
    
namespace MapSystem.TileLayer
{
    [Serializable]
    internal class ConnectingTileGenerateParameters
    {
        [SerializeField] private ConnectingTileGenerateParametersSO _parameters;

        private Dictionary<int, Tile> _tiles;

        public ConnectingTileGenerateParametersSO Parameters => _parameters;

        public void InitializeConnectingTiles()
        {
            _tiles = new Dictionary<int, Tile>();

            List<int> keys;
            foreach (var tileChoiceRules in _parameters.TilesChoiceRiles)
            {
                keys = tileChoiceRules.ChoiceRules.GetKeys();

                foreach(int key in keys)
                {
                    if (_tiles.ContainsKey(key)) Debug.LogError("Wrong rules! " + tileChoiceRules.name);
                    else _tiles.Add(key, tileChoiceRules.Tile);
                }
            }
        }

        public Tile GetTile(ChoiceRules choiceRules)
        {
            int key = choiceRules.GetKey();

            if (_tiles.TryGetValue(key, out Tile result))
            {
                return result;
            }
            else
            {
                Debug.Log("Wrong key: " + key);
                return _parameters.TilesChoiceRiles[0].Tile;
            }
        }
    }
}