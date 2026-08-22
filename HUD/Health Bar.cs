using Cs_raylib_test.Entities;
using Raylib_cs;
namespace Cs_raylib_test.UI_Elements;

public class Health_Bar
{
    private Rectangle rect;
    private float healthPercentage;
    private Player player;

    public Health_Bar(Player player, Rectangle rect)
    {
        this.player = player;
        this.rect = rect;
        
        this.healthPercentage = ((float)player.getGlobalStats().Health / (float)player.getGlobalStats().MaxHealth);
    }

    public void update()
    {
        this.healthPercentage = ((float)player.getGlobalStats().Health / (float)player.getGlobalStats().MaxHealth);
        Console.Write('\n'+healthPercentage);
    }

    public void draw()
    {
        DrawRectangleRec(rect, Color.Red);
        DrawRectangle((int)rect.X, (int)rect.Y, (int)(rect.Width * healthPercentage), (int)rect.Height, Color.Green);
    }
}