using MohawkGame2D;
using System.Numerics;

public class Ball
    {
    public Vector2 position;
    public Vector2 size;
    public Vector2 velocity = new Vector2(4, 2);
    public Color color = Color.White;

    public Ball(Vector2 pos, Vector2 sz)
    {
        position = pos;
        size = sz;
    }

    public void Move()
    {
        position += velocity;
    }

    public void ChangeColor()
    {
        color = Random.Color();
    }

    public void Render()
    {
        Draw.FillColor = color;
        Draw.LineColor = Color.Black;
        Draw.Rectangle(position, size);
    }
}
    

