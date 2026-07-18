using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePackStartProjectGodot.Prototype.Tennis.PlayerTennisModel;
using static Godot.TextServer;

namespace GamePackStartProjectGodot.Prototype.Tennis
{    
    public abstract class TennisPlayerMoveStrategy
    {
        public GameplayTennisModel gameplayTennisModel = null;
        public abstract void MovePlayerAlgorithmInterface();
        public abstract void ActionPlayerAlgorithmInterface();
        public abstract void ResetAlgorithmInterface();

        public TennisPlayerMoveStrategy GameplayTennisModelBuilder(GameplayTennisModel gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;
            return this;
        }        
    }
    public class ConcreteStrategyMoveServe : TennisPlayerMoveStrategy
    {
        public override void MovePlayerAlgorithmInterface()
        {
            this.gameplayTennisModel.playerTennisModel.direction = Vector3.Zero;
            if (this.gameplayTennisModel.isServeLeft)
            {
                if (Input.IsKeyPressed(Key.Left) && this.gameplayTennisModel.player.Position.X > this.gameplayTennisModel.servePlayerLeftMin.Position.X)
                {
                    this.gameplayTennisModel.playerTennisModel.direction.X -= 1.0f;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Right) && this.gameplayTennisModel.player.Position.X < this.gameplayTennisModel.servePlayerLeftMax.Position.X)
                {
                    this.gameplayTennisModel.playerTennisModel.direction.X += 1.0f;
                    return;
                }
            }
            else
            {
                if (Input.IsKeyPressed(Key.Left) && this.gameplayTennisModel.player.Position.X > this.gameplayTennisModel.servePlayerRightMin.Position.X)
                {
                    this.gameplayTennisModel.playerTennisModel.direction.X -= 1.0f;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Right) && this.gameplayTennisModel.player.Position.X < this.gameplayTennisModel.servePlayerRightMax.Position.X)
                {
                    this.gameplayTennisModel.playerTennisModel.direction.X += 1.0f;
                    return;
                }
            }

        }
        public override void ActionPlayerAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.A))
            {
                this.gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel.ServeEnum.ThrowBall;
                return;
            }
        }
        public override void ResetAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.R))
            {
                if (!this.gameplayTennisModel.isReset)
                    this.gameplayTennisModel.isReset = true;
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                }
                return;
            }
        }
    }
    
    public class ConcreteStrategyServeDirection : TennisPlayerMoveStrategy
    {
        public override void MovePlayerAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.Left))
            {                
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = ServeDirectionEnum.Left;
                return;
            }
            else if (Input.IsKeyPressed(Key.Right))
            {             
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = ServeDirectionEnum.Right;
                return;
            }
            else if (!Input.IsKeyPressed(Key.Right) && !Input.IsKeyPressed(Key.Left))
            {             
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = ServeDirectionEnum.Center;
                return;
            }
        }
        public override void ActionPlayerAlgorithmInterface()
        {
            
        }
        public override void ResetAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.R))
            {
                if (!this.gameplayTennisModel.isReset)
                    this.gameplayTennisModel.isReset = true;
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                }
                return;
            }
        }
    }

    public class ConcreteStrategyMove : TennisPlayerMoveStrategy
    {
        public override void MovePlayerAlgorithmInterface()
        {
            
        }
        public override void ActionPlayerAlgorithmInterface()
        {

        }
        public override void ResetAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.R))
            {
                if (!this.gameplayTennisModel.isReset)
                    this.gameplayTennisModel.isReset = true;
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                }
                return;
            }
        }
    }

    public class Context
    {
        TennisPlayerMoveStrategy strategy;        
        public Context(TennisPlayerMoveStrategy strategy)
        {
            this.strategy = strategy;
        }
        public void ContextInterface()
        {
            strategy.MovePlayerAlgorithmInterface();
            strategy.ActionPlayerAlgorithmInterface();
            strategy.ResetAlgorithmInterface();
        }
    }
}
