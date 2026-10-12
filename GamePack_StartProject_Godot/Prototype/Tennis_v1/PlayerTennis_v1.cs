using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public partial class PlayerTennis_v1 : CharacterBody3D
    {
        public GameplayTennisModel_v1 gameplayTennisModel = null;
        public Node3D serveBallRef = null;
        public string serveBallRefName { get; set; } = "ServeBallRef";
        public float Speed = 18.0f;        
        public Context contextMove;        
        public Node3D ServeBallGoodMinRef = null;
        public string ServeBallGoodMinRefName { get; set; } = "ServeBallGoodMinRef";
        public Node3D ServeBallGoodMaxRef = null;
        public string ServeBallGoodMaxRefName { get; set; } = "ServeBallGoodMaxRef";




        RandomNumberGenerator rng = new RandomNumberGenerator();

        private int DefineServeStart()
        {
            rng.Randomize();
            var rangeServe = rng.RandfRange(0.0f, 100.0f);
            if (rangeServe < 45.0f && rangeServe >= 0.0f)
                return 0;
            else if (rangeServe < 90.0f && rangeServe >= 45.0f)
                return 1;
            else
                return 0;
        }
        private Node3D GetServePosition()
        {            
            var servePosition = DefineServeStart();
            if (gameplayTennisModel.isServeLeft)
            {
                if (servePosition == 0)
                    return gameplayTennisModel.servePlayerRightP1;
                else if (servePosition == 1)
                    return gameplayTennisModel.servePlayerRightP3;
                return gameplayTennisModel.servePlayerRightP2;
            }
            else
            {
                if (servePosition == 0)
                    return gameplayTennisModel.servePlayerLeftP3;
                else if (servePosition == 1)
                    return gameplayTennisModel.servePlayerLeftP1;
                return gameplayTennisModel.servePlayerLeftP2;                
            }
        }

        public override void _Ready()
        {
            serveBallRef = this.GetNode<Node3D>(serveBallRefName);
            ServeBallGoodMinRef = this.GetNode<Node3D>(ServeBallGoodMinRefName);
            ServeBallGoodMaxRef = this.GetNode<Node3D>(ServeBallGoodMaxRefName);
        }
        public override void _PhysicsProcess(double delta)
        {

            if (this.gameplayTennisModel is null)
                return;

            if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.HitBall && 
                (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 || gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP1))
            {
                GD.Print("A");                
                gameplayTennisModel.backHandPosition = GetServePosition().GlobalPosition;
            }
        }
        public PlayerTennis_v1 GameplayTennisModelBuilder(GameplayTennisModel_v1 gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;
            return this;
        }
    }
}
