using System.Numerics;

namespace Cs_raylib_test.Entities;

public partial class Player : Entity
{
        private void PlayerMovement()
        {
                
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