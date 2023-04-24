using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    public class TilemapKeeper : MonoBehaviour
    {
        [SerializeField] private Tilemap _tileLayer;
        [SerializeField] private Tilemap _objectsLayer;

        public Tilemap Tiles => _tileLayer;
        public Tilemap Objects => _objectsLayer;
    }
}