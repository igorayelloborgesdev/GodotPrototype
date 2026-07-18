using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;

public partial class TennisPrototypeView : Node
{
    private TennisPrototypeController tennisPrototypeController = new TennisPrototypeController();
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
