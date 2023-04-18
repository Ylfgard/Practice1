using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    [CreateAssetMenu (fileName = "NewTileGenerateParameters", menuName = "Scriptable Objects/Tile Generate Parameters")]
    public class TileGenerateParameters : ScriptableObject
    {
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private Tile[] _tiles;

        public int MinValue => _minValue;
        public int MaxValue => _maxValue;
        public Tile[] Tiles => _tiles;
    }
}