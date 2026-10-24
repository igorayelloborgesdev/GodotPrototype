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
                    gameplayModel.Reset();
                }                
            }
        }
        public void PhysicsProcess(double delta, GameplayTennisModel_v1 gameplayModel)
        {

        }
    }
}
