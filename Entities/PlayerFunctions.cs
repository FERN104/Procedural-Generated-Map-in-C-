using System.Numerics;

namespace Cs_raylib_test.Entities;

public partial class Player : Entity
{
        private void PlayerMovement()
        {
            Vector2 mouspos = GetMousePosition();
            Vector2 targetpos = new Vector2(0,0);
            
            //Calculate deltas
            float dx = (mouspos.X - globalPhysics.position.X);                                                          // difference in x
            float dy = (mouspos.Y - globalPhysics.position.Y);                                                          // difference in y
            
            // Rotation
            double radians = (float)Math.Atan2(dy, dx);                                                                 // Atan returns radians
            globalPhysics.rotation = (float)(radians * (180.0 / Math.PI)) + 90;                                         // Convert radians to degrees and add offset
                                                                                                                        // because our character is facing up
            //Movement
            if (IsMouseButtonDown(0)) targetpos = mouspos;
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