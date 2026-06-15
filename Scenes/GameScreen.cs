using Cs_raylib_test.UI_Elements;
using Raylib_cs;

namespace Cs_raylib_test.Scenes;

public class GameScreen : Scene
{
    private bool isPaused = false;
    private Buttons menu;
    private Buttons resume;
    private Buttons pause;

    public GameScreen()
    {
        this.resume = new Buttons("Resume", (int)(GetScreenCenter().X) - 250, (int)(GetScreenCenter().Y-150), 500, 200, Color.White, 100);
        this.menu = new Buttons("Menu", (int)(GetScreenCenter().X) - 250, (int)(GetScreenCenter().Y+150), 500, 200, Color.White, 100);
        this.pause = new Buttons("Pause", GetScreenWidth()-130, 30, 100, 50, Color.White, 20);
    }
    public override SceneSwitch update()
    {
        if (IsKeyPressed(KeyboardKey.Escape))
        {
            isPaused = !isPaused;
        }

        if (!isPaused)
        {
            
            if (IsKeyPressed(KeyboardKey.Enter)) return SceneSwitch.MAIN_MENU;
            
            pause.update();
            if (pause.getIsClicked())
            {
                isPaused = true;
                pause.setIsClicked(false);
            }
        }
        else
        {
            menu.update();
            resume.update();

            if (menu.getIsClicked())
            {
                return SceneSwitch.MAIN_MENU;
            }

            if (resume.getIsClicked())
            {
                isPaused = false;
                resume.setIsClicked(false);
            }
        }

        return SceneSwitch.GAME_SCREEN;
    }

    public override void draw()
    {
        pause.draw();
        
        if (isPaused)
        {
            DrawText("Game Paused", (int)(GetScreenCenter().X - MeasureText("Game Paused", 100)/2), 100, 100, Color.Red);
            menu.draw();   
            resume.draw();
        }
    }
}