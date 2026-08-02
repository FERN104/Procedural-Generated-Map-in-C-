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
        float width = entity.getTextureVars().frameRec.Width;
        float height = entity.getTextureVars().frameRec.Height;
        
        Rectangle hitbox = new Rectangle(
            entity.getGlobalPhysics().position.X - (entity.getTextureVars().frameRec.Width / 2.0f),
            entity.getGlobalPhysics().position.Y - (entity.getTextureVars().frameRec.Height / 2.0f),
            width,
            height
        );
        
        // X Check
        float nextX  = entity.getGlobalPhysics().position.X + entity.getGlobalPhysics().velocity.X;
        Rectangle hitboxX = new Rectangle(
            nextX - width / 2f,
            hitbox.Y,
            width,
            height
        );
        
        foreach (GridCell cell in map.GetCellsAtRect(hitboxX))
        {
            if (!cell.Walkable)
            {
                entity.getGlobalPhysics().velocity.X = 0;
                break;
            }
        }
        
        // Y Check
        float nextY  = entity.getGlobalPhysics().position.Y + entity.getGlobalPhysics().velocity.Y;
        Rectangle hitboxY = new Rectangle(
            hitbox.X,
            nextY - height / 2f,
            width,
            height
        );
        
        foreach (GridCell cell in map.GetCellsAtRect(hitboxY))
        {
            if (!cell.Walkable)
            {
                entity.getGlobalPhysics().velocity.Y = 0;
                break;
            }
        }
    }
}