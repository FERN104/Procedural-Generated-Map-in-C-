using System.Diagnostics.Contracts;
using System.Numerics;
using Raylib_cs;

namespace Cs_raylib_test.MapLogic;

public class Wall : MapObject
{
    private Vector2 position;
    private Rectangle rect;
    private Color color = Color.Gray;

    public Wall(Rectangle rect)
    {
        this.rect = rect;
        this.position = new Vector2(rect.X, rect.Y);
    }

    public override void Draw()
    {
        DrawRectangleRec(rect, color);
    }
}