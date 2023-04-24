using UnityEngine;
using MapSystem.TileLayer;
using UnityEngine.Tilemaps;

namespace MapSystem.ObjectsLayer
{
    [CreateAssetMenu(fileName = "NewObjectGenerateParameters", menuName = "Scriptable Objects/Object Generate Parameters")]
    internal class ObjectGenerateParametersSO : ScriptableObject
    {
        [SerializeField] private Tile[] _tiles;
        [SerializeField] private ObjectType _type;
        [SerializeField] private TileGenerateParametersSO[] _placedOnTilesParameters;
        [SerializeField][Range(0, 100)] private float _fillingPercentage;
        

        public Tile[] Tiles => _tiles;
        public ObjectType Type => _type;
        public int[] PlacedOnTilesWeights
        {
            get
            {
                int[] placedOnTilesWeights = new int[_placedOnTilesParameters.Length];
                for(int i = 0; i < _placedOnTilesParameters.Length; i++)
                    placedOnTilesWeights[i] = _placedOnTilesParameters[i].MinWeight;
                return placedOnTilesWeights;
            }
        }
        public float FillingPercentage => _fillingPercentage / 100;
    }
}