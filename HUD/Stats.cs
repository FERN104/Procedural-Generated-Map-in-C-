using System.Numerics;
using Cs_raylib_test.Entities;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class Stats
{
    private Icon skull;
    private Icon coin;
    private Vector2 pos;
    private Vector2 size;
    private Player player;
    
    public Stats(Vector2 pos, Vector2 size, Player player)
    {
        this.player = player;
        skull = new Icon("Assets/skull.png", new Vector2(pos.X +5, pos.Y), new Vector2(100, 100));
        coin = new Icon("Assets/coin.png", new Vector2(pos.X + 100 + 50 + 10, pos.Y), new Vector2(100, 100));
        this.pos = pos;
        this.size = size;
    }

    public void draw()
    {
        DrawRectangleRec(new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y), Fade(Color.Gray, 0.4f));
        skull.draw();

        int killX = (int)(skull.pos.X + skull.size.X + 10);
        int killY = (int)(skull.pos.Y + skull.size.Y/4);
        int fontSize = (int)skull.size.Y / 2;
        
        DrawText(player.getGameStats().kills.ToString(), killX, killY, fontSize, Color.White);
        coin.draw();
        
        int coinX = (int)(coin.pos.X + coin.size.X + 10);
        int coinY = (int)(coin.pos.Y + coin.size.Y/4);
        fontSize = (int)coin.size.Y / 2;
        
        DrawText(player.getGameStats().collectedCoins.ToString(), coinX, coinY, fontSize, Color.White);
    }
}