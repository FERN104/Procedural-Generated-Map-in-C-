using System.Numerics;
using Cs_raylib_test.Entities;
using Rectangle = Raylib_cs.Rectangle;

namespace Cs_raylib_test.MapLogic;

public class GridCell
{
    public Vector2 position;
    public bool Walkable;
    public List<Entity> current_entities;
    public GridCell(Vector2 pos) {
        position = pos;
        Walkable = true;
        current_entities = new List<Entity>();
    }
}

public class MapGrids
{
    public readonly int mapWidth;
    public readonly int mapHeight;
    public readonly float cellSize;
    HashSet<GridCell> dirtyCells;
    private GridCell[,] grid;

    public MapGrids(int width, int height, float cellSize)
    {
        mapWidth = width;
        mapHeight = height;
        this.cellSize = cellSize;
        grid = new GridCell[mapWidth, mapHeight];
        
        for (int x = 0; x < mapWidth; x++)
            for (int y = 0; y < mapHeight; y++)
                 grid[x, y] = new GridCell(new Vector2(x * cellSize, y * cellSize));
        dirtyCells = new HashSet<GridCell>();
    }

    public GridCell GetCellAtPosition(Vector2 pos)
    {
        int x = (int)(pos.X / cellSize);
        int y = (int)(pos.Y / cellSize);
        
        if (x < 0 || y < 0 || x>= mapWidth || y >= mapHeight)
            return null;
        
        return grid[x, y];
    }

    public IEnumerable<GridCell> GetCellsAtRect(Rectangle rect)
    {
        float minX = rect.X;
        float minY = rect.Y;
        float maxX = rect.X + rect.Width-1;
        float maxY = rect.Y + rect.Height-1;
        
        int startX = Math.Clamp((int)(minX/cellSize), 0, mapWidth - 1);
        int startY = Math.Clamp((int)(minY / cellSize), 0, mapHeight - 1);
        int endX = Math.Clamp((int)(maxX / cellSize), 0, mapWidth - 1);
        int endY = Math.Clamp((int)(maxY/cellSize), 0, mapHeight - 1);
        
        for (int x = startX; x <= endX; x++)
            for (int y = startY; y <= endY; y++)
                yield return grid[x, y];
    }


    public void UpdateCells(List<Entity> entities)
    {
        foreach (GridCell cell in dirtyCells)
        {
            cell.current_entities.Clear();
        }
        dirtyCells.Clear();

        foreach (Entity entity in entities)
        {
            Rectangle rect = entity.getRectangle();
            foreach (GridCell cell in GetCellsAtRect(rect))
            {
                cell.current_entities.Add(entity);
                dirtyCells.Add(cell);
            }
        }
    }
}