using System.Numerics;
using Cs_raylib_test.Engine_Tools;
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
    private bool isAttacking = false;
    
    private States state = States.IDLE;

    private int sightRange = 800;
    private int attackRange = 50;
    
    private int patrolRadius = 500;

    private Cooldown animTimer;
    private Cooldown attack;
    
    public Enemy(MapGrids map) : base(map)
    {
        //Textures
        
        //Sheet setup
        textureVars.spriteSheetSize = new Vector2(256, 128);
        textureVars.frameColumnCount = 2;
        textureVars.spriteSheet = TextureManager.loadPathtoText("Assets/Enemy SpriteSheet.png", (int)textureVars.spriteSheetSize.X, (int)textureVars.spriteSheetSize.Y);
        
        //Frame Setup
        textureVars.frameDimensions = new Vector2(128, 128);
        textureVars.frameRec = new Rectangle(0, 0, textureVars.frameDimensions.X, textureVars.frameDimensions.Y);
        
        textureVars.numberOfFrames = 2;
        textureVars.currentFrame = 0;
        textureVars.frameTime = 0.1f;
        animTimer = new Cooldown(textureVars.frameTime);
        
        // Physics
        globalPhysics.Hitbox = new Vector2(100, 100);
        globalPhysics.position = map.findEmptyGrid(new Vector2(globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y), Random.Shared.Next(0, (int)(map.mapWidth/map.cellSize)), Random.Shared.Next(0, (int)(map.mapHeight/map.cellSize))
            );
        globalPhysics.speed = 6;
        
        //Stats
        globalStats.MaxHealth = 20;
        globalStats.Health = globalStats.MaxHealth;
        globalStats.attackDelay = 1f;
        globalStats.damageMultiplier = 1f;
        globalStats.attackDamage = (int)(5 * globalStats.damageMultiplier);
        
        targetPos = globalPhysics.position;

        attack = new Cooldown(globalStats.attackDelay);
    }

    public override void update(Vector2 mousePos, MapGrids map)
    {
        if (globalStats.Health <= 0)
            state = States.DEAD;
        
        CollisionManager.instance.MoveToPoint(this);
        
        isMoving = (globalPhysics.velocity.X != 0 ||
                    globalPhysics.velocity.Y != 0);   
        
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
            Console.WriteLine($"Frame Count:{textureVars.currentFrame}");
        }
        else if (animTimer.isReady())
        {
            textureVars.frameRec.X = 0;
            textureVars.frameRec.Y = textureVars.frameDimensions.Y;
        }
    }
    
    public void enemyAI(Player player)
    { 
        if (state == States.DEAD) return;
        
        // Logic to decide what State we are in
        int dist = (int)Vector2.Distance(player.getGlobalPhysics().position, globalPhysics.position);
        
        if (dist < attackRange)
            state = States.ATTACKING;
        
        else if (dist < sightRange) // If The player is outside line of sight exit
        {
            if (ClearLineOfSight(globalPhysics.position, player.getGlobalPhysics().position))
                state = States.TARGETING;
            else
                state = States.ROAMING;
        }
        
        else state = States.ROAMING;
        
        switch (state) // What state are we in? What do we do?
        {
            case States.ATTACKING: Attack(player); break;
            case States.TARGETING: Target(player); break;
            case States.ROAMING: Roaming(); break;
            default: return;
        }
    }

    private void Attack(Player player)
    {
        if (attack.isReady())
        {
            attack.reset();
            isAttacking = true;
            player.Damage(globalStats.attackDamage);
        }
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

        int moveX = x0 < x1 ? 1 : -1;
        int moveY = y0 < y1 ? 1 : -1;

        int err = dx - dy;
        
        while (true)
        {
            if (!map.GetCellAt(x0, y0).Walkable)
                return false;

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
        if (targetPos == globalPhysics.position)
        {
            Rectangle patrolSquare = new Rectangle(globalPhysics.position.X - patrolRadius,
                globalPhysics.position.Y - patrolRadius, globalPhysics.position.X + patrolRadius,
                globalPhysics.position.Y + patrolRadius);
            
            targetPos = map.findEmptyGrid(new Vector2(globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y),
                Math.Clamp((int)Random.Shared.Next((int)(patrolSquare.X), (int)(patrolSquare.Width)), 0,
                    (int)(map.mapWidth)),
                Math.Clamp((int)Random.Shared.Next((int)(patrolSquare.Y), (int)(patrolSquare.Height)), 0,
                    (int)(map.mapHeight)));
        }
    }
}