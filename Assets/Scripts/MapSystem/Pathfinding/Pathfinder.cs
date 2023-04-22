using UnityEngine;
using System.Collections.Generic;
using System;

namespace MapSystem.Pathfinding
{
    public class Pathfinder : MonoBehaviour
    {
        private static readonly float DiagonalStepMultiplier = (float)Math.Sqrt(2);
        private static readonly int StepCost = 10;

        [SerializeField] private MapKeeper _mapKeeper;
        [SerializeField] private GameObject _marker;

        //FOR DEBUG
        [Header("Test")]
        [SerializeField] private Transform _origin;
        [SerializeField] private Transform _target;
        [SerializeField] private float _movePoints;

        private Vector3Int _originPos;
        private List<Vector3Int> _path;
        //

        private MapCell[,] _mapCells => _mapKeeper.MapCells;
        private Dictionary<Vector3Int, PathNode> _availableCells;

        private void Awake()
        {
            _availableCells = new Dictionary<Vector3Int, PathNode>();
        }

        [ContextMenu ("CalculatePath")]
        private void TestCalculation()
        {
            Vector3Int origin = _mapKeeper.GetCellPosition(_origin.position);
            GetAvailableCells(origin, _movePoints);
        }

        public List<Vector3Int> GetPathTo(Vector3Int origin, Vector3Int target)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            BuildPath(origin, target, ref path);
            return path;
        }

        public Dictionary<Vector3Int, PathNode> GetAvailableCells(Vector3Int origin, float movementPoints)
        {
            _availableCells.Clear();
            CalculatePath(origin, movementPoints);
            _originPos = origin;
            return _availableCells;
        }

        private void BuildPath(Vector3Int origin, Vector3Int current, ref List<Vector3Int> path)
        {
            if (_availableCells.TryGetValue(current, out var node))
            {
                if (node.Previos != origin)
                {
                    path.Add(node.Previos);
                    if (current == node.Previos)
                    {
                        Debug.LogError(origin + ": " + current + "-" + node.Previos);
                        return;
                    }
                        
                    BuildPath(origin, node.Previos, ref path);
                }
            }
        }

        private void CalculatePath(Vector3Int current, float restMovement)
        {
            MapCell cell = _mapCells[current.x, current.y];

            int i = 0;
            for (int y = current.y + 1; y >= current.y - 1; y--)
            {
                for (int x = current.x - 1; x <= current.x + 1; x++)
                {
                    Vector3Int checkPos = new Vector3Int(x, y, current.z);
                    bool notSameX = x != current.x;
                    bool notSameY = y != current.y;

                    if (checkPos != current && cell.Transitions.All[i] && _mapCells[x, y].Available)
                    {
                        float cost = StepCost;
                        if (notSameX && notSameY) cost *= DiagonalStepMultiplier;
                        cost += _mapCells[x, y].Cost;

                        float checkRestMovement = restMovement - cost;
                        if (checkRestMovement >= 0)
                        {
                            if (_availableCells.TryGetValue(checkPos, out PathNode node))
                            {
                                if (node.RestMovement < checkRestMovement)
                                {
                                    node.SetData(current, checkRestMovement);
                                    CalculatePath(checkPos, checkRestMovement);
                                }
                            }
                            else
                            {
                                node = new PathNode(current, checkRestMovement);
                                _availableCells.Add(checkPos, node);
                                CalculatePath(checkPos, checkRestMovement);
                            }
                        }
                    }

                    if (notSameX || notSameY) i++;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_availableCells == null) return;
            var originPos = _mapKeeper.GetCellPosition(_origin.position);

            if (_originPos != originPos) return;
            var pos = _mapKeeper.GetCellPosition(_target.position);

            _path = GetPathTo(originPos, pos);
            if (_path.Count == 0) return;

            
            Gizmos.color = Color.red;
            var prevNode = _mapKeeper.GetCellCenter(pos);
            foreach (var node in _path)
            {
                Gizmos.DrawLine(_mapKeeper.GetCellCenter(node), prevNode);
                prevNode = _mapKeeper.GetCellCenter(node);
            }
            Gizmos.DrawLine(prevNode, _mapKeeper.GetCellCenter(_originPos));

            Gizmos.color = Color.green;
            foreach (var point in _availableCells.Keys)
                if (_path.Contains(point) == false)
                    Gizmos.DrawSphere(_mapKeeper.GetCellCenter(point), 0.05f);
        }
#endif
    }
}