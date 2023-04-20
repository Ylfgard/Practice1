namespace MapSystem
{
    public class MapCell
    {
        private int _cost;
        private AvailableTransitions _availableTransitions;
        private bool _available;

        public int Cost => _cost;
        public AvailableTransitions Transitions => _availableTransitions;
        public bool Available => _available;

        public MapCell(int cost, AvailableTransitions availableTransitions)
        {
            _cost = cost;
            _availableTransitions = availableTransitions;
            _available = true;
        }
    }

    public class AvailableTransitions
    {
        public const int TransitionsCount = 8;

        private bool[] _transitions = new bool[TransitionsCount];

        public bool[] All => _transitions;
        public bool TopLeft => _transitions[0];
        public bool Top => _transitions[1];
        public bool TopRight => _transitions[2];
        public bool Left => _transitions[3];
        public bool Right => _transitions[4];
        public bool BottomLeft => _transitions[5];
        public bool Bottom => _transitions[6];
        public bool BottomRight => _transitions[7];
        
        public AvailableTransitions(bool topLeft, bool top, bool topRight, bool left, bool right,
            bool bottomLeft, bool bottom, bool bottomRight)
        {
            _transitions[0] = topLeft;
            _transitions[1] = top;
            _transitions[2] = topRight;
            _transitions[3] = left;
            _transitions[4] = right;
            _transitions[5] = bottomLeft;
            _transitions[6] = bottom;
            _transitions[7] = bottomRight;
        }
    }
}