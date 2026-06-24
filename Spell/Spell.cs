using System.Numerics;

namespace Cs_raylib_test.Spell;

public abstract class Spell
{
    public bool isAlive = true;

    public virtual void update()
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