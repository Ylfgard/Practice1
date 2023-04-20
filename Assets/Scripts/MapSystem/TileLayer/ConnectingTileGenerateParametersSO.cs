using UnityEngine;

namespace MapSystem.TileLayer
{
    [CreateAssetMenu(fileName = "NewConnectingTileGenerateParameters", menuName = "Scriptable Objects/Connecting Tile/Generate Parameters")]
    internal class ConnectingTileGenerateParametersSO : TileGenerateParameterNode
    {
        [SerializeField] private TileChoiceRulesSO[] _tilesChoiceRules;

        public TileChoiceRulesSO[] TilesChoiceRiles => _tilesChoiceRules;
    }
}