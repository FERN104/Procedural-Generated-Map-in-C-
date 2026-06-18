using System.Text.Json.Serialization;
using Raylib_cs;

namespace Cs_raylib_test.Settings;

public class GameSettings
{
    public bool isFullscreen { get; set; }                                                                              //These two are stand alone
    public int MasterVolume { get; set; }                                                                             
    public CharacterInput controls { get; set; } =  new CharacterInput();                                               //This makes its own tab in the JSON file
}