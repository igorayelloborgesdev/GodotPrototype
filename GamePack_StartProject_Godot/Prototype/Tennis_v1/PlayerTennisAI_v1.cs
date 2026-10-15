using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public partial class PlayerTennisAI_v1 : CharacterBody3D
    {
        public GameplayTennisModel_v1 gameplayTennisModel = null;
        public Node3D serveBallRef = null;
        public string serveBallRefName { get; set; } = "ServeBallRef";
        public float Speed = 18.0f;

        public double speedServe = 0.0;
        public double speedBackHand = 0.0;

        public Context contextMove;
        public Node3D ServeBallGoodMinRef = null;
        public string ServeBallGoodMinRefName { get; set; } = "ServeBallGoodMinRef";
        public Node3D ServeBallGoodMaxRef = null;
        public string ServeBallGoodMaxRefName { get; set; } = "ServeBallGoodMaxRef";

        public List<GamePackStartProjectGodot.Prototype.Tennis.Range> ranges = new List<GamePackStartProjectGodot.Prototype.Tennis.Range>
        {
            new GamePackStartProjectGodot.Prototype.Tennis.Range(0.0, 5.0, 0.0),
            new GamePackStartProjectGodot.Prototype.Tennis.Range(5.0, 20.0, 1.0),
            new GamePackStartProjectGodot.Prototype.Tennis.Range(20.0, 100.0, 2.0),
        };

        RandomNumberGenerator rng = new RandomNumberGenerator();

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
                (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP2 
                || gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2
                ))
            {                
                gameplayTennisModel.backHandPosition = GetServePosition().GlobalPosition;
                speedServe = DefineServeStartTime();
                speedBackHand = DefineServeStartTime();
            }

            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2)
            {                
                if (speedServe > 0.0)
                {                    
                    DefineServePlayerAIMove(delta);
                }
                else 
                {
                    MoveTo((gameplayTennisModel.ball as TennisBall_v1).bestHitPosition);
                }
                
            }
            else if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP1)
            {             
                if ((gameplayTennisModel.ball as TennisBall_v1).theBallWasHit
                    && gameplayTennisModel.playerTennisModel.serveEnumWait == PlayerTennisModel_v1.ServeEnumWait.Act)
                {                    
                    speedServe = DefineServeStartTime();
                    speedBackHand = DefineServeStartTime();

                    var positionBackHand = DefineBackhandPosition();
                    var positionBackHandFinal = GamePackStartProjectGodot.Prototype.Tennis.Range.GetRangeValue(positionBackHand, ranges);
                    var listPlayerPositionUnordered = ((gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.P1BallPlaceList[(int)positionBackHandFinal];
                    var listPlayerPosition =
                                 listPlayerPositionUnordered
                                 .OrderByDescending(node =>
                                 node.GlobalPosition.DistanceTo(((gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.player.GlobalPosition)).ToList();
                    ((gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.backHandPosition = listPlayerPosition[0].GlobalPosition;
                }
                else
                {
                    if (speedBackHand > 0.0)
                    {                 
                        DefineBackHandPlayerAIMove(delta);
                    }
                    else
                    {
                        MoveTo(new Vector3(gameplayTennisModel.P2BallPlaceList[1].GlobalPosition.X, this.GlobalPosition.Y, this.GlobalPosition.Z));
                    }
                }
            }
        }
        private void DefineServePlayerAIMove(double delta)
        {
            speedServe -= delta;
        }
        private void DefineBackHandPlayerAIMove(double delta)
        {
            speedBackHand -= delta;
        }
        private void MoveTo(Vector3 target)
        {
            target.Y = GlobalPosition.Y;

            Vector3 direction = target - GlobalPosition;

            if (direction.Length() < 0.2f)
            {
                Velocity = Vector3.Zero;
            }
            else
            {
                Velocity = direction.Normalized() * Speed;
            }

            MoveAndSlide();
        }
        public PlayerTennisAI_v1 GameplayTennisModelBuilder(GameplayTennisModel_v1 gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;
            return this;
        }
        private float DefineServeStartTime()
        {
            rng.Randomize();
            return rng.RandfRange(0.0f, 0.75f);
        }

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
                    return gameplayTennisModel.servePlayerLeftP3_1;
                else if (servePosition == 1)
                    return gameplayTennisModel.servePlayerLeftP1_1;
                return gameplayTennisModel.servePlayerLeftP2_1;
            }
            else
            {                                
                if (servePosition == 0)
                    return gameplayTennisModel.servePlayerRightP1_1;
                else if (servePosition == 1)
                    return gameplayTennisModel.servePlayerRightP3_1;
                return gameplayTennisModel.servePlayerRightP2_1;

            }
        }
        public float DefineBackhandPosition()
        {
            rng.Randomize();
            return rng.RandfRange(0.0f, 100.0f);
        }

        public void Reset()
        {
            gameplayTennisModel.gameEnum = (GameplayTennisModel_v1.GameEnum)2;
            gameplayTennisModel.playerTennisModel.direction = Vector3.Zero;
            gameplayTennisModel.playerTennisModel.serveEnum = (PlayerTennisModel_v1.ServeEnum)0;
            gameplayTennisModel.playerTennisModel.serveDirectionEnum = (PlayerTennisModel_v1.ServeDirectionEnum)0;
            gameplayTennisModel.playerTennisModel.serveEnumWait = PlayerTennisModel_v1.ServeEnumWait.Init;
        }

    }
}
