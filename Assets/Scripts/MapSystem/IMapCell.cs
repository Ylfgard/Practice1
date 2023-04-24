namespace MapSystem
{
    public interface IMapCell
    {
        public int Cost { get; }
        public IAvailableTransitions Transitions { get; }
        public ContainedType Type { get; }
    }
}