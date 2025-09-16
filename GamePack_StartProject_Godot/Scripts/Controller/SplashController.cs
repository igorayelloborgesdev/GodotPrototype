using Godot;
using System;

public class SplashController
{
    #region Variable
    private SplashModel splashModel = new SplashModel();
    SplashFacade splashFacade = new SplashFacade();
    #endregion
    public void Init()
    {
        splashFacade.Init();
    }
    public bool TimerCheck(float delta)
    {
        splashModel.time = Timer(delta, splashModel.time);
        if (splashModel.time > splashModel.timeToProceedToMainMenu)
            return true;
        return false;
    }
    public float Timer(float delta, float time)
    {
        return time += delta;
    }
}
