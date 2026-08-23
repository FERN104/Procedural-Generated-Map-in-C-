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
    
    public void draw()
    {
        DrawTextureRec(texture, rect, new Vector2(rect.X, rect.Y),Color.White);
        DrawRectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)(rect.Height * cooldown.getCooldownPercent()), Fade(Color.Black, 0.7f));
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
        slots.Add(new Slot(new Rectangle(position.X + buffer, position.Y + (position.Y - slotSize), slotSize, slotSize), "", player.getSpellCooldowns().fireball));
        // slots[1] = new Slot(new Rectangle((position.X + buffer*2)*2, position.Y + (position.Y - slotSize), slotSize, slotSize), "");
        // slots[2] = new Slot(new Rectangle((position.X + buffer*2)*3, position.Y + (position.Y - slotSize), slotSize, slotSize), "");
        // slots[3] = new Slot(new Rectangle((position.X + buffer*2)*4, position.Y + (position.Y - slotSize), slotSize, slotSize), "");
        // slots[4] = new Slot(new Rectangle((position.X + buffer*2)*5, position.Y + (position.Y - slotSize), slotSize, slotSize), "");
    }

    public void draw()
    {
        DrawRectangleRec(new Rectangle(position.X, position.Y, size.X, size.Y), Color.DarkGray);
        foreach (Slot slot in slots)
            slot.draw();
    }

}