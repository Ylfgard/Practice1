using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem.TileLayer
{
    [CreateAssetMenu (fileName = "NewTileGenerateParameters", menuName = "Scriptable Objects/Tile Generate Parameters")]
    internal class TileGenerateParametersSO : ScriptableObject
    {
        [SerializeField] private Tile[] _mainTiles;
        [SerializeField] private TileChoiceRulesSO[] _connectingTiles;
        [SerializeField] protected int _maxWeight;
        [SerializeField] protected TileGenerateParametersSO _prevGenerateParam;
        
        [Header("Balance")]
        [SerializeField] protected int _cost;
        
        public Tile[] MainTiles => _mainTiles;
        public TileChoiceRulesSO[] ConnectingTiles => _connectingTiles;
        public int MaxWeight => _maxWeight;
        public int MinWeight
        {
            get
            {
                if (_prevGenerateParam == null)
                    return 0;
                else
                    return _prevGenerateParam.MaxWeight + 1;
            }
        }
        public int Cost => _cost;
    }
}