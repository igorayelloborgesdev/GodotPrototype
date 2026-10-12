using GamePackStartProjectGodot.Prototype.Tennis;
using GamePackStartProjectGodot.Prototype.Tennis_v1;
using Godot;
using System;

public partial class TennisPrototypeView_v1 : Node
{
    private TennisPrototypeController_v1 tennisPrototypeController = new TennisPrototypeController_v1();
    public override void _Ready()
    {
        tennisPrototypeController.Init(this);
    }
    public override void _Process(double delta)
    {
        tennisPrototypeController.Update(delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        tennisPrototypeController.PhysicsProcess(delta);
    }
}
