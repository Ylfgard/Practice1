using UnityEngine.Tilemaps;

namespace MapSystem.TileLayer
{
    public class TileData
    {
        public Tile Tile { get; private set; }
        public int Cost { get; private set; }
        public int Weight { get; private set; }
        public bool FirstRun { get; private set; }
        public ChoiceRules ChoiceRules { get; private set; }

        public TileData(int weight, bool firstRun, ChoiceRules choiceRules)
        {
            Weight = weight;
            FirstRun = firstRun;
            ChoiceRules = choiceRules;
        }

        public void SetTile(Tile tile, int cost)
        {
            Tile = tile;
            Cost = cost;
        }

        public void SetTile(int weight, Tile tile, int cost)
        {
            Weight = weight;
            Tile = tile;
            Cost = cost;
        }
    }
}