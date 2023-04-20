using UnityEngine.Tilemaps;

namespace MapSystem.TileLayer
{
    public class TileData
    {
        protected Tile _tile;
        protected int _cost;
        
        public int Weight;

        public Tile Tile => _tile;
        public int Cost => _cost;

        public TileData(int weight)
        {
            Weight = weight;
        }

        public void SetTile(Tile tile, int cost)
        {
            _tile = tile;
            _cost = cost;
        }
    }
}