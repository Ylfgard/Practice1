using UnityEngine.Tilemaps;

namespace MapSystem.ObjectsLayer
{
    internal interface IObjectGenerateParameter
    {
        public ContainedType Type { get; }
        public Tile Tile { get; }
        public int[] PlacedOnTilesWeights { get; }
        public float FillingPercentage { get; }
    }
}