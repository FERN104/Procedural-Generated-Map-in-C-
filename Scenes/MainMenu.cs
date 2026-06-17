using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.UI_Elements;
using Raylib_cs;

namespace Cs_raylib_test.Scenes;

public class MainMenu : Scene
{
    private Vector2 center = GetScreenCenter();
    private TexturedButton play;
    private Buttons quit;
    
    public MainMenu()
    {
        this.quit = new Buttons("Quit", (int)center.X - 500/2, (int)center.Y + 150, 500, 200, Color.White, 100);
       this.play = new TexturedButton("Assets/Play.png", "Assets/ClickedPlay.png", new Rectangle(center.X - 500/2, center.Y - 150, 500, 200));
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
        DrawText("SPELL RAIDER", (int)(center.X - MeasureText("SPELL RAIDER", 200)/2), 100, 200, Color.Red);
        play.draw();
        quit.draw();
    }

    public override void Dispose()
    {
        TextureManager.UnloadTextCache();
    }
}