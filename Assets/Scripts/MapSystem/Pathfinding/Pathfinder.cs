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
        [SerializeField] private bool _showPath;
        [SerializeField] private Transform _unit;
        [SerializeField] private Transform _target;
        [SerializeField] private float _movePoints;

        private Vector3Int _from;
        private List<Vector3Int> _path;
        private Dictionary<Vector3Int, PathNode> _availableCells;
        //

        private IMapCell[,] _mapCells => _mapKeeper.MapCells;
        
        [ContextMenu ("CalculatePath")]
        private void TestCalculation()
        {
            Vector3Int origin = _mapKeeper.GetCellPosition(_unit.position);
            GetAvailableCells(origin, _movePoints);
        }

        public List<Vector3Int> GetPathTo(Vector3Int form, Vector3Int to)
        {
            List<Vector3Int> path = new List<Vector3Int>();
            if (_availableCells.ContainsKey(to))
            {
                path.Add(to);
                BuildPath(form, to, ref path);
            }
            return path;
        }

        public Dictionary<Vector3Int, PathNode> GetAvailableCells(Vector3Int from, float movementPoints)
        {
            _from = from;
            _availableCells = new Dictionary<Vector3Int, PathNode>();

            List<PathNodeData> startNode = new List<PathNodeData>(1);
            startNode.Add(new PathNodeData(from, movementPoints));

            CalculatePath(startNode);
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

        private void CalculatePath(List<PathNodeData> currentNodes)
        {
            List<PathNodeData> newPathNodesData = new List<PathNodeData>();
            foreach (var currentNode in currentNodes)
            {
                var currentPos = currentNode.Position;
                IMapCell cell = _mapCells[currentPos.x, currentPos.y];

                int i = 0;
                for (int y = currentPos.y + 1; y >= currentPos.y - 1; y--)
                {
                    for (int x = currentPos.x - 1; x <= currentPos.x + 1; x++)
                    {
                        Vector3Int checkPos = new Vector3Int(x, y, currentPos.z);
                        bool notSameX = x != currentPos.x;
                        bool notSameY = y != currentPos.y;

                        if (checkPos != currentPos && cell.Transitions.All[i] == ContainedType.Free &&
                            _mapCells[x, y].Type == ContainedType.Free)
                        {
                            float cost = StepCost;
                            if (notSameX && notSameY) cost *= DiagonalStepMultiplier;
                            cost += _mapCells[x, y].Cost;

                            float checkRestMovement = currentNode.RestMovement - cost;
                            if (checkRestMovement >= 0)
                            {
                                if (_availableCells.TryGetValue(checkPos, out PathNode node))
                                {
                                    if (node.RestMovement < checkRestMovement)
                                    {
                                        node.SetData(currentPos, checkRestMovement);
                                        
                                        var connectedTile = new PathNodeData(checkPos, checkRestMovement);
                                        if (newPathNodesData.Contains(connectedTile) == false)
                                            newPathNodesData.Add(connectedTile);
                                    }
                                }
                                else
                                {
                                    node = new PathNode(currentPos, checkRestMovement);
                                    _availableCells.Add(checkPos, node);
                                    newPathNodesData.Add(new PathNodeData(checkPos, checkRestMovement));
                                }
                            }
                        }

                        if (notSameX || notSameY) i++;
                    }
                }
            }
            
            if (newPathNodesData.Count > 0)
                CalculatePath(newPathNodesData);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_showPath == false) return;
            if (_availableCells == null) return;
            var unitPos = _mapKeeper.GetCellPosition(_unit.position);

            if (_from != unitPos) return;
            var pos = _mapKeeper.GetCellPosition(_target.position);

            _path = GetPathTo(unitPos, pos);
            if (_path.Count == 0) return;
            Vector3 prevNode = _target.position;
            Gizmos.color = Color.red;
            foreach (var node in _path)
            {
                Gizmos.DrawLine(_mapKeeper.GetCellCenter(node), prevNode);
                prevNode = _mapKeeper.GetCellCenter(node);
            }
            Gizmos.DrawLine(_mapKeeper.GetCellCenter(unitPos), prevNode);

            Gizmos.color = Color.green;
            foreach (var point in _availableCells.Keys)
                if (_path.Contains(point) == false)
                    Gizmos.DrawSphere(_mapKeeper.GetCellCenter(point), 0.05f);
        }
#endif
    }
}