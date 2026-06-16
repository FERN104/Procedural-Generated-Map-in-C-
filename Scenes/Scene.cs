namespace Cs_raylib_test.Scenes;
public abstract class Scene : IDisposable
{
    public abstract SceneSwitch update();
    public abstract void draw();
    public abstract void Dispose();
}