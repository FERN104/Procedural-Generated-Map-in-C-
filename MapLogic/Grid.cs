using System.Numerics;
using Cs_raylib_test.Entities;
using Rectangle = Raylib_cs.Rectangle;

namespace Cs_raylib_test.MapLogic;

public class GridCell
{
    public Vector2 Position;
    public bool Walkable;
    public char Code;
    public List<Entity> Current_Entities;
    public GridCell(Vector2 pos) {
        Position = pos;
        Walkable = true;
        Current_Entities = new List<Entity>();
        Code = '.';
    }
}

public partial class MapGrids
{
    public readonly int mapWidth;
    public readonly int mapHeight;
    
    public readonly float cellSize;
    private readonly int cols;
    private readonly int rows;
    
    private HashSet<GridCell> dirtyCells;
    private GridCell[,] grid;
    private List<MapObject> staticMap;
    
    //Map Gen Chances
    private float emptyChance = 0.45f;
    
    public MapGrids(int width, int height, float cellSize)
    {
        staticMap = new List<MapObject>();
        mapWidth = width;
        mapHeight = height;
        this.cellSize = cellSize;
        
        cols = (int)(mapWidth / cellSize);
        rows = (int)(mapHeight / cellSize);
        
        grid = new GridCell[cols, rows];
        
        for (int x = 0; x < cols; x++)
            for (int y = 0; y < rows; y++)
                 grid[x, y] = new GridCell(new Vector2(x * cellSize, y * cellSize));
        
        GeneratorAlgorithm(this.emptyChance);
        LoadMap();
        
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
        
        int startX = Math.Clamp((int)(minX/cellSize), 0, cols- 1);
        int startY = Math.Clamp((int)(minY / cellSize), 0, rows - 1);
        int endX = Math.Clamp((int)(maxX / cellSize), 0, cols - 1);
        int endY = Math.Clamp((int)(maxY/cellSize), 0, rows - 1);
        
        for (int x = startX; x <= endX; x++)
            for (int y = startY; y <= endY; y++)
                yield return grid[x, y];
    }


    public void UpdateCells(List<Entity> entities)
    {
        foreach (GridCell cell in dirtyCells)
        {
            cell.Current_Entities.Clear();
        }
        dirtyCells.Clear();

        foreach (Entity entity in entities)
        {
            Rectangle rect = entity.getRectangle();
            foreach (GridCell cell in GetCellsAtRect(rect))
            {
                cell.Current_Entities.Add(entity);
                dirtyCells.Add(cell);
            }
        }
    }

    public void Draw()
    {
        foreach (MapObject obj in staticMap)
        {
            obj.Draw();
        }
    }

    private void LoadMap()
    {
        foreach (GridCell cell in grid)
        {
            var obj = decodeSymbol[cell.Code](new Rectangle(cell.Position.X, cell.Position.Y, cellSize, cellSize));
            if (obj == null) continue;
            staticMap.Add(obj);
        }
    }

    private Dictionary<char, Func<Rectangle, MapObject?>> decodeSymbol = new() // Single character instead of string to save memory
    {
        {'#', (rect) => new Wall(rect)},
        {'*', (rect) => new Wall(rect)},
        {'.', (rect) => null},
    };
}