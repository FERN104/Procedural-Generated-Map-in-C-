using System.Numerics;
using Cs_raylib_test.MapLogic;
using Raylib_cs;

namespace Cs_raylib_test.Entities;

public struct GlobalStats
{
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;
    public float damageMultiplier;
    public int attackDamage;
    public float attackDelay;
}
public struct GlobalPhysics
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 Hitbox;
    public float rotation;
    public float speed;
}

public struct TextureVars
{
    public Texture2D spriteSheet;
    public Rectangle frameRec;
    public Vector2 spriteSheetSize;
    public Vector2 frameDimensions;
    public int frameColumnCount;
    public int currentFrame;
    public int numberOfFrames;
    public float animDuration;
    public float frameTime;
}

public abstract class Entity
{
    protected GlobalStats globalStats;
    protected GlobalPhysics globalPhysics;
    protected TextureVars textureVars;
    protected MapGrids map;
    public Vector2 oldTarget;
    public Vector2 targetPos;

    public ref GlobalStats getGlobalStats() { return ref globalStats; }
    public ref GlobalPhysics getGlobalPhysics() { return ref globalPhysics; }
    public ref TextureVars getTextureVars() { return ref textureVars; }
    public abstract void draw();

    public abstract void update(Vector2 mousePos, MapGrids map);

    public virtual Rectangle getRectangle()
    {
        return new Rectangle(this.globalPhysics.position.X, globalPhysics.position.Y, globalPhysics.Hitbox.X, globalPhysics.Hitbox.Y);
    }

    public Entity(MapGrids grid)
    {
        map = grid;
    }
    
    public void Damage(int amount)
    {
        globalStats.Health -= amount;
    }
    public bool Alive()
    {
        return globalStats.Health > 0;
    }
}