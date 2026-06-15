using System.Data;
using System.Numerics;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class Buttons
{
    private Rectangle rect;
    private readonly Color colour;
    private readonly Color dimColour;
    private readonly float dimFactor = 0.5f;
    private Color currentColour;

    private String text;
    private Color txtColor;
    private int fontSize;
    private int textPosx;
    private int textPosy;
    
    private bool isClicked = false;
    
    public bool getIsClicked() { return isClicked; }
    public bool setIsClicked(bool var) { isClicked = var; return isClicked; }

    public Buttons(String text, int x, int y, int width, int height, Color colour, int fontSize)
    {
        this.rect = new Rectangle(x, y, width, height);
        
        this.text = text;
        this.fontSize = fontSize; 
        this.textPosy = (int)(rect.Y + (rect.Height - fontSize) / 2);
        this.textPosx = (int)(rect.X + (rect.Width - MeasureText(text, fontSize)) / 2);
        
        this.colour = colour;
        this.dimColour = new Color(
            (byte)(colour.R * dimFactor),
            (byte)(colour.G * dimFactor),
            (byte)(colour.B * dimFactor),
            colour.A
        );
        this.txtColor = new Color(
            (byte)(255 - colour.R),
            (byte)(255 - colour.G),
            (byte)(255 - colour.B),
            colour.A
            );
        this.currentColour = colour;
    }
    
    public void update()
    {
        if (CheckCollisionPointRec(GetMousePosition(), rect))
        {
            currentColour = dimColour;
            if (IsMouseButtonPressed(0)) isClicked = true;
        }
        else
        {
            currentColour = colour;
        }
    }

    public void draw()
    {
        DrawRectangleRec(rect, currentColour);
        DrawText(text, textPosx, textPosy, fontSize, txtColor);
    }

}