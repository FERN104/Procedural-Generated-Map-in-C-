using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Raylib_cs;

namespace Cs_raylib_test.Spell;

public class Fireball : Spell
{
    //private Texture2D fireballTexture;
    private Vector2 startpos;
    private Vector2 position;
    private Vector2 velocity;
    private float speed = 100f;
    private float radius = 5f;
    private float range = 400f;

    public Fireball(Entity caster) : base(caster)
    {
        damage = 10;
    }

    public override void update(MapGrids map)
    {
        if (!isAlive) return;
        position += velocity;
        if (MathF.Sqrt((position.X - startpos.X)*(position.X - startpos.X) + (position.Y - startpos.Y)*(position.Y - startpos.Y)) > range)
        {
            isAlive = false;
        }

        foreach (var entity in map.GetCellAtPosition(position).Current_Entities)
        {
            if (entity == null) continue;
            if (entity == caster) continue;
            
            entity.Damage(damage);
        }
    }

    public override void draw()
    {
        DrawCircle((int)position.X, (int)position.Y, radius, Color.Orange);
    }

    public override void Reset(Vector2 dir, Vector2 pos)
    {
        base.Reset(dir, pos);
        startpos = pos;
        velocity = dir * speed;
        position = pos;
    }
}