using UnityEngine;
using MapSystem.TileLayer;
using UnityEngine.Tilemaps;
using Zenject;

namespace MapSystem
{
    public class MapKeeper : MonoBehaviour
    {
        [SerializeField] private TileGenerator _mapGenerator;
        [SerializeField] private TileConnectionsGenerator _connectionsGenerator;

        [Inject] private Tilemap _tilemap;
        
        private MapCell[,] _mapCells;
        private int _width, _hight;

        public MapCell[,] MapCells => _mapCells;

        private void Awake()
        {
            _mapGenerator.GenerationStarted += SetMapSize;
            _mapGenerator.SendCost += SetCell;
            _connectionsGenerator.SendCost += SetCell;
        }

        public Vector3Int GetCellPosition(Vector3 worldPosition)
        {
            return _tilemap.WorldToCell(worldPosition);
        }

        public Vector3 GetCellCenter(Vector3Int cellPosition)
        {
            return _tilemap.GetCellCenterWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
        }

        private void SetMapSize(int width, int hight)
        {
            _width = width;
            _hight = hight;
            _mapCells = new MapCell[width, hight];
        }

        private void SetCell(int x, int y, int cost)
        {
            bool topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight;

            left = x > 0;
            right = x < _width - 1;
            bottom = y > 0;
            top = y < _hight - 1;
            topLeft = left && top;
            topRight = right && top;
            bottomLeft = left && bottom;
            bottomRight = right && bottom;

            AvailableTransitions availableTransitions = 
                new AvailableTransitions(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);
            _mapCells[x, y] = new MapCell(cost, availableTransitions);
        }

        private void OnDestroy()
        {
            _mapGenerator.GenerationStarted -= SetMapSize;
            _mapGenerator.SendCost -= SetCell;
            _connectionsGenerator.SendCost -= SetCell;
        }
    }
}