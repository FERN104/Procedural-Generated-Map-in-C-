using System.Numerics;

namespace Cs_raylib_test.Entities;

public class Player : Entity
{
    
    public Player()
    {
        
    }

    public override void update()
    {
        Console.WriteLine("Stuff");
    }

    public override void draw()
    {
        Console.WriteLine("Stuff");
        return;
    }
}