using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Linq;

public partial class TennisIAArea3D : Area3D
{
    public string player_IA_tennis_prototypeName { get; set; } = "../..";
    private PlayerTennisAI player_IA_tennis_prototype = null;

    public override void _Ready()
    {
        player_IA_tennis_prototype = this.GetNode<PlayerTennisAI>(player_IA_tennis_prototypeName);        
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body is CharacterBody3D ball)
        {
            if (body.Name.ToString().Contains("ball", StringComparison.OrdinalIgnoreCase))
            {
                ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).theBallWasHit = true;
                ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.gameEnum = GameplayTennisModel.GameEnum.InGameP1;

                var listPlayerPosition =
                    ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.P1BallPlaceList
                    .OrderByDescending(node => 
                    node.GlobalPosition.DistanceTo(((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.player.GlobalPosition)).ToList();

                ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.backHandPosition = listPlayerPosition[0].GlobalPosition;
            }
        }
    }
}
