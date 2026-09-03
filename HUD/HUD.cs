using System.Numerics;
using Cs_raylib_test.Entities;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class HUD
{
    Player player;
    HUD_GLOBE healthBar;
    HUD_GLOBE manaBar;
    SpellBar spellBar;
    Stats stats;
    
    public HUD(Player player)
    { 
        this.player = player;
        healthBar = new HUD_GLOBE(new Vector2(30, 900), 75f, Color.Red, Color.Gold);
        manaBar = new HUD_GLOBE(new Vector2(GetScreenWidth() - 30 - 75*2, 900), 75f, Color.DarkBlue, Color.Gold);
        spellBar = new SpellBar(player, new Vector2(1920f/2f - 250, 950), new Vector2(500, 100));
        stats = new Stats(new Vector2(30, 30),  new Vector2(350, 100), player);
    }

    public void update()
    {
        healthBar.update(player.getGlobalStats().Health, player.getGlobalStats().MaxHealth);
        manaBar.update(player.getGlobalStats().Mana, player.getGlobalStats().MaxMana);
    }

    public void draw()
    {
        healthBar.draw();
        manaBar.draw();
        spellBar.Draw();
        stats.draw();
    }
}