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
        if (!File.Exists(settingsFilePath)) // if the file path does not exist create a fresh gameSettings
        {
            gameSettings = new GameSettings();
            Console.WriteLine("\nCan't find settings file\n.\n.\n.\n.");
            return;
        }
        
        string json = File.ReadAllText(settingsFilePath); // Read the JSON file and load it into a string variable
        gameSettings = JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings(); // attempt to deserialise the json file. If it fails to do so then create a new GameSettings class
        Console.WriteLine("\nFound Json File"+settingsFilePath + "\n.\n.\n.\n.");
    }

    public void SaveSettings()
    {
        var options = new JsonSerializerOptions {WriteIndented = true}; // Make the serialisation options have line breaks and propper formatting.
                                                                        // Without this it would try to write to one line instead of formatting it nicely
        
        string json = JsonSerializer.Serialize(gameSettings, options);  // take the object we are serialising (gameSettings) and serialise it with the options we created above 
                                                                        // Writes to a string variable
        
        File.WriteAllText(settingsFilePath, json); // make the change to the file.
    }
}