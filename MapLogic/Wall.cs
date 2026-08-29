using System.Diagnostics.Contracts;
using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Raylib_cs;

namespace Cs_raylib_test.MapLogic;

public class Wall : MapObject
{
    private Rectangle rect;
    private int WallImageCount = 4;
    private Texture2D WallImage;
    
    public Wall(Rectangle rect)
    {
        this.rect = rect;

        int num = Random.Shared.Next(1, WallImageCount+1);
        WallImage = TextureManager.loadPathtoText("Assets/Walls/Wall-" + num + ".png", (int)rect.Width, (int)rect.Height);
    }

    public override void Draw()
    {
        DrawTextureRec(WallImage, new Rectangle(0, 0, rect.Width, rect.Height), new Vector2(rect.X, rect.Y), Color.White);
    }
}