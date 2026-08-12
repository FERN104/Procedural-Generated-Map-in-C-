using System.Numerics;
using System.Runtime.CompilerServices;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Physics;
using Cs_raylib_test.Settings;
using Cs_raylib_test.Spell;
using Raylib_cs;

namespace Cs_raylib_test.Entities;

public partial class Player : Entity
{
    
    private void PlayerMovement(Vector2 mousePos, MapGrids map)
    {

        // Movement
        if (IsMouseButtonDown(SettingsManager.singleInstance.gameSettings.controls.move))
            targetpos = mousePos;                                                                                   // Update the mouse position in the target pos Vector
                                                                                                                    // Only do this when holding left-click
        CollisionManager.instance.MoveToPoint(this, targetpos, dir=> SpellDirection = dir);

        isMoving = (globalPhysics.velocity.X != 0 ||
                    globalPhysics.velocity.Y != 0);                                                                                                 // Updates Animation Boolean flag (tells the animator whether to walk or not)
    }

    private void AnimationLoop()
    {
        if (animTimer.isReady() && isMoving)
        {
            animTimer.reset();
            textureVars.currentFrame++;

            if (textureVars.currentFrame >= textureVars.numberOfFrames)
            {
                textureVars.currentFrame = 0;
            }
            
            int currentCol = textureVars.currentFrame % textureVars.frameColumnCount;
            int currentRow = textureVars.currentFrame / textureVars.frameColumnCount;
            
            textureVars.frameRec.X = currentCol * textureVars.frameDimensions.X;
            textureVars.frameRec.Y = currentRow * textureVars.frameDimensions.Y;
        }
        else if (animTimer.isReady())
        {
            textureVars.frameRec.X = 0;
            textureVars.frameRec.Y = textureVars.frameDimensions.Y;
        }
    }
}