using UnityEngine;

namespace MapSystem.Pathfinding
{
    public class PathNode
    {
        public PathNode(Vector3Int previos, float restMovement)
        {
            Previos = previos;
            RestMovement = restMovement;
        }

        internal void SetData(Vector3Int previos, float restMovement)
        {
            Previos = previos;
            RestMovement = restMovement;
        }        

        public Vector3Int Previos { get; private set; }
        public float RestMovement { get; private set; }
    }
}