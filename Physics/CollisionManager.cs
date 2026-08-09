using System.Numerics;
using System.Runtime.CompilerServices;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Raylib_cs;

namespace Cs_raylib_test.Physics;

public class CollisionManager
{
    public static CollisionManager instance;
    private static MapGrids map;
    
    public CollisionManager(MapGrids mapGrid)
    {
        map = mapGrid;
        instance = this;
    }

    public void CheckCollision(Entity entity)
    {
        ref GlobalPhysics physics = ref entity.getGlobalPhysics();
        float width = physics.Hitbox.X;
        float height = physics.Hitbox.Y;
        
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        // --- X Check ---
        float nextX = physics.position.X + physics.velocity.X;
        float moveX = physics.velocity.X;
        
        Rectangle hitboxX = new Rectangle(
            nextX - halfWidth,
            physics.position.Y - halfHeight, // Use current Y
            width,
            height
        );
        
        bool hitWallX = false;
        float targetSnapX = nextX; // Find the closest wall (fixes issues with moving left because it found the wrong wall)
        
        // NOTE: Noticed that the loop checks left to right so I need to check every cell outputted to find the most accurate cell to snap to
        foreach (GridCell cell in map.GetCellsAtRect(hitboxX))
        {
            if (!cell.Walkable)
            {
                if (moveX >= 0) // Moving right snap to left edge
                {
                    float wallLeftEdge = cell.Position.X - halfWidth - 0.01f;
                    if (!hitWallX || wallLeftEdge < targetSnapX)
                    {
                        targetSnapX = wallLeftEdge;
                    }
                    hitWallX = true;
                }
                
                if (moveX <= 0) // Moving left snap to right edge
                {
                    // This was the key side to fix
                    float wallRightEdge = cell.Position.X + map.cellSize + halfWidth + 0.01f;
                    if (!hitWallX || wallRightEdge > targetSnapX)
                    {
                        targetSnapX = wallRightEdge;
                    }
                    hitWallX = true;
                }
            }
        }
        
        // if no hit apply regular movement
        if (hitWallX)
        {
            physics.position.X = targetSnapX;
            physics.velocity.X = 0;
        }
        else
        {
            physics.position.X = nextX;
        }

        // --- Y Check ---
        float nextY = physics.position.Y + physics.velocity.Y;
        float moveY = physics.velocity.Y;
        
        Rectangle hitboxY = new Rectangle(
            physics.position.X - halfWidth, // updated x position after last calculation
            nextY - halfHeight,
            width,
            height
        );
        
        bool hitWallY = false;
        float targetSnapY = nextY;
        foreach (GridCell cell in map.GetCellsAtRect(hitboxY))
        {
            if (!cell.Walkable)
            {
                if (moveY >= 0) // Moving down snap to top edge
                {
                    float wallTopEdge = cell.Position.Y - halfHeight - 0.01f;

                    if (!hitWallY || wallTopEdge < targetSnapY)
                    {
                        targetSnapY = wallTopEdge;
                    }
                    hitWallY = true;
                }
                else if (moveY <= 0) // Moving up snap to bottom edge
                {
                    float wallBottomEdge = cell.Position.Y + map.cellSize + halfHeight + 0.01f;
                    if (!hitWallY || wallBottomEdge > targetSnapY)
                    {
                        targetSnapY = wallBottomEdge;
                    }
                    hitWallY = true;
                }
            }
        }

        // If we didn't hit a wall, apply normal Y movement
        if (hitWallY)
        {
            physics.position.Y = targetSnapY;
            physics.velocity.Y = 0;
        }
        else
        {
            physics.position.Y = nextY;
        }
    }
}