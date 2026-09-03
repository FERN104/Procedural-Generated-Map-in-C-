using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class Icon
{
    private Texture2D texture;
    public Vector2 pos;
    public Vector2 size;

    public Icon(String path, Vector2 pos, Vector2 size)
    {
        this.texture = TextureManager.loadPathtoText(path, (int)size.X, (int)size.Y);
        this.pos = pos;
        this.size = size;
    }

    public void draw()
    {
        DrawTextureRec(texture, new Rectangle(0, 0, (int)size.X, (int)size.Y), pos, Color.White);
    }
}