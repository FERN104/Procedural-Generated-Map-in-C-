using System.Numerics;
using Cs_raylib_test.Entities;
using Raylib_cs;
namespace Cs_raylib_test.UI_Elements;

public class HUD_GLOBE
{
    private Vector2 screenPosition;
    private Color mainColor;
    private Color rimColor;
    private float percentage;
    private float radius;
    private float rimBuffer;
    private float currentVal;
    private float maxVal;
    private int fontSize;

    public HUD_GLOBE(Vector2 position, float radius, Color mainColor, Color rimColor)
    {
        percentage = 1;
        screenPosition = position;
        this.radius = radius;
        this.mainColor = mainColor;
        this.rimColor = rimColor;
        rimBuffer = radius/10;
        fontSize = (int)(radius / 3);
    }

    public void update(float currentVal, float maxVal)
    {
        this.currentVal = currentVal;
        this.maxVal = maxVal;
        percentage = currentVal / maxVal;
    }

    public void draw()
    {
        DrawCircle((int)(screenPosition.X + radius), (int)(screenPosition.Y + radius), radius + rimBuffer, rimColor);
        DrawCircle((int)(screenPosition.X + radius), (int)(screenPosition.Y + radius), radius, Color.Black);
        
        // Cut the circle by moving its drawable region down based on health percentage
        BeginScissorMode((int)screenPosition.X, (int)screenPosition.Y + (int)(radius*2 - (radius*2*percentage)), (int)(radius*2), (int)(radius*2 * percentage));
        DrawCircle((int)(screenPosition.X + radius), (int)(screenPosition.Y + radius), radius, mainColor);
        EndScissorMode();

        
        String s = currentVal.ToString() + "/" + maxVal.ToString();
        int length = MeasureText(s, fontSize);
        Vector2 txtPos = new Vector2((screenPosition.X + radius) - (float)length/2, screenPosition.Y + radius - (float)fontSize / 2);
        DrawText(s, (int)txtPos.X, (int)txtPos.Y, fontSize, Color.White);
    }
}