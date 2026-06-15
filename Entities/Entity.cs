using System.Numerics;

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
}

public abstract class Entity
{
    protected GlobalStats globalStats;
    protected GlobalPhysics globalPhysics;

    public GlobalStats getGlobalStats() { return globalStats; }

    public abstract void draw();
    public abstract void update();
}