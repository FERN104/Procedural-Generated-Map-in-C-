using System.Numerics;
using Cs_raylib_test.Entities;
using Cs_raylib_test.MapLogic;
using Cs_raylib_test.Physics;
using Cs_raylib_test.Settings;
using Raylib_cs;

namespace Cs_raylib_test.Spell;

public class SpellPools
{
    public Stack<Spell> fireball = new();
    public Stack<Spell> firebeam = new();
    
    public void ReturnToPool(Spell spell)
    {
        if (spell is Fireball) fireball.Push(spell);
        else if (spell is FireBeam) firebeam.Push(spell);
    }

    public void Clear()
    {
        fireball.Clear();
        firebeam.Clear();
    }
}

public class SpellManager
{
    private List<Spell> activeSpells;
    private SpellPools spellPools;

    private static SpellManager instance;
    public static SpellManager Instance => instance ??= new SpellManager();
    
    private SpellManager()
    {
        activeSpells = new List<Spell>();
        spellPools = new SpellPools();
    }

    private static Dictionary<KeyboardKey, Func<Entity, SpellPools, Spell?>> SpellCaster = new()
    {
        { SettingsManager.singleInstance.gameSettings.controls.fireball, (caster, spellPool) =>
            {
                if (caster.getSpellCooldowns().fireball.isReady())
                    
                    if (spellPool.fireball.Count > 0)
                        return spellPool.fireball.Pop();
                    else
                        return new Fireball(caster);
                return null;
            }
        },
        { SettingsManager.singleInstance.gameSettings.controls.Firebeam, (caster, spellPool) => new FireBeam(caster) },
    };

    public void update(Player player, MapGrids map, Camera2D camera)
    {
        KeyboardKey key;
        while ((key = (KeyboardKey)GetKeyPressed()) != KeyboardKey.Null)
        {
            if (SpellCaster.TryGetValue(key, out var spawn))
            {
                Spell? spell = spawn(player, spellPools);
                if (spell != null)
                {
                    Vector3 vectInfo = CollisionManager.instance.RotateToPoint(player, GetScreenToWorld2D(GetMousePosition(), camera));
                    player.targetPos = player.getGlobalPhysics().position; // Stop the player when a spell is cast
                    
                    // vect Info Contains
                    // .X = distance X from position to target
                    // .Y = distance Y from position to target
                    // .Z = magnitude / hypotenuse
                    
                    Vector2 dir = new Vector2(vectInfo.X / vectInfo.Z, vectInfo.Y / vectInfo.Z); // Vector Normalisation
                    
                    spell.Reset(dir, player.getGlobalPhysics().position);
                    activeSpells.Add(spell);
                }
            }
        }

        for (int i = activeSpells.Count - 1; i >= 0; i--)
        {
            Spell spell = activeSpells[i];
            spell.update(map);

            if (!spell.isAlive)
            {
                activeSpells.RemoveAt(i);
                spellPools.ReturnToPool(spell);
            }
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
        spellPools.Clear();
    }
}