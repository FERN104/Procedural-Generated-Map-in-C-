using System.Numerics;

namespace Cs_raylib_test.MapLogic;

public partial class MapGrids
{
    //Strategy 1
    private void GeneratorAlgorithm(float emptyChance)
    {
        // Stage 1 Generate using random chances
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (y == 0 || x == 0 || x == cols - 1 || y == rows - 1)
                {
                    grid[x, y].Walkable = false;
                    grid[x, y].Code = '#';
                    continue;
                }

                if (Random.Shared.NextDouble() <= emptyChance)
                {
                    grid[x, y].Walkable = true;
                    grid[x, y].Code = '.';
                }

                else
                {
                    grid[x, y].Walkable = false;
                    grid[x, y].Code = '#';
                }
            }
        }
        
        // Smooth the map multiple times
        for (int i = 0; i < 8; i++)
        {
            SmoothMap();
        }
    }

    private void SmoothMap()
    {
        // Stage 2 Check for surrounding cells to clear a path
        
        //Create an identical copy of the grid before adding smoothing changes
        GridCell[,] oldGrid = new GridCell[cols, rows];
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                oldGrid[x, y] = new GridCell(grid[x,y].Position)
                {
                    Walkable = grid[x, y].Walkable,
                    Code = grid[x, y].Code
                };
            }
        }
        
        //Don't waste time looping through the perimeter
        for (int x = 1; x < cols - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                // Count each surrounding wall of the cell
                int wallCount = 0;
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int o = y -1; o <= y +  1; o++)
                    {
                        if (oldGrid[i, o].Code == '#')
                            wallCount++;
                    }
                }
                
                if (oldGrid[x, y].Code == '#')
                    wallCount--;
                
                //Apply Changes to the real grid
                if (wallCount > 4)
                {
                    grid[x, y].Code = '#';
                    grid[x, y].Walkable = false;
                }
                
                else
                {
                    grid[x, y].Code = '.';
                    grid[x, y].Walkable = true;
                }
            }
        }
    }
}