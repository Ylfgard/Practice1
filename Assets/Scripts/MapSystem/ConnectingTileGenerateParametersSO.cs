using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "NewConnectingTileGenerateParameters", menuName = "Scriptable Objects/Connecting Tile Generate Parameters")]
    internal class ConnectingTileGenerateParametersSO : TileGenerateParameterNode
    {
        [SerializeField] private TileChoiceRules[] _topLeftCorner, _topCenter, _topRightCorner, _middleLeft, 
            _middleRight, _bottomLeftCorner, _bottomCenter, _bottomRightCorner;
        [SerializeField] private TileChoiceRules _topLeftInnerCorner, _topRightInnerCorner, _bottomLeftInnerCorner, _bottomRightInnerCorner;

        private Dictionary<int, Tile> _connectingTiles;

        public void InitializeConnectingTiles()
        {
            _connectingTiles = new Dictionary<int, Tile>();

            TileChoiceRules[] rules = _topLeftCorner;
            for (int i = 1; i <= 8; i++)
            {
                foreach (var tile in rules)
                {
                    int key = GetKey(tile.TopLeft, tile.Top, tile.TopRight, tile.Left, tile.Right, tile.BottomLeft, tile.Bottom, tile.BottomRight);

                    if (_connectingTiles.ContainsKey(key)) Debug.LogError("Wrong rules! " + tile.Tile.name);
                    else _connectingTiles.Add(key, tile.Tile);
                }

                switch(i)
                {
                    case 1:
                        rules = _topCenter;
                        break;
                    case 2:
                        rules = _topRightCorner;
                        break;
                    case 3:
                        rules = _middleLeft;
                        break;
                    case 4:
                        rules = _middleRight;
                        break;
                    case 5:
                        rules = _bottomLeftCorner;
                        break;
                    case 6:
                        rules = _bottomCenter;
                        break;
                    case 7:
                        rules = _bottomRightCorner;
                        break;
                }
            }

            var curRules = _topLeftInnerCorner;
            for (int i = 1; i <= 4; i++)
            {
                int key = GetKey(curRules.TopLeft, curRules.Top, curRules.TopRight, curRules.Left,
                curRules.Right, curRules.BottomLeft, curRules.Bottom, curRules.BottomRight);

                if (_connectingTiles.ContainsKey(key)) Debug.LogError("Wrong rules! " + curRules.Tile.name);
                else _connectingTiles.Add(key, curRules.Tile);

                switch (i)
                {
                    case 1:
                        curRules = _topRightInnerCorner;
                        break;
                    case 2:
                        curRules = _bottomLeftInnerCorner;
                        break;
                    case 3:
                        curRules = _bottomRightInnerCorner;
                        break;
                }
            }
        }

        public Tile GetTile(bool topLeft, bool top, bool topRight, bool left, bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            int key = GetKey(topLeft, top, topRight, left, right, bottomLeft, bottom, bottomRight);

            if (_connectingTiles.TryGetValue(key, out Tile result))
                return result;
            else
                return _bottomRightInnerCorner.Tile;
        }

        private int GetKey(bool topLeft, bool top, bool topRight, bool left, bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            int key = Convert.ToByte(topLeft) * 1 + Convert.ToByte(top) * 2
                    + Convert.ToByte(topRight) * 4 + Convert.ToByte(left) * 8
                    + Convert.ToByte(right) * 16 + Convert.ToByte(bottomLeft) * 32
                    + Convert.ToByte(bottom) * 64 + Convert.ToByte(bottomRight) * 128;
            return key;
        }
    }

    [Serializable]
    public class TileChoiceRules
    {
        [SerializeField] private bool _topLeft, _top, _topRight;
        [SerializeField] private bool _left, _right;
        [SerializeField] private bool _bottomLeft, _bottom, _bottomRight;
        [SerializeField] private Tile _tile; 

        public bool TopLeft => _topLeft;
        public bool Top => _top;
        public bool TopRight => _topRight;
        public bool Left => _left;
        public bool Right => _right;
        public bool BottomLeft => _bottomLeft;
        public bool Bottom => _bottom;
        public bool BottomRight => _bottomRight;
        public Tile Tile => _tile;
    }
}