using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using static GamePackStartProjectGodot.Prototype.Tennis.GameplayTennisModel;
using static GamePackStartProjectGodot.Prototype.Tennis.PlayerTennisModel;
using static Godot.TextServer;

public partial class PlayerTennis : CharacterBody3D
{
    public float Speed = 18.0f;
    public GameplayTennisModel gameplayTennisModel = null;
    public Context contextMove;
    public Node3D serveBallRef = null;
    public string serveBallRefName { get; set; } = "ServeBallRef";

    public Node3D ServeBallGoodMinRef = null;
    public string ServeBallGoodMinRefName { get; set; } = "ServeBallGoodMinRef";
    public Node3D ServeBallGoodMaxRef = null;
    public string ServeBallGoodMaxRefName { get; set; } = "ServeBallGoodMaxRef";

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

        contextMove = new Context(DefineTennisPlayerMoveStrategy());
        contextMove.ContextInterface();        


        this.gameplayTennisModel.playerTennisModel.direction = this.gameplayTennisModel.playerTennisModel.direction.Normalized();
        Velocity = this.gameplayTennisModel.playerTennisModel.direction * Speed;
        MoveAndSlide();
    }

    private TennisPlayerMoveStrategy DefineTennisPlayerMoveStrategy()
    {
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP1 && gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.Stand)
        {         
            return new ConcreteStrategyMoveServe().GameplayTennisModelBuilder(gameplayTennisModel);         
        }            
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP1 &&
            (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.ThrowBall ||
            gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.HitBall))
        {         
            return new ConcreteStrategyServeDirection().GameplayTennisModelBuilder(gameplayTennisModel);            
        }            
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP2)
        {            
            return new ConcreteStrategyServeReceive().GameplayTennisModelBuilder(gameplayTennisModel);
        }
        
        return new ConcreteStrategyMove().GameplayTennisModelBuilder(gameplayTennisModel);
    }

    public PlayerTennis GameplayTennisModelBuilder(GameplayTennisModel gameplayTennisModel)
    { 
        this.gameplayTennisModel = gameplayTennisModel;
        return this;
    }

    public void Reset()
    {
        gameplayTennisModel.gameEnum = GameEnum.ServeP1;
        gameplayTennisModel.playerTennisModel.direction = Vector3.Zero;
        gameplayTennisModel.playerTennisModel.serveEnum = ServeEnum.Stand;
        gameplayTennisModel.playerTennisModel.serveDirectionEnum = ServeDirectionEnum.Center;
    }
}
