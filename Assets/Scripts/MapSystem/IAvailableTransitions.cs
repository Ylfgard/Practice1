namespace MapSystem
{
    public interface IAvailableTransitions
    {
        public const int TransitionsCount = 8;

        public ContainedType[] All { get; }
        public ContainedType TopLeft { get; }
        public ContainedType Top { get; }
        public ContainedType TopRight { get; }
        public ContainedType Left { get; }
        public ContainedType Right { get; }
        public ContainedType BottomLeft { get; }
        public ContainedType Bottom { get; }
        public ContainedType BottomRight { get; }
    }
}