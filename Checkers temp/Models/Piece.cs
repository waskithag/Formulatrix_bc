public class Piece(bool isKing, PieceColor color) : IPiece
{
    public readonly bool IsKing = isKing;
    public readonly PieceColor Color = color;
}