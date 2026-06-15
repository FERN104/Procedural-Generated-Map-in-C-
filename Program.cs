using Raylib_cs;
using Cs_raylib_test.Scenes;

namespace Cs_raylib_test;
public enum SceneSwitch {
    MAIN_MENU,
    GAME_SCREEN,
    QUIT,
}

class MainClass
{
    private static Dictionary<SceneSwitch, Func<Scene>> SceneRegistry = new()
    {
        { SceneSwitch.MAIN_MENU, () => new MainMenu() },
        {SceneSwitch.GAME_SCREEN, () => new GameScreen() },
    };
    
    static void Main(string[] args)
    {
        InitWindow(800, 1200, "C# Raylib Test");
        ToggleBorderlessWindowed();
        
        SetTargetFPS(60);
        SetExitKey(KeyboardKey.Null);
        
        SceneSwitch currentScene = SceneSwitch.MAIN_MENU;
        Scene activeScene = SceneRegistry[currentScene]();
        while (!WindowShouldClose())
        {
            SceneSwitch nextScene = activeScene.update();

            if (nextScene == SceneSwitch.QUIT) break;
            if (nextScene != currentScene)
            {
                currentScene = nextScene;
                activeScene = SceneRegistry[currentScene]();
            }

            BeginDrawing();
            ClearBackground(Color.Black);
            
            activeScene.draw();
            
            EndDrawing();
        }
        
        CloseWindow();
    }
}