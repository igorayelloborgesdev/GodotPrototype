using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using static GamePackStartProjectGodot.Prototype.Tennis.GameplayTennisModel;
using static GamePackStartProjectGodot.Prototype.Tennis.PlayerTennisModel;

public partial class PlayerTennisAI : CharacterBody3D
{
    public GameplayTennisModel gameplayTennisModel = null;
    public float Speed = 18.0f;
    public double speedServe = 0.0;
    public double speedBackHand = 0.0;
    RandomNumberGenerator rng = new RandomNumberGenerator();

    public Node3D serveBallRef = null;
    public string serveBallRefName { get; set; } = "ServeBallRef";
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
    public override void _Ready()
    {
        serveBallRef = this.GetNode<Node3D>(serveBallRefName);
        ServeBallGoodMinRef = this.GetNode<Node3D>(ServeBallGoodMinRefName);
        ServeBallGoodMaxRef = this.GetNode<Node3D>(ServeBallGoodMaxRefName);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (gameplayTennisModel is null)
            return;

        if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.HitBall)
        {
            speedServe = DefineServeStart();
            speedBackHand = DefineServeStart();
        }
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.InGameP2)
        {            
            if (speedServe > 0.0)
            {         
                DefineServePlayerAIMove(delta);
            }
            else 
            {                
                MoveTo((gameplayTennisModel.ball as TennisBall).bestHitPosition);                
            }            
        }
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.InGameP1)
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
    public PlayerTennisAI GameplayTennisModelBuilder(GameplayTennisModel gameplayTennisModel)
    {
        this.gameplayTennisModel = gameplayTennisModel;
        return this;
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
    private float DefineServeStart()
    {
        rng.Randomize();
        return rng.RandfRange(0.0f, 0.75f);
    }
    public float DefineBackhandPosition()
    {
        rng.Randomize();
        return rng.RandfRange(0.0f, 100.0f);
    }
    private void DefineServePlayerAIMove(double delta)
    {
        speedServe -= delta;
    }
    private void DefineBackHandPlayerAIMove(double delta)
    {
        speedBackHand -= delta;
    }

    public void Reset()
    {
        gameplayTennisModel.gameEnum = GameEnum.ServeP2;
        gameplayTennisModel.playerTennisModel.direction = Vector3.Zero;
        gameplayTennisModel.playerTennisModel.serveEnum = ServeEnum.Stand;
        gameplayTennisModel.playerTennisModel.serveDirectionEnum = ServeDirectionEnum.Center;
    }

    public int DefineServeP2()
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
}
