using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace MapSystem.ObjectsLayer
{
    internal class ObjectGenerateParameter : IObjectGenerateParameter
    {
        public ContainedType Type { get; private set; }
        public Tile Tile => _parameter.Tiles[Random.Range(0, _parameter.Tiles.Length)];
        public int[] PlacedOnTilesWeights => _parameter.PlacedOnTilesWeights;
        public float FillingPercentage { get; private set; }

        

        private ObjectGenerateParametersSO _parameter;

        public ObjectGenerateParameter(ObjectGenerateParametersSO parameter)
        {
            _parameter = parameter;
            FillingPercentage = parameter.FillingPercentage;
            switch (parameter.Type)
            {
                case ObjectType.HighObstacle:
                    Type = ContainedType.HighObstacle;
                    break;

                case ObjectType.LowObstacle:
                    Type = ContainedType.LowObstacle;
                    break;

                default:
                    Debug.LogError("Wrong object type " + parameter.Type);
                    break;
            }
        }

        public void SetFillingPercentage(float fillingPercentage)
        {
            if (fillingPercentage < FillingPercentage)
                FillingPercentage = fillingPercentage;
        }
    }

    internal enum ObjectType
    {
        HighObstacle,
        LowObstacle
    }
}