using Cs_raylib_test.MapLogic;

namespace Cs_raylib_test.Entities;

public class EnemySpawner
{
    MapGrids grid;
    List<Entity> entities;
    List<Enemy> enemies;
    int maxAmount;
    bool endlessMode;
    
    public EnemySpawner(int amount, bool endless, List<Entity> entities, MapGrids grid)
    {
        this.grid = grid;
        this.entities = entities;
        this.enemies = new List<Enemy>();
        this.maxAmount = amount;
        this.endlessMode = endless;
        
        SpawnAmount(amount);
    }

    public void update(Player player)
    {
        foreach (Enemy e in enemies)
        {
            e.enemyAI(player);
        }
        
        if (endlessMode)
            InfiniteMode();
        
        entities.RemoveAll(e => !e.Alive());
        enemies.RemoveAll(e => !e.Alive());
    }

    private void InfiniteMode()
    {
        int toSpawn = maxAmount - enemies.Count;
        
        if (toSpawn > 0)
            SpawnAmount(toSpawn);
    }

    private void SpawnAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy temp = new Enemy(grid);
            enemies.Add(temp);
            entities.Add(temp);
        }
    }

}