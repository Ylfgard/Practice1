using UnityEngine;

namespace MapSystem.TileLayer
{
    internal abstract class TileGenerateParameterNode : ScriptableObject
    {
        [SerializeField] protected TileGenerateParameterNode _prevGenerateParam;
        [SerializeField] protected int _maxWeight;

        [Header ("Balance")]
        [SerializeField] protected int _cost;

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