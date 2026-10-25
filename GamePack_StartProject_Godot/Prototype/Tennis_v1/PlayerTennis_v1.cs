using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public double speedServe = 0.0;
        public double speedBackHand = 0.0;

        public float servePower = 0.2f;
        public float BackhandForehandPower = 0.2f;

        public Context_v1 contextMove;        
        public Node3D ServeBallGoodMinRef = null;
        public string ServeBallGoodMinRefName { get; set; } = "ServeBallGoodMinRef";
        public Node3D ServeBallGoodMaxRef = null;
        public string ServeBallGoodMaxRefName { get; set; } = "ServeBallGoodMaxRef";

        RandomNumberGenerator rng = new RandomNumberGenerator();

        public List<GamePackStartProjectGodot.Prototype.Tennis.Range> ranges = new List<GamePackStartProjectGodot.Prototype.Tennis.Range>
        {
            new GamePackStartProjectGodot.Prototype.Tennis.Range(0.0, 5.0, 0.0),
            new GamePackStartProjectGodot.Prototype.Tennis.Range(5.0, 20.0, 1.0),
            new GamePackStartProjectGodot.Prototype.Tennis.Range(20.0, 100.0, 2.0),
        };

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

            if(this.gameplayTennisModel.isPlayFinished)
                return;

            contextMove = new Context_v1(DefineTennisPlayerMoveStrategy());
            contextMove.ContextInterface();

            this.gameplayTennisModel.playerTennisModel.direction = this.gameplayTennisModel.playerTennisModel.direction.Normalized();
            Velocity = this.gameplayTennisModel.playerTennisModel.direction * Speed;            

            if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.HitBall && 
                (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 
                || gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP1
                ))
            {                
                gameplayTennisModel.backHandPosition = GetServePosition(this.gameplayTennisModel.playerTennisModel).GlobalPosition;
            
            }
            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2)
            {
                
                if ((gameplayTennisModel.ball as TennisBall_v1).theBallWasHit
                    && gameplayTennisModel.playerTennisModel.serveEnumWait == PlayerTennisModel_v1.ServeEnumWait.Act)
                {            
                    ((gameplayTennisModel.ball) as TennisBall_v1).gameplayTennisModel.backHandPosition =
                        ((gameplayTennisModel.ball) as TennisBall_v1)
                        .gameplayTennisModel.P2BallPlaceList[(int)this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum].GlobalPosition;
                }                
            }

            if (!((gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.ThrowBall) &&
                gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1))
                MoveAndSlide();
        }

        private Node3D GetServePosition(PlayerTennisModel_v1 playerTennisModel)
        {
            if (gameplayTennisModel.isServeLeft)
            {
                if (playerTennisModel.serveDirectionEnum == PlayerTennisModel_v1.ServeDirectionEnum.Left)
                    return gameplayTennisModel.servePlayerRightP1;
                else if (playerTennisModel.serveDirectionEnum == PlayerTennisModel_v1.ServeDirectionEnum.Right)
                    return gameplayTennisModel.servePlayerRightP3;
                return gameplayTennisModel.servePlayerRightP2;
            }
            else
            {
                if (playerTennisModel.serveDirectionEnum == PlayerTennisModel_v1.ServeDirectionEnum.Left)
                    return gameplayTennisModel.servePlayerLeftP3;
                else if (playerTennisModel.serveDirectionEnum == PlayerTennisModel_v1.ServeDirectionEnum.Right)
                    return gameplayTennisModel.servePlayerLeftP1;
                return gameplayTennisModel.servePlayerLeftP2;
            }

        }

        private TennisPlayerMoveStrategy_V1 DefineTennisPlayerMoveStrategy()
        {
            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 && gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.Stand)
            {
                return new ConcreteStrategyMoveServe_V1().GameplayTennisModelBuilder(gameplayTennisModel);
            }
            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 &&
                (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.ThrowBall ||
                gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.HitBall))
            {
                return new ConcreteStrategyServeDirection_V1().GameplayTennisModelBuilder(gameplayTennisModel);
            }
            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP2)
            {
                return new ConcreteStrategyServeReceive_V1().GameplayTennisModelBuilder(gameplayTennisModel);
            }

            return new ConcreteStrategyMove_V1().GameplayTennisModelBuilder(gameplayTennisModel);
        }
        private float DefineServeStartTime()
        {
            rng.Randomize();
            return rng.RandfRange(0.0f, 0.75f);
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
        public float DefineBackhandPosition()
        {
            rng.Randomize();
            return rng.RandfRange(0.0f, 100.0f);
        }
        public PlayerTennis_v1 GameplayTennisModelBuilder(GameplayTennisModel_v1 gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;
            return this;
        }

        public void Reset()
        {
            gameplayTennisModel.gameEnum = (GameplayTennisModel_v1.GameEnum)1;
            gameplayTennisModel.playerTennisModel.direction = Vector3.Zero;
            gameplayTennisModel.playerTennisModel.serveEnum = (PlayerTennisModel_v1.ServeEnum)0;
            gameplayTennisModel.playerTennisModel.serveDirectionEnum = (PlayerTennisModel_v1.ServeDirectionEnum)0;
            gameplayTennisModel.playerTennisModel.serveEnumWait = PlayerTennisModel_v1.ServeEnumWait.Init;
        }
    }
}
