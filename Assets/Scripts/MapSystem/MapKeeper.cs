using UnityEngine;
using System.Collections.Generic;

namespace MapSystem
{
    public class MapKeeper : MonoBehaviour
    {
        [SerializeField] private MapGenerator _mapGenerator;
        [SerializeField] private MapConnectionsGenerator _connectionsGenerator;
        private MapCell[,] _mapCells;

        private int _width, _hight;

        private void Awake()
        {
            _mapGenerator.GenerationStarted += SetMapSize;
            _mapGenerator.SendCost += SetCell;
            _connectionsGenerator.SendCost += SetCell;
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

            _mapCells[x, y] = new MapCell(cost, topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);
        }

        private void OnDestroy()
        {
            _mapGenerator.GenerationStarted -= SetMapSize;
            _mapGenerator.SendCost -= SetCell;
            _connectionsGenerator.SendCost -= SetCell;
        }
    }

    public class MapCell
    {
        private int _cost;
        private bool _topLeft, _top, _topRight, _left, _right, _bottomLeft, _bottom, _bottomRight, _available;

        public int Cost => _cost;
        public bool TopLeft => _topLeft;
        public bool Top => _top;
        public bool TopRight => _topRight;
        public bool Left => _left;
        public bool Right => _right;
        public bool BottomLeft => _bottomLeft;
        public bool Bottom => _bottom;
        public bool BottomRight => _bottomRight;
        public bool Available => _available;

        public MapCell(int cost, bool topLeft, bool top, bool topRight, bool left, bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            _cost = cost;
            _topLeft = topLeft;
            _top = top;
            _topRight = topRight;
            _left = left;
            _right = right;
            _bottomLeft = bottomLeft;
            _bottom = bottom;
            _bottomRight = bottomRight;
            _available = true;
        }
    }
}