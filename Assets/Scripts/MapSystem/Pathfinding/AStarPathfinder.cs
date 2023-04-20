using UnityEngine;
using System.Collections.Generic;
using System;

namespace MapSystem.Pathfinding
{
    public class AStarPathfinder : MonoBehaviour
    {
        private const float CrossStepMultiplier = 1.4f;

        [SerializeField] private MapKeeper _mapKeeper;

        private MapCell[,] _mapCells;
        private List<Vector2Int> _path;
        private HashSet<Vector2Int> _closedList;
        private HashSet<PathNode> _pathNodes;
        private Dictionary<Vector2Int, Vector2Int> _transitions;

        private void Awake()
        {
            _mapCells = _mapKeeper.MapCells;
            _path = new List<Vector2Int>();
            _closedList = new HashSet<Vector2Int>();
            _pathNodes = new HashSet<PathNode>();
            _transitions = new Dictionary<Vector2Int, Vector2Int>();
        }

        public List<Vector2Int> GetPath(Vector2Int origin, Vector2Int target)
        {
            _path.Clear();
            _pathNodes.Clear();
            _closedList.Clear();
            _transitions.Clear();

            CalculatePath(origin, target);

            return _path;
        }

        private void CalculatePath(Vector2Int current, Vector2Int target)
        {
            MapCell cell = _mapCells[current.x, current.y];
            _closedList.Add(current);

            int i = 0;
            for (int y = current.y + 1; y > current.y - 1; y--)
            {
                for (int x = current.x - 1; x < current.x + 1; x++)
                {
                    Vector2Int curPos = new Vector2Int(x, y);
                    MapCell curCell = _mapCells[x, y];

                    if (_closedList.Contains(curPos) == false &&
                        cell.Transitions.All[i] && curCell.Available)
                    {
                        float cost = curCell.Cost;
                        if (i % 2 != 0) cost *= CrossStepMultiplier;

                        int dist = Math.Abs(x - target.x) + Math.Abs(y - target.y);

                        PathNode node = new PathNode(curPos, cost, dist);
                        _pathNodes.Add(node);
                    }
                    
                    if (x != current.x || y != current.x) i++;
                }
            }


        }
    }

    internal struct PathNode
    {
        public PathNode(Vector2Int position, float cost, float dist)
        {
            Position = position;
            Cost = cost;
            TotalCost = cost + dist;
        }

        public Vector2Int Position { get; }
        public float Cost { get; }
        public float TotalCost { get; }
    }
}