using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis
{
    public class GameplayTennisModel
    {
        public string playerPathName { get; set; } = "res://Prototype/Tennis/player_tennis_prototype.tscn";

        public string playerIAPathName { get; set; } = "res://Prototype/Tennis/player_ia_tennis_prototype.tscn";
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


        public string ballPathName { get; set; } = "res://Prototype/Tennis/tennis_ball.tscn";


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


        public List<Node3D> P1CenterList = new List<Node3D>();
        public List<Node3D> P1LeftList = new List<Node3D>();
        public List<Node3D> P1RightList = new List<Node3D>();

        public List<Node3D> P1BallPlaceList = new List<Node3D>();

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


        public float height = 0.0130f;
        public CharacterBody3D ball;

        public PlayerTennisModel playerTennisModel = new PlayerTennisModel();

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
    }
}
