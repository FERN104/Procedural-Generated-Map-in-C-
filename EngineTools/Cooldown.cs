using System.Diagnostics;

namespace Cs_raylib_test.Engine_Tools;

public class Cooldown
{
    private Stopwatch cooldown;
    private float cooldownTime;
    
    public Cooldown(float seconds) {
        cooldown = Stopwatch.StartNew();
        cooldownTime = seconds;
    }

    public void reset()
    {
        cooldown.Restart();
    }

    public bool isReady()
    {
        return cooldown.Elapsed.TotalSeconds >= cooldownTime;
    }
}