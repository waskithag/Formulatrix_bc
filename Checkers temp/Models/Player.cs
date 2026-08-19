public class Player(string name, PieceColor color) : IPlayer 
{
    public string Name { get; set; } = name;
    public PieceColor Color { get; set; } = color;
}