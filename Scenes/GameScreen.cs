using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Spell;
using Cs_raylib_test.UI_Elements;
using Raylib_cs;

namespace Cs_raylib_test.Scenes;

public class GameScreen : Scene
{
    private bool isPaused = false;
    private TexturedButton menu;
    private Buttons resume;
    private Buttons pause;

    private Player player;
    MapGrids grid;
    
    List<Entity> entities;
    
    public GameScreen()
    {
        entities = new List<Entity>(); // Initialise the list
        
        /* Menu Objects */
        this.resume = new Buttons("Resume", (int)(GetScreenCenter().X) - 250, (int)(GetScreenCenter().Y-150), 500, 200, Color.White, 100);
        this.menu = new TexturedButton("Assets/Menu.png", "Assets/ClickedMenu.png",
            new Rectangle(GetScreenCenter().X - 250, GetScreenCenter().Y + 150, 500, 200));
        this.pause = new Buttons("Pause", GetScreenWidth()-130, 30, 100, 50, Color.White, 20);
        
        /* Game Objects */
        player = new Player();
        entities.Add(player); // Add the player object so the map knows it exists
        
        /* Map */
        grid = new MapGrids(1920, 1080, 32); // Create The grid the map is on

    }
    
    public override SceneSwitch update()
    {
        if (IsKeyPressed(KeyboardKey.Escape))
        {
            isPaused = !isPaused;
        }

        if (!isPaused)
        {
            SpellManager.Instance.update(player);
            player.update();
            pause.update();
            if (pause.getIsClicked())
            {
                isPaused = true;
                pause.setIsClicked(false);
            }
            
            grid.UpdateCells(entities); // While the game is active update the map grid
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
        player.draw();
        pause.draw();
        SpellManager.Instance.draw();
        if (isPaused)
        {
            DrawText("Game Paused", (int)(GetScreenCenter().X - MeasureText("Game Paused", 100)/2), 100, 100, Color.Red);
            menu.draw();   
            resume.draw();
        }
    }

    public override void Dispose()
    {
        TextureManager.UnloadTextCache();
        SpellManager.Instance.Dispose();
    }
}