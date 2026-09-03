using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Raylib_cs;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Settings;

namespace Cs_raylib_test.Entities;

public struct GameStats
{
    public int kills;
    public int collectedCoins;
}

public partial class Player : Entity
{
    private Cooldown animTimer;
    private bool isMoving = true;
    private GameStats gameStats;
    public Player(MapGrids map) : base(map)
    {
        gameStats = new GameStats();
        
        // Load all textures on initialisaton
        textureVars.spriteSheetSize = new Vector2(208, 216);
        textureVars.frameColumnCount = 2;
        textureVars.spriteSheet = TextureManager.loadPathtoText("Assets/WizardSpriteSheet.png", (int)textureVars.spriteSheetSize.X, (int)textureVars.spriteSheetSize.Y);
        
        textureVars.frameDimensions = new Vector2(104, 108);
        textureVars.frameRec = new Rectangle(0.0f, 0.0f, textureVars.frameDimensions.X, textureVars.frameDimensions.Y);

        textureVars.numberOfFrames = 2;
        textureVars.currentFrame = 0;
        textureVars.frameTime = 0.1f;
        animTimer = new Cooldown(textureVars.frameTime);
        
        //Change starting position values
        globalPhysics.Hitbox = new(104, 108);
        globalPhysics.position = map.findEmptyGrid(new Vector2(globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y), 0, 0);
        globalPhysics.speed = 13f;
        
        // Stats
        globalStats.MaxHealth = 100;
        globalStats.Health = globalStats.MaxHealth;
        
        globalStats.MaxMana = 100;
        globalStats.Mana = globalStats.MaxMana;
        
        //Spell Cooldowns
        spellCooldowns.fireball = new Cooldown(0.2f);
        
        targetPos = globalPhysics.position;
    }

    public override void update(Vector2 mousePos, MapGrids map)
    {
        PlayerMovement(mousePos);
        AnimationLoop();
    }

    public ref GameStats getGameStats()
    {
        return ref gameStats;
    }

    public override void draw()
    {
        DrawTexturePro(
            textureVars.spriteSheet, 
            textureVars.frameRec, 
            new Rectangle(globalPhysics.position.X, globalPhysics.position.Y, textureVars.frameRec.Width, textureVars.frameRec.Height), 
            new Vector2(textureVars.frameRec.Width/2.0f, textureVars.frameRec.Height/2.0f),
            globalPhysics.rotation,
            Color.White);
    }
}