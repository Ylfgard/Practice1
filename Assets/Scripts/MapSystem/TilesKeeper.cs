using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace MapSystem
{
    public class TilesKeeper : MonoBehaviour
    {
        [SerializeField] private TileGenerateParameters[] _tileParameters;
        [SerializeField] private ConnectingTileGenerateParameters[] _connectingTilesParameters;

        private Dictionary<int, TileGenerateParameters> _tiles;
        private Dictionary<int, ConnectingTileGenerateParameters> _connectingTiles;

        private void Awake()
        {
            _tiles = new Dictionary<int, TileGenerateParameters>();
            _connectingTiles = new Dictionary<int, ConnectingTileGenerateParameters>();

            GenerateNewTileParameters();    
        }

        [ContextMenu("Generate New Tile Parameters")]
        private void GenerateNewTileParameters()
        {
            _tiles.Clear();
            _connectingTiles.Clear();

            foreach (TileGenerateParameters parameter in _tileParameters)
            {
                for (int i = parameter.MinValue; i <= parameter.MaxValue; i++)
                    _tiles.Add(i, parameter);
            }

            foreach (ConnectingTileGenerateParameters parameter in _connectingTilesParameters)
                _connectingTiles.Add(parameter.Value, parameter);
        }

        public bool TryGetTile(int value, out Tile tile)
        {
            tile = null;
            if (_tiles.TryGetValue(value, out TileGenerateParameters parameters))
            {
                int i = Random.Range(0, parameters.Tiles.Length);
                tile = parameters.Tiles[i];
                return true;
            }
            return false;
        }

        public Tile GetConnectingTile(int value, bool topLeft, bool top, bool topRight, bool left, bool right,
             bool bottomLeft, bool bottom, bool bottomRight)
        {
            TryGetTile(value - 1, out Tile result);

            if (topLeft && top && topRight && left && right && bottomLeft && bottom && bottomRight)
            {
                return result;
            }
                
            else
            {
                _connectingTiles.TryGetValue(value, out ConnectingTileGenerateParameters parameters);
                result = parameters.GetTile(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);
                return result;
            }
        }
    }
}