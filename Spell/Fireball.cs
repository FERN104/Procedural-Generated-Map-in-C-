using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Raylib_cs;

namespace Cs_raylib_test.Spell;

public class Fireball : Spell
{
    //private Texture2D fireballTexture;
    private Vector2 startpos;
    private Vector2 position;
    private Vector2 velocity;
    private float speed = 100f;
    private float rotationAngle;
    private float radius = 10f;
    private float range = 400f;
    
    public Fireball (Vector2 direction, Vector2 pos){
        this.position = pos;
        this.startpos = pos;
        this.velocity = direction * speed;
        //this.fireballTexture = TextureManager.loadPathtoText("Assets/fireball.png", 50, 50);
        
    }

    public override void update()
    {
        if (!isAlive) return;
        position += velocity;
        if (MathF.Sqrt((position.X - startpos.X)*(position.X - startpos.X) + (position.Y - startpos.Y)*(position.Y - startpos.Y)) > range)
        {
            isAlive = false;
        }
    }

    public override void draw()
    {
        DrawCircle((int)position.X, (int)position.Y, radius, Color.Orange);
    }

    public override void Reset(Vector2 dir, Vector2 pos)
    {
        base.Reset(dir, pos);
        velocity = dir * speed;
        position = pos;
    }
}