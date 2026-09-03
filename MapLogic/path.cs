using System.Diagnostics.Contracts;
using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Raylib_cs;

namespace Cs_raylib_test.MapLogic;

public class Path : MapObject
{
    private Rectangle rect;
    private int PathImageCount = 4;
    private Texture2D PathImage;
    
    public Path(Rectangle rect)
    {
        this.rect = rect;

        int num = Random.Shared.Next(1, PathImageCount+1);
        PathImage = TextureManager.loadPathtoText("Assets/Paths/Path-" + num + ".png", (int)rect.Width, (int)rect.Height);
    }

    public override void Draw()
    {
        DrawTextureRec(PathImage, new Rectangle(0, 0, rect.Width, rect.Height), new Vector2(rect.X, rect.Y), Color.White);
    }
}