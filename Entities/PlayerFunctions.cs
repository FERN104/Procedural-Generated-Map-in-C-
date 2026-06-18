using System.Numerics;
using Cs_raylib_test.Settings;
using Raylib_cs;

namespace Cs_raylib_test.Entities;

public partial class Player : Entity
{
        private void PlayerMovement()
        {
            globalPhysics.position.X += globalPhysics.velocity.X;                                                       // Register velocity
            globalPhysics.position.Y += globalPhysics.velocity.Y;
            
            globalPhysics.velocity = Vector2.Zero;                                                                      // Completely reset velocity to ensure it doesn't accelerate
            
            Vector2 mouspos = GetMousePosition();
            
            // Movement
            if (IsMouseButtonDown(SettingsManager.singleInstance.gameSettings.controls.move)) targetpos = mouspos;      // Update the mouse position in the target pos Vector
                                                                                                                        // Only do this when holding left-click
            // Normalise Vector to give pure direction
            float dx = (targetpos.X - globalPhysics.position.X);                                                        // Redefine with target position to keep going to clicked location
            float dy = (targetpos.Y - globalPhysics.position.Y);  
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);                                                       // Find distance with Pythagoras theorem
            
            // Rotation
            double radians = (float)Math.Atan2(dy, dx);                                                                 // Atan returns radians
            globalPhysics.rotation = (float)(radians * (180.0 / Math.PI)) + 90;                                         // Convert radians to degrees and add offset
                                                                                                                        // because our character is facing up
            if (distance > globalPhysics.speed){
                globalPhysics.velocity.X = (dx / distance) * globalPhysics.speed;                                       // this uses the vector normalisation formula to create a pure direction (Distance is 1).
                globalPhysics.velocity.Y = (dy / distance) * globalPhysics.speed;                                       // Formula: Normalised = Vector / Magnitude
                                                                                                                        // Multiplying by speed then allows us to move the player along the vector
                // Collisions
                
            }

            isMoving = (globalPhysics.velocity.X != 0 || globalPhysics.velocity.Y != 0);                                // Updates Animation Boolean flag (tells the animator whether to walk or not)
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