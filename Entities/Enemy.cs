using System.ComponentModel;
using System.Numerics;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Physics;
using Raylib_cs;

namespace Cs_raylib_test.Entities;

public enum States
{
    ATTACKING,
    TARGETING,
    ROAMING,
    IDLE,
    DEAD
}


public class Enemy : Entity
{
    private bool isMoving = false;
    private States state = States.IDLE;

    private Vector2 targetPos;

    private int sightRange = 800;
    private int attackRange = 1;
    
    public Enemy(MapGrids map) : base(map)
    {
        globalPhysics.Hitbox = new Vector2(100, 100);
        globalPhysics.position = map.findEmptyGrid(new Vector2(globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y));
        globalPhysics.speed = 4;

        targetPos = globalPhysics.position;
    }

    public override void update(Vector2 mousePos, MapGrids map)
    {
        CollisionManager.instance.MoveToPoint(this, targetPos, (dir) => { });
        return;
    }

    public override void draw()
    {
        DrawRectangleRec(new Rectangle(globalPhysics.position.X, globalPhysics.position.Y, globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y), Color.Red);
    }

    public void enemyAI(Player player)
    {
        if (state == States.DEAD) return;
        
        // Logic to decide what State we are in
        int dist = (int)Vector2.Distance(player.getGlobalPhysics().position, globalPhysics.position);
        
        if (attackRange > dist)
            state = States.ATTACKING;
        
        else if (sightRange > dist) // If The player is outside line of sight exit
        {
            if (ClearLineOfSight(globalPhysics.position, player.getGlobalPhysics().position))
                state = States.TARGETING;
        }
        
        else state = States.TARGETING;
        
        switch (state) // What state are we in? What do we do?
        {
            case States.ATTACKING: Attack(); break;
            case States.TARGETING: Target(player); break;
            case States.ROAMING: Roaming(); break;
            default: return;
        }
    }

    private void Attack()
    {
        
    }

    private void Target(Player player)
    {
        targetPos = player.getGlobalPhysics().position;
    }

    private bool ClearLineOfSight(Vector2 pos, Vector2 target) // Can the enemy see the player?
    {
        GridCell currentCell = map.GetCellAtPosition(pos);
        GridCell targetCell = map.GetCellAtPosition(target);

        if (currentCell == null || targetCell == null)
            return false;
        
        // Convert to indexes with a cast to integer
        // Casting to integer drops the decimal place (floor division)
        int x0 = (int)(currentCell.Position.X / map.cellSize);
        int y0 = (int)(currentCell.Position.Y / map.cellSize);

        int x1 = (int)(targetCell.Position.X / map.cellSize);
        int y1 = (int)(targetCell.Position.Y / map.cellSize);
        
        //Distance Calculations
        int dx = Math.Abs(x0 - x1);
        int dy = Math.Abs(y0 - y1);

        int moveX = x0 > x1 ? 1 : -1;
        int moveY = y0 > y1 ? 1 : -1;

        int err = dx - dy;
        
        while (true)
        {
            if (!map.GetCellAt(x0,y0).Walkable) return false; 
            
            if (x0 == x1 && y0 == y1) 
                break;
            
            int e2 = err * 2; // Doubling is done to keep everything in integer space removes floating point arithmetic
            
            
            // This part is weighing which side needs to move this frame via the formulas
            if (e2 > -dy)
            {
                err -= dy;
                x0 += moveX;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += moveY;
            }
            // this works because we check how for the line is drifting in each direction to decide what axis to move on
        }

        return true;
    }

    private void Roaming()
    {
        
    }
}