using UnityEngine;
using Zenject;
using MapSystem.TileLayer;
using MapSystem.ObjectsLayer;

namespace MapSystem
{
    public class MapKeeper : MonoBehaviour
    {
        [SerializeField] private TilesGenerator _tilesGenerator;
        [SerializeField] private ObjectsGenerator _objectsGenerator;

        [Inject] private TilemapKeeper _tilemapKeeper;
        
        private MapCell[,] _mapCells;
        private int _width, _hight;

        public IMapCell[,] MapCells => _mapCells;

        private void Awake()
        {
            _tilesGenerator.GenerationStarted += SetMapSize;
            _tilesGenerator.SendCost += SetCell;
            _tilesGenerator.TileLayerGenerated += _objectsGenerator.GenerateObjects;
            _objectsGenerator.ObjectSpawned += SetObject;
        }

        public Vector3Int GetCellPosition(Vector3 worldPosition)
        {
            return _tilemapKeeper.Tiles.WorldToCell(worldPosition);
        }

        public Vector3 GetCellCenter(Vector3Int cellPosition)
        {
            return _tilemapKeeper.Tiles.GetCellCenterWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
        }

        private void SetMapSize(int width, int hight)
        {
            _width = width;
            _hight = hight;
            _mapCells = new MapCell[width, hight];
        }

        private void SetCell(int x, int y, int cost)
        {
            bool leftTrans, rightTrans, topTrans, bottomTrans;
            ContainedType topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight;

            leftTrans = x > 0;
            rightTrans = x < _width - 1;
            bottomTrans = y > 0;
            topTrans = y < _hight - 1;

            left = leftTrans ? ContainedType.Free : ContainedType.MapBorder;
            right = rightTrans ? ContainedType.Free : ContainedType.MapBorder;
            bottom = bottomTrans ? ContainedType.Free : ContainedType.MapBorder;
            top = topTrans ? ContainedType.Free : ContainedType.MapBorder;
            topLeft = leftTrans && topTrans ? ContainedType.Free : ContainedType.MapBorder;
            topRight = rightTrans && topTrans ? ContainedType.Free : ContainedType.MapBorder;
            bottomLeft = leftTrans && bottomTrans ? ContainedType.Free : ContainedType.MapBorder;
            bottomRight = rightTrans && bottomTrans ? ContainedType.Free : ContainedType.MapBorder;

            AvailableTransitions availableTransitions = 
                new AvailableTransitions(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);
            _mapCells[x, y] = new MapCell(cost, availableTransitions);
        }

        private void SetObject(int x, int y, ContainedType type)
        {
            _mapCells[x, y].MyType = type;
        }

        private void OnDestroy()
        {
            _tilesGenerator.GenerationStarted -= SetMapSize;
            _tilesGenerator.SendCost -= SetCell;
            _tilesGenerator.TileLayerGenerated -= _objectsGenerator.GenerateObjects;
            _objectsGenerator.ObjectSpawned -= SetObject;
        }
    }
}