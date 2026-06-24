using System.Numerics;
using Cs_raylib_test.Entities;
using Cs_raylib_test.Settings;
using Raylib_cs;

namespace Cs_raylib_test.Spell;

public class SpellManager
{
    private List<Spell> activeSpells;
    private Stack<Spell> spellPool;

    private static SpellManager instance;
    public static SpellManager Instance => instance ??= new SpellManager();
    
    private SpellManager()
    {
        activeSpells = new List<Spell>();
        spellPool = new Stack<Spell>();
    }

    private static Dictionary<KeyboardKey, Func<Vector2, Vector2, Spell>> SpellCaster = new()
    {
        {
            SettingsManager.singleInstance.gameSettings.controls.fireball,
            (direction, position) => new Fireball(direction, position)
        },
        { SettingsManager.singleInstance.gameSettings.controls.Firebeam, (direction, position) => new FireBeam() },
    };

    public void update(Player player)
    {
        KeyboardKey key = (KeyboardKey)GetKeyPressed();
        if (SpellCaster.TryGetValue(key, out var func) && key != KeyboardKey.Null)
        {
            Vector2[] spellinfo = player.getSpellInfo();
            Spell spell = spellPool.Count > 0
                ? spellPool.Pop()
                : func(spellinfo[0], spellinfo[1]);
            spell.Reset(spellinfo[0], spellinfo[1]);
            activeSpells.Add(spell);
        }
        
        foreach (Spell spell in activeSpells) { spell.update(); }
        
        if (activeSpells.Count != 0)
        {
            ClearDeadSpells();
        }
    }

    private void ClearDeadSpells()
    {
        for (int i = activeSpells.Count - 1; i >= 0; i--)
        {
            Spell spell = activeSpells[i];
            if (spell.isAlive) continue;
            activeSpells.RemoveAt(i);
            spellPool.Push(spell);
        }
    }

    public void draw()
    {
        foreach (Spell spell in activeSpells)
            spell.draw();
    }

    public void Dispose()
    {
        activeSpells.Clear();
        spellPool.Clear();
    }
}