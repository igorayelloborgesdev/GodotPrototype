using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public class GameplayTennisModel_v1
    {
        public string playerPathName { get; set; } = "res://Prototype/Tennis_v1/player_tennis_prototype_v1.tscn";

        public string playerIAPathName { get; set; } = "res://Prototype/Tennis_v1/player_ia_tennis_prototype_v1.tscn";
        public string servePlayerLeftName { get; set; } = "ServePlayerLeft";
        public string servePlayerLeftMaxName { get; set; } = "ServePlayerLeftMax";
        public string servePlayerLeftMinName { get; set; } = "ServePlayerLeftMin";

        public string servePlayerRightName { get; set; } = "ServePlayerRight";
        public string servePlayerRightMaxName { get; set; } = "ServePlayerRightMax";
        public string servePlayerRightMinName { get; set; } = "ServePlayerRightMin";

        public string servePlayerLeft2Name { get; set; } = "ServePlayerLeft2";
        public string servePlayerLeftMax2Name { get; set; } = "ServePlayerLeftMax2";
        public string servePlayerLeftMin2Name { get; set; } = "ServePlayerLeftMin2";

        public string servePlayerRight2Name { get; set; } = "ServePlayerRight2";
        public string servePlayerRightMax2Name { get; set; } = "ServePlayerRightMax2";
        public string servePlayerRightMin2Name { get; set; } = "ServePlayerRightMin2";

        public string P1CenterName { get; set; } = "../P1Center";
        public string P1LeftName { get; set; } = "../P1Left";
        public string P1RightName { get; set; } = "../P1Right";

        public string P2CenterName { get; set; } = "../P2Center";
        public string P2LeftName { get; set; } = "../P2Left";
        public string P2RightName { get; set; } = "../P2Right";

        public string ballTrailPathName { get; set; } = "res://Prototype/Tennis_v1/ball_trail.tscn";

        public string ballPathName { get; set; } = "res://Prototype/Tennis_v1/tennis_ball_v1.tscn";
        public Node3D ballTrailPathObj;
        public List<Node3D> ballTrailPathList = new List<Node3D>(5);

        public CharacterBody3D player;

        public CharacterBody3D playerAI;

        public Node3D servePlayerLeft;
        public Node3D servePlayerLeftMax;
        public Node3D servePlayerLeftMin;

        public Node3D servePlayerRight;
        public Node3D servePlayerRightMax;
        public Node3D servePlayerRightMin;

        public Node3D servePlayerLeft2;
        public Node3D servePlayerLeftMax2;
        public Node3D servePlayerLeftMin2;

        public Node3D servePlayerRight2;
        public Node3D servePlayerRightMax2;
        public Node3D servePlayerRightMin2;


        public List<Node3D> P1FrontList = new List<Node3D>();
        public List<Node3D> P1CenterList = new List<Node3D>();
        public List<Node3D> P1BackList = new List<Node3D>();

        public List<List<Node3D>> P1BallPlaceList = new List<List<Node3D>>();

        public List<Node3D> P2BallPlaceList = new List<Node3D>();

        public string servePlayerRightP1Name { get; set; } = "../ServePlayerRightP1";
        public Node3D servePlayerRightP1 = null;
        public string servePlayerRightP2Name { get; set; } = "../ServePlayerRightP2";
        public Node3D servePlayerRightP2 = null;
        public string servePlayerRightP3Name { get; set; } = "../ServePlayerRightP3";
        public Node3D servePlayerRightP3 = null;

        public string servePlayerLeftP1Name { get; set; } = "../ServePlayerLeftP1";
        public Node3D servePlayerLeftP1 = null;
        public string servePlayerLeftP2Name { get; set; } = "../ServePlayerLeftP2";
        public Node3D servePlayerLeftP2 = null;
        public string servePlayerLeftP3Name { get; set; } = "../ServePlayerLeftP3";
        public Node3D servePlayerLeftP3 = null;

        public string servePlayerRightP1_1Name { get; set; } = "../ServePlayerRightP1_1";
        public Node3D servePlayerRightP1_1 = null;
        public string servePlayerRightP2_1Name { get; set; } = "../ServePlayerRightP2_1";
        public Node3D servePlayerRightP2_1 = null;
        public string servePlayerRightP3_1Name { get; set; } = "../ServePlayerRightP3_1";
        public Node3D servePlayerRightP3_1 = null;

        public string servePlayerLeftP1_1Name { get; set; } = "../ServePlayerLeftP1_1";
        public Node3D servePlayerLeftP1_1 = null;
        public string servePlayerLeftP2_1Name { get; set; } = "../ServePlayerLeftP2_1";
        public Node3D servePlayerLeftP2_1 = null;
        public string servePlayerLeftP3_1Name { get; set; } = "../ServePlayerLeftP3_1";
        public Node3D servePlayerLeftP3_1 = null;

        public float height = 0.0130f;
        public CharacterBody3D ball;

        public PlayerTennisModel_v1 playerTennisModel = new PlayerTennisModel_v1();

        public GameEnum gameEnum = GameEnum.ServeP1;
        public enum GameEnum
        {
            ServeP1 = 1,
            ServeP2 = 2,
            InGameP2 = 3,
            InGameP1 = 4,
            Finish = 5
        }
        public bool isReset = false;

        public bool isServeLeft = true;

        public Vector3 backHandPosition = Vector3.Zero;

        public bool isServeP1 = true;

        public bool isPlayFinished = false;

        public string boxScene_Name { get; set; } = "res://Prototype/Tennis_v1/ImpactBox_v1.tscn";
        public PackedScene boxScene = null;

        public double maxResetTime = 0.75;
        public double resetTime = 0.0;

        public bool isSpecial = false;

        public void Reset()
        {
            if (isReset)
            {
                player.Position = isServeLeft ?
                    new Vector3(servePlayerLeft.Position.X, height, servePlayerLeft.Position.Z) :
                    new Vector3(servePlayerRight.Position.X, height, servePlayerRight.Position.Z);
                playerAI.Position = isServeLeft ?
                    new Vector3(servePlayerRight2.Position.X, height, servePlayerRight2.Position.Z) :
                    new Vector3(servePlayerLeft2.Position.X, height, servePlayerLeft2.Position.Z);
                
                if (isServeP1)
                {
                    (player as PlayerTennis_v1).Reset();
                    (ball as TennisBall_v1).Reset(1);
                }
                else
                {
                    (playerAI as PlayerTennisAI_v1).Reset();
                    (ball as TennisBall_v1).Reset(2);
                }
                isReset = false;
                isPlayFinished = false;
                resetTime = 0.0;
                isSpecial = false;
            }
        }


    }
}
