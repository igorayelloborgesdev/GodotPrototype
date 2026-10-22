using GamePackStartProjectGodot.Prototype.Tennis_v1;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis
{
    public abstract class TennisPlayerMoveStrategy_V1
    {
        public GameplayTennisModel_v1 gameplayTennisModel = null;
        public abstract void MovePlayerAlgorithmInterface();
        public abstract void ActionPlayerAlgorithmInterface();
        public abstract void ResetAlgorithmInterface();

        public float hipotenusa = 2.0f;
        public float cateto = 0.0f;

        public TennisPlayerMoveStrategy_V1 GameplayTennisModelBuilder(GameplayTennisModel_v1 gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;
            GameplayTennisMoveModelBuilder();
            return this;
        }
        private TennisPlayerMoveStrategy_V1 GameplayTennisMoveModelBuilder()
        {
            cateto = hipotenusa / Mathf.Sqrt(2.0f);
            return this;
        }
    }

    public class Context_v1
    {
        TennisPlayerMoveStrategy_V1 strategy;
        public Context_v1(TennisPlayerMoveStrategy_V1 strategy)
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

    public class ConcreteStrategyMoveServe_V1 : TennisPlayerMoveStrategy_V1
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
                this.gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel_v1.ServeEnum.ThrowBall;
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
            if (Input.IsKeyPressed(Key.Q))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.W))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
        }
    }

    public class ConcreteStrategyMove_V1 : TennisPlayerMoveStrategy_V1
    {
        public override void MovePlayerAlgorithmInterface()
        {

            if (Input.IsKeyPressed(Key.A))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X = 0.0f;
                this.gameplayTennisModel.playerTennisModel.direction.Z = 0.0f;

                this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.Center;
                if (Input.IsKeyPressed(Key.Left) && Input.IsKeyPressed(Key.Up))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.LeftUp;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Right) && Input.IsKeyPressed(Key.Up))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.RightUp;
                    return;
                }
                if (Input.IsKeyPressed(Key.Left) && Input.IsKeyPressed(Key.Down))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.LeftDown;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Right) && Input.IsKeyPressed(Key.Down))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.RightDown;
                    return;
                }

                if (Input.IsKeyPressed(Key.Left))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.Left;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Right))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.Right;
                    return;
                }
                if (Input.IsKeyPressed(Key.Up))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.CenterUp;
                    return;
                }
                else if (Input.IsKeyPressed(Key.Down))
                {
                    this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.CenterDown;
                    return;
                }
                return;
            }
            else
            {
                this.gameplayTennisModel.playerTennisModel.backForeHandDirectionEnum = PlayerTennisModel_v1.BackForeHandDirectionEnum.Center;
            }

            if (Input.IsKeyPressed(Key.Left) && Input.IsKeyPressed(Key.Up))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X -= cateto;
                this.gameplayTennisModel.playerTennisModel.direction.Z -= cateto;
                return;
            }
            else if (Input.IsKeyPressed(Key.Right) && Input.IsKeyPressed(Key.Up))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X += cateto;
                this.gameplayTennisModel.playerTennisModel.direction.Z -= cateto;
                return;
            }
            if (Input.IsKeyPressed(Key.Left) && Input.IsKeyPressed(Key.Down))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X -= cateto;
                this.gameplayTennisModel.playerTennisModel.direction.Z += cateto;
                return;
            }
            else if (Input.IsKeyPressed(Key.Right) && Input.IsKeyPressed(Key.Down))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X += cateto;
                this.gameplayTennisModel.playerTennisModel.direction.Z += cateto;
                return;
            }

            if (Input.IsKeyPressed(Key.Left))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X -= hipotenusa;
                return;
            }
            else if (Input.IsKeyPressed(Key.Right))
            {
                this.gameplayTennisModel.playerTennisModel.direction.X += hipotenusa;
                return;
            }
            if (Input.IsKeyPressed(Key.Up))
            {
                this.gameplayTennisModel.playerTennisModel.direction.Z -= hipotenusa;
                return;
            }
            else if (Input.IsKeyPressed(Key.Down))
            {
                this.gameplayTennisModel.playerTennisModel.direction.Z += hipotenusa;
                return;
            }

            this.gameplayTennisModel.playerTennisModel.direction.Z = 0.0f;
            this.gameplayTennisModel.playerTennisModel.direction.X = 0.0f;

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
            if (Input.IsKeyPressed(Key.Q))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.W))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
        }
    }

    public class ConcreteStrategyServeDirection_V1 : TennisPlayerMoveStrategy_V1
    {
        public override void MovePlayerAlgorithmInterface()
        {
            
            if (Input.IsKeyPressed(Key.Left))
            {                
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = PlayerTennisModel_v1.ServeDirectionEnum.Left;
                return;
            }
            else if (Input.IsKeyPressed(Key.Right))
            {             
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = PlayerTennisModel_v1.ServeDirectionEnum.Right;
                return;
            }
            else if (!Input.IsKeyPressed(Key.Right) && !Input.IsKeyPressed(Key.Left))
            {            
                this.gameplayTennisModel.playerTennisModel.serveDirectionEnum = PlayerTennisModel_v1.ServeDirectionEnum.Center;
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
            if (Input.IsKeyPressed(Key.Q))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.W))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
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

    public class ConcreteStrategyServeReceive_V1 : TennisPlayerMoveStrategy_V1
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

        }
        public override void ResetAlgorithmInterface()
        {
            if (Input.IsKeyPressed(Key.R))
            {
                if (!this.gameplayTennisModel.isReset)
                    this.gameplayTennisModel.isReset = true;
                return;
            }
            if (Input.IsKeyPressed(Key.Q))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.W))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = false;
                    this.gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = true;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                if (!this.gameplayTennisModel.isReset)
                {
                    this.gameplayTennisModel.isReset = true;
                    this.gameplayTennisModel.isServeLeft = false;
                    this.gameplayTennisModel.isServeP1 = true;
                }
                return;
            }
        }
    }
}
