using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Raylib_cs;

namespace Cs_raylib_test.Spell;

public class LightningBolt : Spell
{
    //private Texture2D fireballTexture;
    private Vector2 startpos;
    private Vector2 position;
    private Vector2 velocity;
    private float speed = 200f;
    private float radius = 5f;
    private float range = 400f;
    private float chainRadius = 300f;
    private int maxChains = 5;
    private int currentChain = 0;
    

    public LightningBolt(Entity caster) : base(caster)
    {
        caster.getSpellCooldowns().LightningBolt.reset();
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
            
            if (currentChain > maxChains)
                isAlive = false;
        }
    }

    public override void draw()
    {
        DrawCircle((int)position.X, (int)position.Y, radius, Color.Blue);
    }

    public override void Reset(Vector2 dir, Vector2 pos)
    {
        caster.getSpellCooldowns().LightningBolt.reset();
        base.Reset(dir, pos);
        startpos = pos;
        velocity = dir * speed;
        position = pos;
    }
}