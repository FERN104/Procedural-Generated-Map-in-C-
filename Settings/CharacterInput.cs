namespace Cs_raylib_test.Settings;
using System.Text.Json.Serialization;
using Raylib_cs;
public class CharacterInput
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MouseButton move { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public KeyboardKey fireball { get; set; }

    public CharacterInput()
    {
        move = MouseButton.Left;
        fireball = KeyboardKey.W;
    }

    public CharacterInput(MouseButton move, KeyboardKey fireball)
    {
        this.move = move;
        this.fireball = fireball;
    }

}