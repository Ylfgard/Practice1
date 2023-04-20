using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    [CreateAssetMenu (fileName = "NewTileGenerateParameters", menuName = "Scriptable Objects/Tile Generate Parameters")]
    internal class TileGenerateParametersSO : TileGenerateParameterNode
    {
        [SerializeField] private Tile[] _tiles;

        public Tile[] Tiles => _tiles;
    }
}