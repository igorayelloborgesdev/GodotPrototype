using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;

public partial class PlayerTennisAI : CharacterBody3D
{
    public GameplayTennisModel gameplayTennisModel = null;
    public float Speed = 18.0f;
    public double speedServe = 0.0;
    RandomNumberGenerator rng = new RandomNumberGenerator();
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        if (gameplayTennisModel is null)
            return;

        if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.HitBall)
        {
            speedServe = DefineServeStart();            
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
    private void DefineServePlayerAIMove(double delta)
    {
        speedServe -= delta;
    }
}
