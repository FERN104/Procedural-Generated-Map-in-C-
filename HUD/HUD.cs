using Cs_raylib_test.Entities;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class HUD
{
    Health_Bar healthBar;
    
    public HUD(Player player)
    { 
        healthBar = new Health_Bar(player, new Rectangle(10f, 10f, 400f, 30f));
    }

    public void update()
    {
        healthBar.update();
    }

    public void draw()
    {
        healthBar.draw();
    }
}