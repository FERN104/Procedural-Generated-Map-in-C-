using Raylib_cs;

namespace Cs_raylib_test.Engine_Tools;

public static class TextureManager
{
    private static Dictionary<string, Texture2D> cache = new();

    public static Texture2D loadPathtoText(String path, int width, int height)
    {
        String key = path;

        if (!cache.ContainsKey(key))
        {
            Image img = LoadImage(path);
            ImageResize(ref img, width, height);
            
            Texture2D tex = LoadTextureFromImage(img);
            UnloadImage(img);
            
            cache.Add(key, tex);
        }
        return cache[key];
    }

    public static void UnloadTextCache()
    {
        Console.WriteLine("[CACHE] Clearing Textures...");
        foreach (Texture2D text in cache.Values)
        {
            UnloadTexture(text);
        }
        cache.Clear();
        Console.WriteLine("[CACHE] Successfully Cleared Textures");
    }
}