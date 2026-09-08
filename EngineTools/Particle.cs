using Raylib_cs;
using System.Numerics;

namespace Cs_raylib_test.Engine_Tools;

public class Particle
{
    private Vector2 position;
    private Vector2 velocity;
    private float diameter = 5f;
    private Color color;
    private float maxLife = 3f;
    private float lifetime;
    private bool alive = false;
    private float speed = 10f;
    
    
    public Particle(Vector2 pos, Color color)
    {
        lifetime = maxLife;
        
        position = pos;
        velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle());
        
        //add random direction in the negatives aswell
        if (Random.Shared.NextDouble() < 0.5)
            velocity.X *= -1;
        if  (Random.Shared.NextDouble() < 0.5)
            velocity.Y *= -1;
        
        diameter = 5f;
        this.color = color;
        alive = true;
    }
    
    public void Reset(Vector2 pos, Color col)
    {
        lifetime = maxLife;

        position = pos;
        velocity = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle());

        diameter = 5f;
        this.color = col;
        alive = true;
    }

    public void update()
    {
        lifetime -= GetFrameTime();
        if (lifetime < 0) alive = false;

        diameter *= lifetime/maxLife;
        position += velocity * GetFrameTime() * speed;
    }

    public void draw()
    {
        DrawCircle((int)position.X, (int)position.Y, diameter, color);
    }

    public bool isAlive()
    {
        return alive;
    }
}