using System.Numerics;
using Raylib_cs;

namespace Cs_raylib_test.Engine_Tools;

public class ParticleManager
{
    private Stack<Particle> particlePool;
    private List<Particle> activeParticles;
    private int maxExplosion = 20;
    
    private static ParticleManager instance;
    public static ParticleManager Instance => instance ??= new ParticleManager();
    
    private ParticleManager()
    {
        particlePool = new Stack<Particle>();
        activeParticles = new List<Particle>();
    }

    public void update()
    {
        foreach (Particle activeParticle in activeParticles)
            activeParticle.update();
        activeParticles.RemoveAll(e =>
        {
            if (!e.isAlive())
            {
                particlePool.Push(e);
                return true;
            }
            return false;
        });
    }

    public void draw()
    {
        foreach (Particle activeParticle in activeParticles)
            activeParticle.draw();
    }

    public void spawn(Vector2 pos, Color color)
    {
        if (!particlePool.Any()) 
            activeParticles.Add(new Particle(pos, color));
        else
        {
            Particle p = particlePool.Pop();
            p.Reset(pos, color);
            activeParticles.Add(p);
        }
    }
    
    public void Explode(Vector2 pos, Color col, float scale)
    {
        for (int i = 0; i < maxExplosion * scale; i++)
            spawn(pos, col);
    }
    
}