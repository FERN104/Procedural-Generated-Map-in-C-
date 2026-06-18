using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using Raylib_cs;
using Cs_raylib_test.Engine_Tools;
namespace Cs_raylib_test.Entities;

public partial class Player : Entity
{
    private Cooldown animTimer;
    Vector2 targetpos = Vector2.Zero;
    Vector2 intitSpellVel = Vector2.Zero;
    private bool isMoving = true;

    public Player()
    {
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
        globalPhysics.position = new Vector2(100, 100);
        globalPhysics.speed = 5f;
    }

    public override void update()
    {
        PlayerMovement();
        if (GetKeyPressed() != 0) SpellCaster[(KeyboardKey)GetKeyPressed()]();
        AnimationLoop();
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