using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.UI_Elements;
using Raylib_cs;

namespace Cs_raylib_test.Scenes;

public class MainMenu : Scene
{
    private Vector2 center = GetScreenCenter();
    private TexturedButton play;
    private TexturedButton quit;
    Texture2D background;
    
    public MainMenu()
    {
        this.quit = new TexturedButton("Assets/Quit.png", "Assets/ClickedQuit.png", new Rectangle(center.X + 250, center.Y + 50, 500, 200));
        this.play = new TexturedButton("Assets/Play.png", "Assets/ClickedPlay.png", new Rectangle(center.X + 250, center.Y - 250, 500, 200));
       background = TextureManager.loadPathtoText("Assets/MenuBackground.png", GetScreenWidth(), GetScreenHeight());
    }
    public override SceneSwitch update()
    {
        play.update();
        quit.update();
        
        if (play.getIsClicked()) return SceneSwitch.GAME_SCREEN;
        if (quit.getIsClicked()) return SceneSwitch.QUIT;
        
        return SceneSwitch.MAIN_MENU;
    }

    public override void draw()
    {
        DrawTexture(background, 0, 0, Color.White);
        DrawText("SPELL RAIDER", (int)((center.X + 500)- MeasureText("SPELL RAIDER", 100)/2), 100, 100, Color.Red);
        play.draw();
        quit.draw();
    }

    public override void Dispose()
    {
        TextureManager.UnloadTextCache();
    }
}