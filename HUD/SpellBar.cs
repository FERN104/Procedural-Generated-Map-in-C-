using System.Data;
using System.Numerics;
using Cs_raylib_test.Engine_Tools;
using Cs_raylib_test.Entities;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class Slot
{
    public Rectangle rect;
    public Texture2D texture;
    private Cooldown cooldown;

    public Slot(Rectangle rect, String path, Cooldown cooldown)
    {
        texture = TextureManager.loadPathtoText(path, (int)rect.Width, (int)rect.Height);
        this.rect = rect;
        this.cooldown = cooldown;
    }
    
    public void Draw()
    {
        DrawTextureRec(texture, new Rectangle(0, 0, rect.Width, rect.Height), new Vector2(rect.X, rect.Y),Color.White);
        if (cooldown.getCooldownPercent() < 1)
            DrawRectangle((int)rect.X, (int)(rect.Y + (rect.Height * (cooldown.getCooldownPercent()))), (int)rect.Width, (int)(rect.Height * (1- cooldown.getCooldownPercent())), Fade(Color.Black, 0.7f));
    }
}

public class SpellBar
{
    private Vector2 position;
    private Vector2 size;
    private float buffer;
    private float slotSize = 75;
    private List<Slot> slots;

    public SpellBar(Player player, Vector2 position, Vector2 size)
    {
        slots = new List<Slot>();
        buffer = (size.X / 20);
        
        this.position = position;
        this.size = size;
        
        //Create each slot
        slots.Add(new Slot(new Rectangle(position.X + buffer, position.Y + (size.Y - slotSize)/2, slotSize, slotSize), "", player.getSpellCooldowns().LightningBolt));
        slots.Add(new Slot(new Rectangle(position.X + buffer + buffer*4, position.Y + (size.Y - slotSize)/2, slotSize, slotSize), "Assets/FireballUI.png", player.getSpellCooldowns().fireball));
    }

    public void Draw()
    {
        DrawRectangleRec(new Rectangle(position.X, position.Y, size.X, size.Y), Color.DarkGray);
        foreach (Slot slot in slots)
            slot.Draw();
    }

}