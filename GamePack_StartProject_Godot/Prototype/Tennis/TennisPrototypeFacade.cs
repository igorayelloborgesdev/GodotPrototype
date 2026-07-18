using GamePackStartProjectGodot.Scripts.Command;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis
{
    public class TennisPrototypeFacade
    {
        public void Init(Node control, GameplayTennisModel gameplayModel)
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
            (gameplayModel.player as PlayerTennis).GameplayTennisModelBuilder(gameplayModel);
            gameplayModel.player.Position = gameplayModel.isServeLeft ?
                new Vector3(gameplayModel.servePlayerLeft.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft.Position.Z) :
                new Vector3(gameplayModel.servePlayerRight.Position.X, gameplayModel.height, gameplayModel.servePlayerRight.Position.Z);

            control.AddChild(gameplayModel.playerAI);
            (gameplayModel.playerAI as PlayerTennisAI).GameplayTennisModelBuilder(gameplayModel);
            gameplayModel.playerAI.Position = gameplayModel.isServeLeft ?                 
                new Vector3(gameplayModel.servePlayerRight2.Position.X, gameplayModel.height, gameplayModel.servePlayerRight2.Position.Z) :
                new Vector3(gameplayModel.servePlayerLeft2.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft2.Position.Z);

            PackedScene ballPacked = ResourceLoader.Load<PackedScene>(gameplayModel.ballPathName);
            gameplayModel.ball = ballPacked.Instantiate<CharacterBody3D>();
            control.AddChild(gameplayModel.ball);
            (gameplayModel.ball as TennisBall).GameplayTennisModelBuilder(gameplayModel).SetServePointBuilder().SetCourtPointBuilder();



        }
        public void Update(double delta, GameplayTennisModel gameplayModel)
        {
            if (gameplayModel.isReset)
            {                
                gameplayModel.player.Position = gameplayModel.isServeLeft ?
                    new Vector3(gameplayModel.servePlayerLeft.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft.Position.Z) :
                    new Vector3(gameplayModel.servePlayerRight.Position.X, gameplayModel.height, gameplayModel.servePlayerRight.Position.Z);
                gameplayModel.playerAI.Position = gameplayModel.isServeLeft ?
                    new Vector3(gameplayModel.servePlayerRight2.Position.X, gameplayModel.height, gameplayModel.servePlayerRight2.Position.Z) :
                    new Vector3(gameplayModel.servePlayerLeft2.Position.X, gameplayModel.height, gameplayModel.servePlayerLeft2.Position.Z);
                (gameplayModel.player as PlayerTennis).Reset();
                (gameplayModel.ball as TennisBall).Reset();
                gameplayModel.isReset = false;
            }
        }
        public void PhysicsProcess(double delta, GameplayTennisModel gameplayModel)
        {
            
        }
    }
}
