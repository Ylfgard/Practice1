using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace MapSystem.TileLayer
{
    [Serializable]
    [CreateAssetMenu (fileName = "NewTileChoiceRiles",
        menuName = "Scriptable Objects/Connecting Tile/Choice Rules")]
    internal class TileChoiceRulesSO : ScriptableObject
    {
        [SerializeField] private Tile _tile;
        [SerializeField] private ChoiceRules _choiceRules;

        public Tile Tile => _tile;
        public ChoiceRules ChoiceRules => _choiceRules;
    }
}