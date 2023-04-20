using UnityEngine;

namespace MapSystem.TileLayer
{
    public class ConnectingTileData : TileData
    {
        public bool FirstRun;
        
        protected ChoiceRules _choiceRules;

        public ChoiceRules ChoiceRules => _choiceRules;

        public ConnectingTileData(int weight, ChoiceRules choiceRules, bool firstRun) :
            base(weight)
        {
            Weight = weight;
            _choiceRules = choiceRules;
            FirstRun = firstRun;
        }
    }
}