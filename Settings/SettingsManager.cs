using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cs_raylib_test.Settings;

public class SettingsManager
{
    public static SettingsManager singleInstance { get; private set; } = new SettingsManager();

    private SettingsManager()
    {
        settingsFilePath = "settings.json";
        LoadSettings();
        
    }
    private readonly  string settingsFilePath;
    
    public GameSettings gameSettings { get; set; } = new GameSettings();

    public void LoadSettings()
    {
        if (!File.Exists(settingsFilePath))
        {
            gameSettings = new GameSettings();
            Console.WriteLine("\nCan't find settings file\n.\n.\n.\n.");
            return;
        }
        
        string json = File.ReadAllText(settingsFilePath);
        gameSettings = JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
        Console.WriteLine("\nFound Json File"+settingsFilePath + "\n.\n.\n.\n.");
    }

    public void SaveSettings()
    {
        var options = new JsonSerializerOptions {WriteIndented = true};
        string json = JsonSerializer.Serialize(gameSettings, options);
        File.WriteAllText(settingsFilePath, json);
    }
}