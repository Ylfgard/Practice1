using UnityEngine;
using System.Collections.Generic;

namespace MapSystem.ObjectsLayer
{
    internal class ObjectsKeeper : MonoBehaviour
    {
        [SerializeField] private ObjectGenerateParametersSO[] _parameters;

        public List<IObjectGenerateParameter> Objects { get; private set; }

        private void Awake()
        {
            Objects = new List<IObjectGenerateParameter>();
            GenerateNewObjectsParameters();
        }

        [ContextMenu("Generate New Objects Parameters")]
        private void GenerateNewObjectsParameters()
        {
            Objects.Clear();
            Dictionary<int, List<ObjectGenerateParameter>> weightToParameter = 
                new Dictionary<int, List<ObjectGenerateParameter>>();
            foreach (var parameter in _parameters)
            {
                var param = new ObjectGenerateParameter(parameter);
                Objects.Add(param);
                foreach(var weight in parameter.PlacedOnTilesWeights)
                {
                    if (weightToParameter.TryGetValue(weight, out var parameters))
                    {
                        parameters.Add(param);
                    }
                    else
                    {
                        List<ObjectGenerateParameter> newParameters = new List<ObjectGenerateParameter>();
                        newParameters.Add(param);
                        weightToParameter.Add(weight, newParameters);
                    }
                }
            }

            foreach (var weight in weightToParameter.Keys)
            {
                weightToParameter.TryGetValue(weight, out var parameters);
                float totalFillPercentage = 0;
                foreach (var param in parameters)
                    totalFillPercentage += param.FillingPercentage;

                if (totalFillPercentage > 1)
                {
                    foreach (var param in parameters)
                    {
                        Debug.LogError("Wrong filling percentage in " + param.Tile.name);
                        param.SetFillingPercentage(param.FillingPercentage / totalFillPercentage);
                    }
                }
            }
        }
    }
}