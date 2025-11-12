using MohawkGame2D;
using System.Numerics;

public class Paddle
    {
    public Vector2 position;
    public Vector2 size;
    public Color color;

    public Paddle(Vector2 pos, Vector2 sz, Color col)
    {
        position = pos;
        size = sz;
        color = col;
    }

    public void Render()
    {
        Draw.FillColor = color;
        Draw.LineColor = Color.Black;
        Draw.Rectangle(position, size);
    }
}
