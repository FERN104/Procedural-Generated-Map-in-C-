using Cs_raylib_test.Engine_Tools;
using Raylib_cs;

namespace Cs_raylib_test.UI_Elements;

public class TexturedButton
{
    private Rectangle rect;
    private Texture2D clickText;
    private Texture2D noClickText;
    private Texture2D currentTexture;
    private Color colour = Color.White;
    
    private bool isClicked = false;

    public bool getIsClicked() { return isClicked; }
    public bool setIsClicked(bool var) { isClicked = var; return isClicked; }

    public TexturedButton(String noClick, String click, Rectangle rect)
    {
        this.rect = rect;
        clickText = TextureManager.loadPathtoText(click, (int)rect.Width, (int)rect.Height);
        noClickText = TextureManager.loadPathtoText(noClick, (int)rect.Width, (int)rect.Height);
        currentTexture = noClickText;
    }
    
    public void update()
    {
        if (CheckCollisionPointRec(GetMousePosition(), rect))
        {
            currentTexture = clickText;
            if (IsMouseButtonPressed(0)) isClicked = true;
        }
        else
        {
            currentTexture = noClickText;
        }
    }

    public void draw()
    {
        DrawTexture(currentTexture, (int)rect.X, (int)rect.Y, colour);
    }
}