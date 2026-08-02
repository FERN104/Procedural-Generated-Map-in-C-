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
        Vector2 nextPos = entity.getGlobalPhysics().position +
                          entity.getGlobalPhysics().velocity;
        foreach (GridCell cell in map.GetCellsAtRect(
                     new Rectangle(nextPos.X,
                         nextPos.Y,
                         entity.getTextureVars().frameRec.Width/2.0f,
                         entity.getTextureVars().frameRec.Height/2.0f)))
        {
            if (cell.Walkable == false)
            {
                entity.getGlobalPhysics().velocity = Vector2.Zero;
                break;
            }
        }
    }
}