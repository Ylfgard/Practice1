using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    [CreateAssetMenu(fileName = "NewConnectingTileGenerateParameters", menuName = "Scriptable Objects/Connecting Tile Generate Parameters")]
    public class ConnectingTileGenerateParameters : ScriptableObject
    {
        [SerializeField] private int _value;
        [SerializeField] private Tile _topLeftCorner, _topLeftInnerCorner, _topCenter, _topRightInnerCorner, _topRightCorner;
        [SerializeField] private Tile _middleLeft, _middleRight;
        [SerializeField] private Tile _bottomLeftCorner, _bottomLeftInnerCorner, _bottomCenter, _bottomRightInnerCorner, _bottomRightCorner;

        public int Value => _value;

        public Tile GetTile(bool topLeft, bool top, bool topRight, bool left, bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            if (top)
            {
                if (bottom)
                {
                    if (left)
                    {
                        if (right)
                        {
                            if (topLeft == false)
                                return _topLeftInnerCorner;

                            if (topRight == false)
                                return _topRightInnerCorner;

                            if (bottomLeft == false)
                                return _bottomLeftInnerCorner;
                            else
                                return _bottomRightInnerCorner;
                        }
                        else
                        {
                            return _middleRight;
                        }
                    }
                    else
                    {
                        return _middleLeft;
                    }
                }
                else
                {
                    if (left)
                    {
                        if (right)
                            return _bottomCenter;
                        else
                            return _bottomRightCorner;
                    }
                    else
                    {
                        return _bottomLeftCorner;
                    }
                }
            }
            else
            {
                if (left)
                {
                    if (right)
                        return _topCenter;
                    else
                        return _topRightCorner;
                }
                else
                {
                    return _topLeftCorner;
                }
            }
        }
    }
}