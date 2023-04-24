using UnityEngine;
using System;

namespace MapSystem.Pathfinding
{
    internal class PathNodeData : IEquatable<PathNodeData>
    {
        public Vector3Int Position { get; private set; }
        public float RestMovement { get; private set; }

        public PathNodeData(Vector3Int position, float restMovement)
        {
            Position = position;
            RestMovement = restMovement;
        }

        public bool Equals(PathNodeData other)
        {
            if (Position.Equals(other.Position))
            {
                if (RestMovement < other.RestMovement)
                    RestMovement = other.RestMovement;
                return true;
            }

            return false;
        }
    }
}