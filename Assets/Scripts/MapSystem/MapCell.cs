namespace MapSystem
{
    public class MapCell : IMapCell
    {
        public int MyCost;
        public AvailableTransitions MyAvailableTransitions;
        public ContainedType MyType;

        public int Cost => MyCost;
        public IAvailableTransitions Transitions => MyAvailableTransitions;
        public ContainedType Type => MyType;

        public MapCell(int cost, AvailableTransitions availableTransitions)
        {
            MyCost = cost;
            MyAvailableTransitions = availableTransitions;
            MyType = ContainedType.Free;
        }
    }

    public class AvailableTransitions : IAvailableTransitions
    {
        public ContainedType[] MyTransitions = new ContainedType[IAvailableTransitions.TransitionsCount];

        public ContainedType[] All => MyTransitions;
        public ContainedType TopLeft => MyTransitions[0];
        public ContainedType Top => MyTransitions[1];
        public ContainedType TopRight => MyTransitions[2];
        public ContainedType Left => MyTransitions[3];
        public ContainedType Right => MyTransitions[4];
        public ContainedType BottomLeft => MyTransitions[5];
        public ContainedType Bottom => MyTransitions[6];
        public ContainedType BottomRight => MyTransitions[7];
        
        public AvailableTransitions(ContainedType topLeft, ContainedType top, ContainedType topRight, ContainedType left, 
            ContainedType right, ContainedType bottomLeft, ContainedType bottom, ContainedType bottomRight)
        {
            MyTransitions[0] = topLeft;
            MyTransitions[1] = top;
            MyTransitions[2] = topRight;
            MyTransitions[3] = left;
            MyTransitions[4] = right;
            MyTransitions[5] = bottomLeft;
            MyTransitions[6] = bottom;
            MyTransitions[7] = bottomRight;
        }
    }

    public enum ContainedType
    {
        Free,
        MapBorder,
        HighObstacle,
        LowObstacle
    }
}