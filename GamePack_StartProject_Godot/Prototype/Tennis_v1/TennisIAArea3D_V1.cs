using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public partial class TennisIAArea3D_V1 : Area3D
    {
        public string player_IA_tennis_prototypeName { get; set; } = "../..";
        private PlayerTennisAI_v1 player_IA_tennis_prototype = null;

        public override void _Ready()
        {
            player_IA_tennis_prototype = this.GetNode<PlayerTennisAI_v1>(player_IA_tennis_prototypeName);
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node3D body)
        {
            if (body is CharacterBody3D ball)
            {
                if (body.Name.ToString().Contains("ball", StringComparison.OrdinalIgnoreCase))
                {                    
                    ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall_v1).theBallWasHit = true;
                    if (((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2)
                        ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.InGameP1;
                    //if (((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.InGame)
                    //    ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.gameEnum = GameplayTennisModel.GameEnum.InGameP1;

                    //var positionBackHand = player_IA_tennis_prototype.DefineBackhandPosition();
                    //var positionBackHandFinal = GamePackStartProjectGodot.Prototype.Tennis.Range.GetRangeValue(positionBackHand, player_IA_tennis_prototype.ranges);

                    //var listPlayerPositionUnordered = ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.P1BallPlaceList[(int)positionBackHandFinal];

                    //var listPlayerPosition =
                    //    listPlayerPositionUnordered
                    //    .OrderByDescending(node =>
                    //    node.GlobalPosition.DistanceTo(((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.player.GlobalPosition)).ToList();

                    //if (((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.InGame)
                    //    ((player_IA_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.backHandPosition = listPlayerPosition[0].GlobalPosition;
                }
            }
        }
    }
}
