using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Physics;
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
    private Enemy enemy;
    
    MapGrids grid;
    CollisionManager collisionManager;
    
    List<Entity> entities;

    private Camera2D camera;
    
    public GameScreen()
    {
        entities = new List<Entity>(); // Initialise the list
        
        /* Map */
        grid = new MapGrids(1920*3, 1080*3, 8); // Create The grid the map is on
        collisionManager = new CollisionManager(grid);
        
        /* Menu Objects */
        this.resume = new Buttons("Resume", (int)(GetScreenCenter().X) - 250, (int)(GetScreenCenter().Y-150), 500, 200, Color.White, 100);
        this.menu = new TexturedButton("Assets/Menu.png", "Assets/ClickedMenu.png",
            new Rectangle(GetScreenCenter().X - 250, GetScreenCenter().Y + 150, 500, 200));
        this.pause = new Buttons("Pause", GetScreenWidth()-130, 30, 100, 50, Color.White, 20);
        
        /* Game Objects */
        player = new Player(grid);
        entities.Add(player); // Add the player object so the map knows it exists

        enemy = new Enemy(grid);
        entities.Add(enemy);
        
        camera = new Camera2D();
        camera.Offset = new Vector2(GetScreenWidth()/2, GetScreenHeight()/2);
        camera.Target = player.getGlobalPhysics().position;
        camera.Zoom = 1f;
        camera.Rotation = 0f;

    }
    
    public override SceneSwitch update()
    {
        
        if (IsKeyPressed(KeyboardKey.Escape))
        {
            isPaused = !isPaused;
        }

        if (!isPaused)
        {
            camera.Target = player.getGlobalPhysics().position; //Smooth following
            
            foreach (Entity e in entities)
                e.update(GetScreenToWorld2D(GetMousePosition(), camera), grid);
            
            SpellManager.Instance.update(player);
            
            enemy.enemyAI(player);
            
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
        BeginMode2D(camera);
        player.draw();
        enemy.draw();
        SpellManager.Instance.draw();
        grid.Draw();
        EndMode2D();
        
        pause.draw();
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