using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public partial class TennisArea3D_V1 : Area3D
    {
        public string player_tennis_prototypeName { get; set; } = "../..";
        private PlayerTennis_v1 player_tennis_prototype = null;

        public override void _Ready()
        {
            player_tennis_prototype = this.GetNode<PlayerTennis_v1>(player_tennis_prototypeName);
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node3D body)
        {
            if (body is CharacterBody3D ball)
            {
                if (body.Name.ToString().Contains("ball", StringComparison.OrdinalIgnoreCase))
                {
                    ((player_tennis_prototype.gameplayTennisModel.ball) as TennisBall_v1).theBallWasHit = true;
                    //if (((player_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.InGameP1)
                    //{

                    //    ((player_tennis_prototype.gameplayTennisModel.ball) as TennisBall).theBallWasHit = true;
                    //    ((player_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.gameEnum = GameplayTennisModel.GameEnum.InGameP2;

                    //    player_tennis_prototype.gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel.ServeEnum.HitBall;

                    //    ((player_tennis_prototype.gameplayTennisModel.ball) as TennisBall).gameplayTennisModel.backHandPosition =
                    //    player_tennis_prototype.gameplayTennisModel.P2BallPlaceList[
                    //        (int)player_tennis_prototype.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum].GlobalPosition;
                    //}
                }
            }
        }
    }
}
