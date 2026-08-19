public struct Move(Position from, Position to, List<Position> capturedPieces, List<Position> path)
{
    public Position From { get; } = from;
    public Position To { get; } = to;
    public List<Position> CapturedPieces { get; } = capturedPieces;
    public List<Position> Path { get; } = path;
}