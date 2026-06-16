using Raylib_cs;
using Cs_raylib_test.Scenes;

namespace Cs_raylib_test;
public enum SceneSwitch {                                                               // An enum is like a category
    MAIN_MENU,                                                                          // This enum holds different Scene types
    GAME_SCREEN,                                                                        
    QUIT,
}                                                                                       

class MainClass
{
    private static Dictionary<SceneSwitch, Func<Scene>> SceneRegistry = new()           // A dictionary is a way of storing data with a key and a value
    {                                                                                   // In this case my 'Category' from the Enum is the key and a function is the value
        { SceneSwitch.MAIN_MENU, () => new MainMenu() },                                // Placing a function as the value allows me to create a scene at the time I need it
        {SceneSwitch.GAME_SCREEN, () => new GameScreen() },
    };
    
    static void Main(string[] args)                                                     // This is the programs starting point
    {
        InitWindow(800, 1200, "C# Raylib Test");                        // This is where I use the 'Raylib' framework to create a window
        ToggleBorderlessWindowed();                                                     // This puts the window into borderless fullscreen
        
        SetTargetFPS(60);                                                               // Limiting the framerate to 60 frames per second
        SetExitKey(KeyboardKey.Null);                                                   // The framework has the escape key as the default program exit key, so I disabled it
        
        SceneSwitch currentScene = SceneSwitch.MAIN_MENU;                               // This declares our current scene variable as a type of our enum and makes it begin with the Main Menu
        Scene activeScene = SceneRegistry[currentScene]();                              // This makes the variable 'activeScene' become a scene object by using our dictionary to create an object of the class
        while (!WindowShouldClose())                                                    // The game loop that runs 60 times per second
        {
            SceneSwitch nextScene = activeScene.update();                               // Keeps whatever scene class is active updating
                                                                                        // Next scene holds whatever scene type update returns
            if (nextScene == SceneSwitch.QUIT) break;                                   // Check if update returned Quit to exit the loop and close the game
            if (nextScene != currentScene)                                              // Don't do this unless the next scene is different to the one we are currently on
            {
                activeScene.Dispose();                                                  // Clean Textures or any cached memory from the old scene to prepare for the new one
                currentScene = nextScene;                                               // Make the switch from old scene 'category' to new scene 'category'
                activeScene = SceneRegistry[currentScene]();                            // Activate the scene from the dictionary above
            }

            BeginDrawing();                                                             // Uses Raylib's Drawing function to prepare the screen for colour
            ClearBackground(Color.Black);                                               // Sets the background colour
            
            activeScene.draw();                                                         // Draws the active scene
            
            EndDrawing();                                                               // Stop Drawing mode
        }
        
        CloseWindow();                                                                  // This is outside of the loop and only runs if the program breaks the loop
    }
}