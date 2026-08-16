using System.Numerics;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;

namespace Cs_raylib_test.Spell;

public abstract class Spell
{
    public bool isAlive = true;
    protected int damage;
    protected Entity caster;
    
    
    public Spell(Entity entity)
    {
        this.caster = entity;
    }

    public virtual void update(MapGrids map)
    {
    }

    public virtual void draw()
    {
    }

    public virtual void Reset(Vector2 dir, Vector2 pos)
    {
        isAlive = true;
    }
}