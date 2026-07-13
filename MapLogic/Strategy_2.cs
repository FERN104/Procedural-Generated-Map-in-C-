using System.Drawing;
using System.Numerics;

namespace Cs_raylib_test.MapLogic;

public struct Room
{
    public Vector2 gridPosition;
    public int shapeIndex;
    public int Diameter;

    public Room(float minPercentage, Rectangle segment)
    {
        shapeIndex = Random.Shared.Next(3); // Inclusive of Min, Exclusive of Max meaning 0 - 2 range if max is 3

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
    private void GraphGeneration(float minPercentage, int roomAmount)
    {
        List<Room> rooms = new List<Room>();
        List<Rectangle> segments = new List<Rectangle>();
        
        // Create Segments to place each room in
        int segCols = (int)Math.Ceiling(Math.Sqrt(roomAmount));
        int segRows = (int)Math.Ceiling((float)roomAmount / segCols);
        
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

        for (int i = 0; i < roomAmount; i++)
        {
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