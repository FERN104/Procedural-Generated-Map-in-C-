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
    private float speed = 15f;
    private float radius = 7f;
    private float range = 400f;

    public Fireball(Entity caster) : base(caster)
    {
        caster.getSpellCooldowns().fireball.reset();
        damage = 10;
        color = Color.Orange;
    }

    public override void update(MapGrids map)
    {
        if (!isAlive) return;
        position += velocity;
        if (MathF.Sqrt((position.X - startpos.X)*(position.X - startpos.X) + (position.Y - startpos.Y)*(position.Y - startpos.Y)) > range)
        {
            isAlive = false;
        }

        GridCell currentCell = map.GetCellAtPosition(position);

        if (!currentCell.Walkable)
        {
            isAlive = false;
            return;
        }

        foreach (var entity in currentCell.Current_Entities)
        {
            if (entity == null) continue;
            if (entity == caster) continue;
            entity.Damage(damage);
            if (!entity.Alive() && caster is Player player)
                player.getGameStats().kills += 1;
                
            isAlive = false;
        }
        
        ParticleManager.Instance.spawn(position, color);
    }

    public override void draw()
    {
        DrawCircle((int)position.X, (int)position.Y, radius, color);
    }

    public override void Reset(Vector2 dir, Vector2 pos)
    {
        caster.getSpellCooldowns().fireball.reset();
        base.Reset(dir, pos);
        startpos = pos;
        velocity = dir * speed;
        position = pos;
    }
}