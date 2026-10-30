using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public class TennisPrototypeFacade_v1
    {
        public void Init(Node control, GameplayTennisModel_v1 gameplayModel)
        {
            #region Test methods            

            #endregion

            Engine.TimeScale = 1.0f;

            gameplayModel.servePlayerLeft = control.GetNode<Node3D>(gameplayModel.servePlayerLeftName);
            gameplayModel.servePlayerLeftMax = control.GetNode<Node3D>(gameplayModel.servePlayerLeftMaxName);
            gameplayModel.servePlayerLeftMin = control.GetNode<Node3D>(gameplayModel.servePlayerLeftMinName);

            gameplayModel.servePlayerRight = control.GetNode<Node3D>(gameplayModel.servePlayerRightName);
            gameplayModel.servePlayerRightMax = control.GetNode<Node3D>(gameplayModel.servePlayerRightMaxName);
            gameplayModel.servePlayerRightMin = control.GetNode<Node3D>(gameplayModel.servePlayerRightMinName);

            gameplayModel.servePlayerLeft2 = control.GetNode<Node3D>(gameplayModel.servePlayerLeft2Name);
            gameplayModel.servePlayerLeftMax2 = control.GetNode<Node3D>(gameplayModel.servePlayerLeftMax2Name);
            gameplayModel.servePlayerLeftMin2 = control.GetNode<Node3D>(gameplayModel.servePlayerLeftMin2Name);

            gameplayModel.servePlayerRight2 = control.GetNode<Node3D>(gameplayModel.servePlayerRight2Name);
            gameplayModel.servePlayerRightMax2 = control.GetNode<Node3D>(gameplayModel.servePlayerRightMax2Name);
            gameplayModel.servePlayerRightMin2 = control.GetNode<Node3D>(gameplayModel.servePlayerRightMin2Name);

            PackedScene playerPacked = ResourceLoader.Load<PackedScene>(gameplayModel.playerPathName);
            gameplayModel.player = playerPacked.Instantiate<CharacterBody3D>();

            PackedScene playerPackedAI = ResourceLoader.Load<PackedScene>(gameplayModel.playerIAPathName);
            gameplayModel.playerAI = playerPackedAI.Instantiate<CharacterBody3D>();

            control.AddChild(gameplayModel.player);
            (gameplayModel.player as PlayerTennis_v1).GameplayTennisModelBuilder(gameplayModel);
            gameplayModel.player.Position = gameplayModel.isServeLeft ?
                new Vector3(gameplayModel.servePlayerLeft.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft.Position.Z) :
                new Vector3(gameplayModel.servePlayerRight.Position.X, gameplayModel.height, gameplayModel.servePlayerRight.Position.Z);

            control.AddChild(gameplayModel.playerAI);
            (gameplayModel.playerAI as PlayerTennisAI_v1).GameplayTennisModelBuilder(gameplayModel);
            gameplayModel.playerAI.Position = gameplayModel.isServeLeft ?
                new Vector3(gameplayModel.servePlayerRight2.Position.X, gameplayModel.height, gameplayModel.servePlayerRight2.Position.Z) :
                new Vector3(gameplayModel.servePlayerLeft2.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft2.Position.Z);

            PackedScene ballPacked = ResourceLoader.Load<PackedScene>(gameplayModel.ballPathName);
            gameplayModel.ball = ballPacked.Instantiate<CharacterBody3D>();
            control.AddChild(gameplayModel.ball);
            (gameplayModel.ball as TennisBall_v1).GameplayTennisModelBuilder(gameplayModel).SetServePointBuilder().SetCourtPointBuilder().SetBoxSceneBuilder();

            PackedScene ballTrailPath = ResourceLoader.Load<PackedScene>(gameplayModel.ballTrailPathName);
            for (int i = 0; i < 5; i++)
            {
                gameplayModel.ballTrailPathList.Add(ballTrailPath.Instantiate<Node3D>());
                gameplayModel.ballTrailPathList[i].Visible = false;
                control.AddChild(gameplayModel.ballTrailPathList[i]);
            }

            gameplayModel.canvasLayerSet1_P1 = control.GetNode<Label>(gameplayModel.canvasLayerSet1_P1Name);
            gameplayModel.canvasLayerSet1_P2 = control.GetNode<Label>(gameplayModel.canvasLayerSet1_P2Name);
            gameplayModel.canvasLayerSet2_P1 = control.GetNode<Label>(gameplayModel.canvasLayerSet2_P1Name);
            gameplayModel.canvasLayerSet2_P2 = control.GetNode<Label>(gameplayModel.canvasLayerSet2_P2Name);
            gameplayModel.canvasLayerSet3_P1 = control.GetNode<Label>(gameplayModel.canvasLayerSet3_P1Name);
            gameplayModel.canvasLayerSet3_P2 = control.GetNode<Label>(gameplayModel.canvasLayerSet3_P2Name);
            gameplayModel.canvasLayerGame_P1 = control.GetNode<Label>(gameplayModel.canvasLayerGame_P1Name);
            gameplayModel.canvasLayerGame_P2 = control.GetNode<Label>(gameplayModel.canvasLayerGame_P2Name);

            gameplayModel.canvasLayerSet_P1_List.Add(gameplayModel.canvasLayerSet1_P1);
            gameplayModel.canvasLayerSet_P1_List.Add(gameplayModel.canvasLayerSet2_P1);
            gameplayModel.canvasLayerSet_P1_List.Add(gameplayModel.canvasLayerSet3_P1);
            gameplayModel.canvasLayerSet_P2_List.Add(gameplayModel.canvasLayerSet1_P2);
            gameplayModel.canvasLayerSet_P2_List.Add(gameplayModel.canvasLayerSet2_P2);
            gameplayModel.canvasLayerSet_P2_List.Add(gameplayModel.canvasLayerSet3_P2);

            gameplayModel.canvasLayerSet1_P1.Text = "0".ToString();
            gameplayModel.canvasLayerSet1_P2.Text = "0".ToString();
            gameplayModel.canvasLayerSet2_P1.Text = "0".ToString();
            gameplayModel.canvasLayerSet2_P2.Text = "0".ToString();
            gameplayModel.canvasLayerSet3_P1.Text = "0".ToString();
            gameplayModel.canvasLayerSet3_P2.Text = "0".ToString();
            gameplayModel.canvasLayerGame_P1.Text = "0".ToString();
            gameplayModel.canvasLayerGame_P2.Text = "0".ToString();

            gameplayModel.p1_set_List.Add(0);
            gameplayModel.p1_set_List.Add(0);
            gameplayModel.p1_set_List.Add(0);

            gameplayModel.p2_set_List.Add(0);
            gameplayModel.p2_set_List.Add(0);
            gameplayModel.p2_set_List.Add(0);

        }
        public void Update(double delta, GameplayTennisModel_v1 gameplayModel)
        {            
            if(gameplayModel.isPlayFinished)
            {                
                if (gameplayModel.resetTime < gameplayModel.maxResetTime)
                    gameplayModel.resetTime += delta;
                else
                {                    
                    gameplayModel.isReset = true;
                    DefineScore(gameplayModel);
                    //gameplayModel.Reset();
                    gameplayModel.ResetGame();
                }                
            }
        }
        private void DefineScore(GameplayTennisModel_v1 gameplayModel)
        {
            if ((gameplayModel.ball as TennisBall_v1).ballBounceCourtEnum == TennisBall_v1.BallBounceCourtEnum.P1)
            {
                gameplayModel.scoreGameP2 += 1;                
            }
            else if ((gameplayModel.ball as TennisBall_v1).ballBounceCourtEnum == TennisBall_v1.BallBounceCourtEnum.P2)
            {
                gameplayModel.scoreGameP1 += 1;                
            }
            DefineGameEnding(gameplayModel);
        }
        private void DefineGameEnding(GameplayTennisModel_v1 gameplayModel)
        {
            if (!gameplayModel.isTieBreak)
            {
                if (gameplayModel.scoreGameP1 == 4 || gameplayModel.scoreGameP2 == 4)
                {
                    if (gameplayModel.scoreGameP1 == 4 && gameplayModel.scoreGameP2 == 4)
                    {
                        gameplayModel.scoreGameP1 = 3;
                        gameplayModel.scoreGameP2 = 3;
                    }
                    else if (Math.Abs(gameplayModel.scoreGameP1 - gameplayModel.scoreGameP2) > 1)
                    {
                        var scorePlayer = gameplayModel.scoreGameP1 > gameplayModel.scoreGameP2 ? 1 : 2;
                        DefinePlayerServe(gameplayModel);
                        gameplayModel.DefineGameScore(scorePlayer);
                        DefineSetEnding(gameplayModel);
                    }
                }
                else if (gameplayModel.scoreGameP1 == 5 || gameplayModel.scoreGameP2 == 5)
                {
                    var scorePlayer = gameplayModel.scoreGameP1 > gameplayModel.scoreGameP2 ? 1 : 2;
                    DefinePlayerServe(gameplayModel);
                    gameplayModel.DefineGameScore(scorePlayer);
                    DefineSetEnding(gameplayModel);
                }
                else if (gameplayModel.scoreSetP1 == 6 && gameplayModel.scoreSetP2 == 6)
                {
                    DefineSetEnding(gameplayModel);
                    DefinePlayerServe(gameplayModel);
                }

                DefinCountText(gameplayModel);
            }
            else 
            {
                DefinePlayerServe(gameplayModel);
                DefineTieBreakEnding(gameplayModel);
                DefinCountTextTieBreak(gameplayModel);                
            }
            
        }
        private void DefineSetEnding(GameplayTennisModel_v1 gameplayModel)
        {
            if (gameplayModel.scoreSetP1 == 6 && gameplayModel.scoreSetP2 == 6)
            {                
                gameplayModel.scoreGameP1 = 0;
                gameplayModel.scoreGameP2 = 0;
                gameplayModel.isTieBreak = true;
                DefinCountTextTieBreak(gameplayModel);
                DefinePlayerServe(gameplayModel);
            }
            else if ((gameplayModel.scoreSetP1 == 7 && gameplayModel.scoreSetP2 == 5) ||
                    (gameplayModel.scoreSetP1 == 5 && gameplayModel.scoreSetP2 == 7))
            {                
                DefinCountText(gameplayModel);                
                gameplayModel.ResetSet();
            }
            else if (gameplayModel.scoreSetP1 == 6 || gameplayModel.scoreSetP2 == 6)
            {
                if (Math.Abs(gameplayModel.scoreSetP1 - gameplayModel.scoreSetP2) > 1)
                {                    
                    DefinCountText(gameplayModel);
                    gameplayModel.ResetSet();
                }
            }
        }
        private void DefineTieBreakEnding(GameplayTennisModel_v1 gameplayModel)
        {
            if (gameplayModel.scoreGameP1 > 6 || gameplayModel.scoreGameP2 > 6)
            {
                if (Math.Abs(gameplayModel.scoreGameP1 - gameplayModel.scoreGameP2) > 1)
                {                    
                    gameplayModel.scoreSetP1 += gameplayModel.scoreGameP1 > gameplayModel.scoreGameP2 ? 1 : 0;
                    gameplayModel.scoreSetP2 += gameplayModel.scoreGameP2 > gameplayModel.scoreGameP1 ? 1 : 0;

                    gameplayModel.canvasLayerSet_P1_List[gameplayModel.gameSet].Text = gameplayModel.scoreSetP1.ToString();
                    gameplayModel.canvasLayerSet_P2_List[gameplayModel.gameSet].Text = gameplayModel.scoreSetP2.ToString();
                 
                    gameplayModel.ResetSet();
                }                
            }            
        }

        private void DefinePlayerServe(GameplayTennisModel_v1 gameplayModel)
        {
            if (!gameplayModel.isTieBreak)
            {
                gameplayModel.isServeP1 = gameplayModel.isServeP1 ? false : true;
                gameplayModel.gameEnum = gameplayModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 ?
                    GameplayTennisModel_v1.GameEnum.ServeP2 : GameplayTennisModel_v1.GameEnum.ServeP1;
            }
            else
            {
                if (gameplayModel.scoreGameP1 == 0 && gameplayModel.scoreGameP2 == 0)
                {
                    gameplayModel.isServeLeft = false;
                    gameplayModel.isServeP1 = !gameplayModel.isServeP1;
                    gameplayModel.gameEnum = gameplayModel.isServeP1 ?
                        GameplayTennisModel_v1.GameEnum.ServeP1 : GameplayTennisModel_v1.GameEnum.ServeP2;
                }
                else
                {                    
                    var isChange = (gameplayModel.scoreGameP1 + gameplayModel.scoreGameP2) % 2;
                    if (isChange == 1)
                    {
                        gameplayModel.isServeP1 = !gameplayModel.isServeP1;
                        gameplayModel.gameEnum = gameplayModel.isServeP1 ?
                            GameplayTennisModel_v1.GameEnum.ServeP1 : GameplayTennisModel_v1.GameEnum.ServeP2;
                    }

                }
                    
            }            
        }
        private void DefinCountText(GameplayTennisModel_v1 gameplayModel)
        {
            gameplayModel.canvasLayerGame_P1.Text = DefineCountText(gameplayModel, 1);
            gameplayModel.canvasLayerGame_P2.Text = DefineCountText(gameplayModel, 2);
            
            gameplayModel.canvasLayerSet_P1_List[gameplayModel.gameSet].Text = gameplayModel.scoreSetP1.ToString();
            gameplayModel.canvasLayerSet_P2_List[gameplayModel.gameSet].Text = gameplayModel.scoreSetP2.ToString();
        }

        private void DefinCountTextTieBreak(GameplayTennisModel_v1 gameplayModel)
        {
            gameplayModel.canvasLayerGame_P1.Text = gameplayModel.scoreGameP1.ToString();
            gameplayModel.canvasLayerGame_P2.Text = gameplayModel.scoreGameP2.ToString();
        }

        private string DefineCountText(GameplayTennisModel_v1 gameplayModel, int playerPos)
        {            
            if (playerPos == 1)
                return gameplayModel.scoreArray[gameplayModel.scoreGameP1];
            else
                return gameplayModel.scoreArray[gameplayModel.scoreGameP2];
        }

        public void PhysicsProcess(double delta, GameplayTennisModel_v1 gameplayModel)
        {

        }
    }
}
