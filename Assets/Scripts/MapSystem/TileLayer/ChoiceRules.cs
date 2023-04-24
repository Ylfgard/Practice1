using UnityEngine;
using System;
using System.Collections.Generic;

namespace MapSystem.TileLayer
{
    [Serializable]
    public struct ChoiceRules
    {
        private const int RulesCount = 8;

        [SerializeField] private TileRule _topLeft, _top, _topRight;
        [SerializeField] private TileRule _left, _right;
        [SerializeField] private TileRule _bottomLeft, _bottom, _bottomRight;

        internal TileRule TopLeft => _topLeft;
        internal TileRule Top => _top;
        internal TileRule TopRight => _topRight;
        internal TileRule Left => _left;
        internal TileRule Right => _right;
        internal TileRule BottomLeft => _bottomLeft;
        internal TileRule Bottom => _bottom;
        internal TileRule BottomRight => _bottomRight;

        internal ChoiceRules(bool topLeft, bool top, bool topRight, bool left,
            bool right, bool bottomLeft, bool bottom, bool bottomRight)
        {
            if (topLeft) _topLeft = TileRule.Same;
            else _topLeft = TileRule.Different;
            if (top) _top = TileRule.Same;
            else _top = TileRule.Different;
            if (topRight) _topRight = TileRule.Same;
            else _topRight = TileRule.Different;

            if (left) _left = TileRule.Same;
            else _left = TileRule.Different;
            if (right) _right = TileRule.Same;
            else _right = TileRule.Different;

            if (bottomLeft) _bottomLeft = TileRule.Same;
            else _bottomLeft = TileRule.Different;
            if (bottom) _bottom = TileRule.Same;
            else _bottom = TileRule.Different;
            if (bottomRight) _bottomRight = TileRule.Same;
            else _bottomRight = TileRule.Different;
        }

        internal int GetKey()
        {
            int key = Convert.ToByte(_topLeft) * 1 + Convert.ToByte(_top) * 2
                    + Convert.ToByte(_topRight) * 4 + Convert.ToByte(_left) * 8
                    + Convert.ToByte(_right) * 16 + Convert.ToByte(_bottomLeft) * 32
                    + Convert.ToByte(_bottom) * 64 + Convert.ToByte(_bottomRight) * 128;
            return key;
        }

        private int GetKey(TileRule[] tileRules)
        {
            if (tileRules.Length < RulesCount)
            {
                Debug.LogError("Wrong tile rules count! " + tileRules.Length);
                return 0;
            }
            int key = Convert.ToByte(tileRules[0]) * 1 + Convert.ToByte(tileRules[1]) * 2
                    + Convert.ToByte(tileRules[2]) * 4 + Convert.ToByte(tileRules[3]) * 8
                    + Convert.ToByte(tileRules[4]) * 16 + Convert.ToByte(tileRules[5]) * 32
                    + Convert.ToByte(tileRules[6]) * 64 + Convert.ToByte(tileRules[7]) * 128;
            return key;
        }

        internal List<int> GetKeys()
        {
            TileRule[] tileRules = GetKeyRulesArray();
            List<int> keys = new List<int>();
            GetKeyRecurs(tileRules, keys);
            return keys;
        }

        internal TileRule[] GetKeyRulesArray()
        {
            TileRule[] tileRules = new TileRule[RulesCount];
            for (int i = 0; i < RulesCount; i++)
            {
                switch (i)
                {
                    case 0:
                        tileRules[i] = _topLeft;
                        break;
                    case 1:
                        tileRules[i] = _top;
                        break;
                    case 2:
                        tileRules[i] = _topRight;
                        break;

                    case 3:
                        tileRules[i] = _left;
                        break;
                    case 4:
                        tileRules[i] = _right;
                        break;

                    case 5:
                        tileRules[i] = _bottomLeft;
                        break;
                    case 6:
                        tileRules[i] = _bottom;
                        break;
                    case 7:
                        tileRules[i] = _bottomRight;
                        break;
                }
            }

            return tileRules;
        }

        private void GetKeyRecurs(TileRule[] tileRules, List<int> keys)
        {
            int i;
            
            for (i = 0; i < RulesCount; i++)
            {
                if (tileRules[i] == TileRule.Both)
                {
                    tileRules[i] = TileRule.Same;
                    GetKeyRecurs(tileRules, keys);
                    tileRules[i] = TileRule.Different;
                    GetKeyRecurs(tileRules, keys);

                    tileRules[i] = TileRule.Both;
                    break;
                }
            }

            if (i == RulesCount)
                keys.Add(GetKey(tileRules));
        }
    }

    public enum TileRule
    {
        Different = 0,
        Same = 1,
        Both = 2
    }
}