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
        Walkable = false;
        Current_Entities = new List<Entity>();
        Code = '#';
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
    
    //Choose Which strategy to load
    private int strategy = 2;
    
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

        switch (strategy)
        {
            case 1: GeneratorAlgorithm(0.45f); break;
            case 2: GraphGeneration(0.5f, 25, 0.6f); break;
        }
        
        LoadMap();
        
        dirtyCells = new HashSet<GridCell>();
    }

    public GridCell? GetCellAtPosition(Vector2 pos)
    {
        int x = (int)(pos.X / cellSize);
        int y = (int)(pos.Y / cellSize);
        
        if (x < 0 || y < 0 || x >= cols || y >= rows)
            return null;
        
        return grid[x, y];
    }

    public IEnumerable<GridCell> GetCellsAtRect(Rectangle rect)
    {
        float minX = rect.X;
        float minY = rect.Y;
        float maxX = rect.X + rect.Width;
        float maxY = rect.Y + rect.Height;
        
        //Math.Floor was an attempt to fix collision bugs (didn't work)
        int startX = Math.Clamp((int)(minX/cellSize), 0, cols- 1);
        int startY = Math.Clamp((int)(minY / cellSize), 0, rows - 1);
        int endX = Math.Clamp((int)(maxX / cellSize), 0, cols - 1);
        int endY = Math.Clamp((int)(maxY /cellSize), 0, rows - 1);
        
        
        for (int y = startY; y <= endY; y++)
            for (int x = startX; x <= endX; x++)    
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
    
    // Allows for random empty grids
    public Vector2 findEmptyGrid(Vector2 size, int startX, int startY)
    {
        int width =  (int)(size.X / cellSize);
        int height = (int)(size.Y / cellSize);

        width += 2; // buffer so the player does spawn against a wall
        height += 2;
        
        if (width > cols || height > rows)
            return Vector2.Zero;
        
        for (int x = startX; x <= cols - width; x++)
        {
            for (int y = startY; y <= rows - height; y++)
            {
                bool space = true;
                
                for (int ix = 0; ix < width; ix++)
                {
                    if (x + ix >= cols || x + ix < 0)
                    {
                        space = false;
                        break;
                    }
                    for (int iy = 0; iy < height; iy++)
                    {
                        if (y + iy >= rows || y + iy < 0)
                        {
                            space = false;
                            break;
                        }

                        if (!grid[x + ix, y + iy].Walkable)
                        {
                            space = false;
                            break;
                        }
                    }
                    if (!space) break;
                }
                if (space)
                    return new Vector2((x + width / 2f)* cellSize, (y + height /2f) * cellSize);
            }
        }
        return Vector2.Zero;
    }

    public void Draw()
    {
        foreach (MapObject obj in staticMap)
        {
            obj.Draw();
        }
    }

    public ref HashSet<GridCell> GetDirtyCells()
    {
        return ref dirtyCells;
    }

    public GridCell GetCellAt(int x, int y) { return grid[x, y];}
    
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