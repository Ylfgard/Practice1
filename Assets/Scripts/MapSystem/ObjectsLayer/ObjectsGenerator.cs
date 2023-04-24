using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace MapSystem.ObjectsLayer
{
    internal class ObjectsGenerator : MonoBehaviour
    {
        public Action<int, int, ContainedType> ObjectSpawned;

        [SerializeField] private ObjectsKeeper _objectsKeeper;

        [Inject] private TilemapKeeper _tilemapKeeper;

        private Dictionary<int, List<Vector3Int>> _freeTiles;
        private Tilemap _tilemap => _tilemapKeeper.Objects;

        public void GenerateObjects(Dictionary<int, List<Vector3Int>> freeTiles)
        {
            _tilemap.ClearAllTiles();
            _freeTiles = freeTiles;
            foreach (var obj in _objectsKeeper.Objects)
            {
                int availableTilesCount = 0;
                foreach (var weight in obj.PlacedOnTilesWeights)
                {
                    if (freeTiles.TryGetValue(weight, out List<Vector3Int> tiles))
                        availableTilesCount += tiles.Count;
                }
                int needToSpawnCount = Mathf.RoundToInt(availableTilesCount * obj.FillingPercentage);
                SpawnObjects(obj, needToSpawnCount);
            }
        }

        private void SpawnObjects(IObjectGenerateParameter obj, int needToSpawnCount)
        {
            int curWeightIndex = 0;
            while (needToSpawnCount > 0)
            {
                int weight = obj.PlacedOnTilesWeights[curWeightIndex];
                if (_freeTiles.TryGetValue(weight, out List<Vector3Int> positions))
                {
                    if (positions.Count > 0)
                    {
                        var pos = positions[Random.Range(0, positions.Count)];
                        var tile = obj.Tile;
                        _tilemap.SetTile(pos, tile);
                        ObjectSpawned?.Invoke(pos.x, pos.y, obj.Type);
                        needToSpawnCount--;
                    }
                }

                curWeightIndex++;
                if (curWeightIndex >= obj.PlacedOnTilesWeights.Length)
                    curWeightIndex = 0;
            }
        }
    }
}