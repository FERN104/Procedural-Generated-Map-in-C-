using System.Drawing;
using System.Numerics;

namespace Cs_raylib_test.MapLogic;

public struct Room
{
    public Vector2 gridPosition;
    public int shapeIndex;
    public int Diameter;
    public bool connected;

    public Room(float minPercentage, Rectangle segment)
    {
        shapeIndex = Random.Shared.Next(2); // Inclusive of Min, Exclusive of Max meaning 0 - 2 range if max is 3

        int margin = 3;
        
        int maxDi = Math.Min(segment.Width, segment.Height) - (margin * 2);
        int minDi = (int)(maxDi * minPercentage);
        
        Diameter = Random.Shared.Next(minDi, maxDi);
        gridPosition = new Vector2(
            
            Random.Shared.Next(segment.X + margin + Diameter / 2,
                segment.X + segment.Width - margin - Diameter / 2),
            
            Random.Shared.Next(segment.Y + margin + Diameter / 2,
                segment.Y + segment.Height - margin - Diameter / 2)
        );
    }
}


public partial class MapGrids
{
    private void GraphGeneration(float minPercentage, int gridCount, float roomChance)
    {
        List<Room> rooms = new List<Room>();
        List<Rectangle> segments = new List<Rectangle>();
        
        // Create Segments for the gridCount
        int segCols = (int)Math.Ceiling(Math.Sqrt(gridCount));
        int segRows = (int)Math.Ceiling((float) gridCount / segCols);
        
        int segWidth = cols / segCols;
        int segHeight = rows / segRows;

        for (int row = 0; row < segRows; row++)
        {
            for (int col = 0; col < segCols; col++)
            {
                int x = col * segWidth;
                int y = row * segHeight;
                
                Rectangle segment = new Rectangle(x, y, segWidth, segHeight);
                segments.Add(segment);
            }
        }

        for (int i = 0; i < segments.Count; i++)
        {
            if (roomChance < Random.Shared.NextSingle())
                continue;
            
            Room room = new Room(minPercentage, segments[i]);
            
            // Draw the room based on the shape index
            switch (room.shapeIndex)
            {
                case 0: SquareRoom(ref room); break;
                case 1: CircleRoom(ref room); break;
                case 2: TriangleRoom(ref room); break;
            }

            rooms.Add(room);
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            //Basic setup to feed each room into the next
            Vector2 a = rooms[i].gridPosition;

            Room closestRoom = rooms[0]; //Temporary declaration
            for (int o = 0; o < rooms.Count; o++)
            {
                Room current = rooms[o];
                Vector2 bPos = current.gridPosition;
                if (bPos == a || current.connected)
                    continue;

                if (Vector2.Distance(closestRoom.gridPosition, a) > Vector2.Distance(current.gridPosition, a))
                    closestRoom = current;
            }

            Vector2 b = closestRoom.gridPosition;
            
            //take half of the smallest room
            int corridorWidth = Math.Min(rooms[i].Diameter, closestRoom.Diameter) / 2;
            
            //CarveCorridorsDiag(a, b);
            carveCorridorL(a, b, corridorWidth);
        }
    }
    
    
    private void CarveCorridorsDiag(Vector2 a, Vector2 b)
    {
        // Heavily inspired by research on Bresenham's Line Algorithm
        // Efficient due to only moving by single integers each frame (Bit Shifting)
        // Removes the need for floating point inaccuracies and arithmetic
        // Computors are notoriously less precise at dealing with decimals than integers
        // Keeping everything as whole numbers is more precise and efficient
        
        
        int x0 = (int)a.X;
        int y0 = (int)a.Y;
        int x1 = (int)b.X;
        int y1 = (int)b.Y;
        
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        
        int moveX = x0 < x1 ? 1 : -1;
        int moveY = y0 < y1 ? 1 : -1;
        
        
        // err tracks deviation from an ideal straight line
        // This variable keeps track of whether the next step should be horizontal or vertical
        // Important for smooth shapes keeping a linear feel in a single line.

        int err = dx - dy; // Tracks the true mathematicaly closest next cell

        int lineThickness = 6;
        
        while (true) // Loop until break
        {
            if (x0 >= 0 && x0 < cols && y0 >= 0 && y0 < rows) // Carve the current cell with the safety check used in shapes to prevent out of bounds errors
            {
                // add thickness when carving the line so corridors are walkable
                int th = lineThickness / 2;

                for (int i = x0 - th; i < x0 + th; i++)
                {
                    for (int o = y0 - th; o < y0 + th; o++)
                    {
                        grid[i, o].Code = '.';
                        grid[i, o].Walkable = true;
                    }
                }
            }

            if (x0 == x1 && y0 == y1) // Reached the other point so stop
                break;
            
            int e2 = err * 2; // Doubling is done to keep everything in integer space removes floating point arithmetic
            
            
            // This part is weighing which side needs to move this frame via the formulas
            if (e2 > -dy)
            {
                err -= dy;
                x0 += moveX;
            }

            if (e2 < dx)
            {
                err += dx;
                y0 += moveY;
            }
            // this works because we check how for the line is drifting in each direction to decide what axis to move on
        }

    }

    public void carveCorridorL(Vector2 a, Vector2 b, int corridorWidth)
    {
        int aX =  (int)a.X;
        int aY =  (int)a.Y;
        int bX = (int)b.X;
        int bY =  (int)b.Y;
        
        //Choose the Bend Point
        int bendX = aX;
        int bendY = bY;
        
        //Starting X and Y
        int startX = Math.Min(bX, bendX);
        int startY = Math.Min(aY, bendY);
        
        //End Y and X
        int endX = Math.Max(bX, bendX);
        int endY = Math.Max(aY, bendY);
        
        //Setup Corridor starting points
        int vCorridorStart = Math.Clamp((bendY - corridorWidth / 2), 0, rows - 1);
        int hCorridorStart = Math.Clamp((bendX - corridorWidth / 2), 0, cols - 1);
        
        //Setup Corridor limits
        int vCorridorLimit = Math.Clamp((bendY + corridorWidth / 2), 0, rows - 1);
        int hCorridorLimit = Math.Clamp((bendX + corridorWidth / 2), 0, cols - 1);
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = vCorridorStart; y < vCorridorLimit; y++){
                grid[x, y].Code = '.';
                grid[x, y].Walkable = true;
            }
        }

        for (int y = startY; y <= endY; y++)
        {
            for (int x = hCorridorStart; x < hCorridorLimit; x++)
            {
                grid[x, y].Code = '.';
                grid[x, y].Walkable = true;
            }
        }
    }

    private void SquareRoom(ref Room room)
    {
        int sx = (int)room.gridPosition.X;
        int sy = (int)room.gridPosition.Y;
        int r = room.Diameter / 2;
        
        for (int x = sx - r; x < sx + r; x++)
        {
            for (int y = sy - r; y < sy + r; y++)
            {
                //Safety to prevent indexOutOfBounds crashes
                if (x < 0 || x >= cols || y < 0 || y >= rows)
                    continue;
                
                //Assign everything inside a floor
                grid[x, y].Code = '.';
                grid[x, y].Walkable = true;
            }
        }
    }

    private void CircleRoom(ref Room room)
    {
        int cx = (int)room.gridPosition.X;
        int cy = (int)room.gridPosition.Y;
        int r = room.Diameter / 2;

        for (int x = cx - r; x < cx + r; x++)
        {
            for (int y = cy - r; y < cy + r; y++)
            {
                //Safety to prevent indexOutOfBounds crashes
                if (x < 0 || x >= cols || y < 0 || y >= rows)
                    continue;
                
                // Filter out corner edges through comparing the distance to the radius
                int dx = x - cx;
                int dy = y - cy;
                
                // Pythagoras' theorem to check distance
                if (dx * dx + dy * dy <= r * r)
                {
                    grid[x, y].Code = '.';
                    grid[x, y].Walkable = true;
                }
            }
        }
    }

    private void TriangleRoom(ref Room room)
    {
        int tx = (int)room.gridPosition.X;
        int ty = (int)room.gridPosition.Y;
        
        int d = room.Diameter;
        int r = d / 2;
        
        int top = ty - r;
        int bottom = ty + r;
        
        for (int y = top + 1; y < bottom; y++)
        {
            int dy = y - top;
            int shrink = dy;

            int rowHalfWidth = r - shrink;
            
            if (rowHalfWidth < 0)
                continue;
            
            int left = tx - rowHalfWidth;
            int right = tx + rowHalfWidth;
            
            for (int x = left + 1; x < right; x++)
            {
                //Safety to prevent indexOutOfBounds crashes
                if (x < 0 || x >= cols || y < 0 || y >= rows)
                    continue;

                grid[x, y].Code = '.';
                grid[x, y].Walkable = true;
            }
        }
    }
}