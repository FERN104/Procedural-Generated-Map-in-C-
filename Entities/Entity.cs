using System.Numerics;
using Raylib_cs;

namespace Cs_raylib_test.Entities;

public struct GlobalStats
{
    public int Health;
    public int MaxHealth;
    public int Mana;
    public int MaxMana;
    public float damageMultiplier;
}
public struct GlobalPhysics
{
    public Vector2 position;
    public Vector2 velocity;
    public float rotation;
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

    public GlobalStats getGlobalStats() { return globalStats; }

    public abstract void draw();
    public abstract void update();
}