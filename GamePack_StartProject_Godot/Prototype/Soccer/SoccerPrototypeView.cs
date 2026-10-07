using GamePackStartProjectGodot.Prototype.Soccer;
using Godot;
using System;

public partial class SoccerPrototypeView : Node
{
    #region Variables
    private SoccerPrototypeController prototypeController = new SoccerPrototypeController();
    #endregion
    #region Behavior
    public override void _Ready()
    {
        prototypeController.Init(this);
    }
    public override void _Process(double delta)
    {
        prototypeController.Update(delta);
    }
    public override void _Input(InputEvent @event)
    {
        prototypeController.Input(@event);
    }
    #endregion
}
