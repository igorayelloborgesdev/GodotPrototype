using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using static GamePackStartProjectGodot.Prototype.Tennis.GameplayTennisModel;
using static GamePackStartProjectGodot.Prototype.Tennis.PlayerTennisModel;

public partial class TennisBall : CharacterBody3D
{

    //Quadra de saibro: Bounce = 0.95f
    //Quadra dura: Bounce = 0.80f
    //Quadra de grama: Bounce = 0.65f
    //Quadra de sintetico (carpet): Bounce = 0.5f
    //Quadra de saibro: Speed = 1.0f
    //Quadra dura: Speed = 1.25f
    //Quadra de grama: Speed= 1.5f
    //Quadra de sintetico (carpet): Speed = 1.75f    
    
    private float BaseGravity = -9.8f;
    private const float BounceFactor = 0.95f;
    private const float Friction = 0.98f;
    private const float FrictionXZ = 1.0f;
    private const float alturaMaximaDesejada = 10.0f;
    private const float bestHitPointMin = 3.0f;
    private const float bestHitPointMax = 3.5f;

    

    private PackedScene boxScene = null;
    private List<TrajectoryPoint> trajectoryPoint = new();
    private List<Node3D> impactBoxes = new();
    private Node3D boxAI = null;
    //Reset
    private float Gravity = 0.0f;    
    private Vector3 velocity = Vector3.Zero;
    private float SpeedMultiplier = 1.0f;
    public GameplayTennisModel gameplayTennisModel = null;
    private bool tossStarted = false;
    public bool theBallWasHit = false;
    private enum BallDirectionEnum
    {
        neutral = 0,
        up = 1,
        down = 2
    }
    private BallDirectionEnum ballDirectionEnum = BallDirectionEnum.neutral;
    public Vector3 bestHitPosition { get; set; }
    
    public override void _Ready()
    {
        boxScene = GD.Load<PackedScene>("res://Prototype/Tennis/ImpactBox.tscn");
    }

    public override void _PhysicsProcess(double delta)
    {        

        if (gameplayTennisModel.playerTennisModel is null)
            return;

        BallStates();        

        if (gameplayTennisModel.playerTennisModel.serveEnum ==
            PlayerTennisModel.ServeEnum.ThrowBall)
        {
            velocity.Y += Gravity * (float)delta;
            velocity.X *= FrictionXZ;
            velocity.Z *= FrictionXZ;

            Velocity = velocity;
            MoveAndSlide();

            velocity = Velocity;

            if (IsOnFloor())
            {
                velocity.Y = -velocity.Y * BounceFactor;
                velocity.X *= Friction;
                velocity.Z *= Friction;
            }

            if (velocity.Y > 0)
            {
                ballDirectionEnum = BallDirectionEnum.up;
            }
            else if (velocity.Y < 0)
            {
                ballDirectionEnum = BallDirectionEnum.down;
            }

            if (ballDirectionEnum == BallDirectionEnum.down)
            {
                if (this.GlobalPosition.Y < (gameplayTennisModel.player as PlayerTennis).ServeBallGoodMaxRef.GlobalPosition.Y &&
                    this.GlobalPosition.Y > (gameplayTennisModel.player as PlayerTennis).ServeBallGoodMinRef.GlobalPosition.Y)
                {
                    gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel.ServeEnum.HitBall;
                }

            }

        }
        else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.HitBall)
        {
            var servePlayerRight = gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP1 ? 
                GetServePosition(gameplayTennisModel.playerTennisModel) : GetServePositionP2(gameplayTennisModel.playerTennisModel);
            CalcularVelocidadesParaAlvo(servePlayerRight.GlobalPosition.X, servePlayerRight.GlobalPosition.Z, alturaMaximaDesejada);
            trajectoryPoint = PredictTrajectory();

            TrajectoryPoint hit = GetBestHitPoint(bestHitPointMin, bestHitPointMax);
            bestHitPosition = hit.Position;

            boxAI = boxScene.Instantiate<Node3D>();

            GetParent().CallDeferred(
                Node.MethodName.AddChild,
                boxAI
            );

            boxAI.Position = hit.Position;

            gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel.ServeEnum.InGame;
            gameplayTennisModel.gameEnum = gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP1 ? GameplayTennisModel.GameEnum.InGameP2 : GameplayTennisModel.GameEnum.InGameP1;

            foreach (Vector3 impact in PredictBounces(2))
            {
                CreateImpactBox(impact);
            }
        }
        else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.InGame)
        {
            if (theBallWasHit)
            {                
                theBallWasHit = false;

                RemoveImpactBoxes();
                RemoveBoxAI();

                var backHandPlayer = gameplayTennisModel.backHandPosition;//<-
                CalcularVelocidadesParaAlvo(backHandPlayer.X, backHandPlayer.Z, alturaMaximaDesejada);
                trajectoryPoint = PredictTrajectory();

                TrajectoryPoint hit = GetBestHitPoint(bestHitPointMin, bestHitPointMax);

                if (hit != null)
                {                    
                    bestHitPosition = hit.Position;                                        
                }

                foreach (Vector3 impact in PredictBounces(2))
                {
                    CreateImpactBox(impact);
                }
            }

            velocity.Y += Gravity * (float)delta;
            velocity.X *= FrictionXZ;
            velocity.Z *= FrictionXZ;

            Velocity = velocity;
            MoveAndSlide();

            if (IsOnFloor())
            {
                velocity.Y = -velocity.Y * BounceFactor;

                velocity.X *= Friction;
                velocity.Z *= Friction;

                if (velocity.Length() < 0.1f * SpeedMultiplier)
                    velocity = Vector3.Zero;
            }
        }
  
    }

    public void SetP2Serve()
    {
        ballDirectionEnum = BallDirectionEnum.up;
        tossStarted = true;
        SpeedMultiplier = 1.75f;
        Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;
        Vector3 pos = GlobalPosition;
        float alvoX = pos.X;
        float alvoZ = pos.Z;
        float altura = pos.Y + 8.0f;
        CalcularVelocidadesParaAlvo(alvoX, alvoZ, altura);
    }

    private TrajectoryPoint GetBestHitPoint(float minHeight, float maxHeight)
    {
        foreach (var p in trajectoryPoint)
        {
            if (p.Velocity.Y > 0 &&
                p.Position.Y >= minHeight &&
                p.Position.Y <= maxHeight &&
                p.Position.Z < 0)
            {
                return p;
            }
        }

        return trajectoryPoint[trajectoryPoint.Count - 1];
    }
    private Node3D GetServePosition(PlayerTennisModel playerTennisModel)
    {
        if (gameplayTennisModel.isServeLeft)
        {
            if (playerTennisModel.serveDirectionEnum == ServeDirectionEnum.Left)
                return gameplayTennisModel.servePlayerRightP1;
            else if (playerTennisModel.serveDirectionEnum == ServeDirectionEnum.Right)
                return gameplayTennisModel.servePlayerRightP3;
            return gameplayTennisModel.servePlayerRightP2;
        }
        else
        {
            if (playerTennisModel.serveDirectionEnum == ServeDirectionEnum.Left)
                return gameplayTennisModel.servePlayerLeftP3;
            else if (playerTennisModel.serveDirectionEnum == ServeDirectionEnum.Right)
                return gameplayTennisModel.servePlayerLeftP1;
            return gameplayTennisModel.servePlayerLeftP2;
        }

    }

    private Node3D GetServePositionP2(PlayerTennisModel playerTennisModel)
    {
        var servePosition = (gameplayTennisModel.playerAI as PlayerTennisAI).DefineServeP2();
        if (gameplayTennisModel.isServeLeft)
        {
            if (servePosition == 0)
                return gameplayTennisModel.servePlayerLeftP3_1;
            else if (servePosition == 1)
                return gameplayTennisModel.servePlayerLeftP1_1;
            return gameplayTennisModel.servePlayerLeftP2_1;
        }
        else
        {
            if (servePosition == 0)
                return gameplayTennisModel.servePlayerRightP1_1;
            else if (servePosition == 1)
                return gameplayTennisModel.servePlayerRightP3_1;
            return gameplayTennisModel.servePlayerRightP2_1;
            
        }
    }

    public Vector3 PredictLandingPoint(float groundY = 0)
    {
        Vector3 pos = GlobalPosition;

        float a = Gravity * 0.5f;
        float b = velocity.Y;
        float c = pos.Y - groundY;

        float delta = b * b - 4 * a * c;

        if (delta < 0)
            return pos;

        float sqrt = Mathf.Sqrt(delta);

        float t1 = (-b + sqrt) / (2 * a);
        float t2 = (-b - sqrt) / (2 * a);

        float t = Mathf.Max(t1, t2);

        return new Vector3(
            pos.X + velocity.X * t,
            groundY,
            pos.Z + velocity.Z * t
        );
    }

    public List<Vector3> PredictBounces(int bounceCount)
    {
        List<Vector3> impacts = new();

        Vector3 pos = GlobalPosition;
        Vector3 vel = velocity;

        if (pos.Y < 0f)
        {
            pos.Y = 0f;
        }

        float delta = (float)GetPhysicsProcessDeltaTime();
        if (Mathf.IsZeroApprox(delta))
        {
            delta = 1f / 60f;
        }
        
        int maxIterations = 5000;
        int iterations = 0;

        while (impacts.Count < bounceCount && iterations < maxIterations)
        {
            iterations++;
            Vector3 previousPos = pos;

            vel.Y += Gravity * delta;
            vel.X *= FrictionXZ;
            vel.Z *= FrictionXZ;

            pos += vel * delta;

            if (previousPos.Y >= 0f && pos.Y <= 0f)
            {
                float denominator = previousPos.Y - pos.Y;
                float alpha = Mathf.IsZeroApprox(denominator) ? 0f : previousPos.Y / denominator;

                Vector3 impact = previousPos.Lerp(pos, alpha);
                impact.Y = 0f;

                impacts.Add(impact);
                
                pos = impact;
                vel.Y = -vel.Y * BounceFactor;
                vel.X *= Friction;
                vel.Z *= Friction;

                if (vel.Length() < 0.1f * SpeedMultiplier)
                    break;
            }
        }
     
        if (impacts.Count == 0)
        {
            GD.PrintErr($"[PredictBounces] Falhou! : {iterations}. Pos Inicial Y: {GlobalPosition.Y}. Vel Inicial Y: {velocity.Y}");
        }

        return impacts;
    }

    public List<TrajectoryPoint> PredictTrajectory(float simulationTime = 5f)
    {
        List<TrajectoryPoint> trajectory = new();

        Vector3 pos = GlobalPosition;
        Vector3 vel = velocity;

        float delta = 1f / 60f;
        float elapsed = 0;

        while (elapsed < simulationTime)
        {
            trajectory.Add(new TrajectoryPoint
            {
                Position = pos,
                Velocity = vel,
                Time = elapsed
            });

            vel.Y += Gravity * delta;
            vel.X *= FrictionXZ;
            vel.Z *= FrictionXZ;

            pos += vel * delta;

            if (pos.Y <= 0)
            {
                pos.Y = 0;

                vel.Y = -vel.Y * BounceFactor;
                vel.X *= Friction;
                vel.Z *= Friction;

                if (vel.Length() < 0.1f)
                    break;
            }

            elapsed += delta;
        }

        return trajectory;
    }

    private void CalcularVelocidadesParaAlvo(float targetX, float targetZ, float peakHeight)
    {
        float x0 = GlobalPosition.X;
        float y0 = GlobalPosition.Y;
        float z0 = GlobalPosition.Z;

        if (peakHeight < y0) peakHeight = y0 + 0.1f;
        velocity.Y = Mathf.Sqrt(-2f * Gravity * (peakHeight - y0));

        float a = 0.5f * Gravity;
        float b = velocity.Y;
        float c = y0;

        float delta = (b * b) - (4f * a * c);
        if (delta < 0f) return;

        float tTotal = (-b - Mathf.Sqrt(delta)) / (2f * a);

        if (tTotal > 0f)
        {
            float distanciaX = targetX - x0;
            float distanciaZ = targetZ - z0;

            velocity.X = (distanciaX / tTotal);
            velocity.Z = (distanciaZ / tTotal);
        }
    }

    public TennisBall GameplayTennisModelBuilder(GameplayTennisModel gameplayTennisModel)
    { 
        this.gameplayTennisModel = gameplayTennisModel;
        return this; 
    }

    public TennisBall SetServePointBuilder()
    {
        this.gameplayTennisModel.servePlayerRightP1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP1Name);
        this.gameplayTennisModel.servePlayerRightP2 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP2Name);
        this.gameplayTennisModel.servePlayerRightP3 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP3Name);

        this.gameplayTennisModel.servePlayerLeftP1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP1Name);
        this.gameplayTennisModel.servePlayerLeftP2 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP2Name);
        this.gameplayTennisModel.servePlayerLeftP3 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP3Name);

        this.gameplayTennisModel.servePlayerRightP1_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP1_1Name);
        this.gameplayTennisModel.servePlayerRightP2_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP2_1Name);
        this.gameplayTennisModel.servePlayerRightP3_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerRightP3_1Name);

        this.gameplayTennisModel.servePlayerLeftP1_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP1_1Name);
        this.gameplayTennisModel.servePlayerLeftP2_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP2_1Name);
        this.gameplayTennisModel.servePlayerLeftP3_1 = this.GetNode<Node3D>(this.gameplayTennisModel.servePlayerLeftP3_1Name);
        return this;
    }

    public TennisBall SetCourtPointBuilder()
    {
        
        var P1Front1 = this.GetNode<Node3D>(this.gameplayTennisModel.P1CenterName + (1).ToString());
        this.gameplayTennisModel.P1FrontList.Add(P1Front1);
        var P1Front2 = this.GetNode<Node3D>(this.gameplayTennisModel.P1LeftName + (1).ToString());
        this.gameplayTennisModel.P1FrontList.Add(P1Front2);
        var P1Front3 = this.GetNode<Node3D>(this.gameplayTennisModel.P1RightName + (1).ToString());
        this.gameplayTennisModel.P1FrontList.Add(P1Front3);

        var P1Center1 = this.GetNode<Node3D>(this.gameplayTennisModel.P1CenterName + (2).ToString());
        this.gameplayTennisModel.P1CenterList.Add(P1Center1);
        var P1Center2 = this.GetNode<Node3D>(this.gameplayTennisModel.P1LeftName + (2).ToString());
        this.gameplayTennisModel.P1CenterList.Add(P1Center2);
        var P1Center3 = this.GetNode<Node3D>(this.gameplayTennisModel.P1RightName + (2).ToString());
        this.gameplayTennisModel.P1CenterList.Add(P1Center3);

        var P1Back1 = this.GetNode<Node3D>(this.gameplayTennisModel.P1CenterName + (3).ToString());
        this.gameplayTennisModel.P1BackList.Add(P1Back1);
        var P1Back2 = this.GetNode<Node3D>(this.gameplayTennisModel.P1LeftName + (3).ToString());
        this.gameplayTennisModel.P1BackList.Add(P1Back2);
        var P1Back3 = this.GetNode<Node3D>(this.gameplayTennisModel.P1RightName + (3).ToString());
        this.gameplayTennisModel.P1BackList.Add(P1Back3);


        this.gameplayTennisModel.P1BallPlaceList.Add(this.gameplayTennisModel.P1FrontList);
        this.gameplayTennisModel.P1BallPlaceList.Add(this.gameplayTennisModel.P1CenterList);
        this.gameplayTennisModel.P1BallPlaceList.Add(this.gameplayTennisModel.P1BackList);
        

        var P2Front1 = this.GetNode<Node3D>(this.gameplayTennisModel.P2CenterName + (1).ToString());
        var P2Center1 = this.GetNode<Node3D>(this.gameplayTennisModel.P2CenterName + (2).ToString());
        var P2Back1 = this.GetNode<Node3D>(this.gameplayTennisModel.P2CenterName + (3).ToString());
        var P2Front3 = this.GetNode<Node3D>(this.gameplayTennisModel.P2RightName + (1).ToString());
        var P2Center3 = this.GetNode<Node3D>(this.gameplayTennisModel.P2RightName + (2).ToString());
        var P2Back3 = this.GetNode<Node3D>(this.gameplayTennisModel.P2RightName + (3).ToString());
        var P2Front2 = this.GetNode<Node3D>(this.gameplayTennisModel.P2LeftName + (1).ToString());        
        var P2Center2 = this.GetNode<Node3D>(this.gameplayTennisModel.P2LeftName + (2).ToString());        
        var P2Back2 = this.GetNode<Node3D>(this.gameplayTennisModel.P2LeftName + (3).ToString());

        this.gameplayTennisModel.P2BallPlaceList.Add(P2Front1);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Center1);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Back1);

        this.gameplayTennisModel.P2BallPlaceList.Add(P2Front3);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Center3);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Back3);

        this.gameplayTennisModel.P2BallPlaceList.Add(P2Front2);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Center2);
        this.gameplayTennisModel.P2BallPlaceList.Add(P2Back2);

        return this;
    }

    private void BallStates()
    {
        if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP1)
        {            
            if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.Stand)
            {                
                this.GlobalPosition = (gameplayTennisModel.player as PlayerTennis).serveBallRef.GlobalPosition;
            }                
            else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.ThrowBall)
            {
                if (!tossStarted)
                {                 
                    ballDirectionEnum = BallDirectionEnum.up;
                    tossStarted = true;
                    SpeedMultiplier = 1.75f;
                    Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;
                    Vector3 pos = GlobalPosition;                 
                    float alvoX = pos.X;
                    float alvoZ = pos.Z;
                    float altura = pos.Y + 8.0f;
                    CalcularVelocidadesParaAlvo(alvoX, alvoZ, altura);                    
                }
            }
        }
        else if (gameplayTennisModel.gameEnum == GameplayTennisModel.GameEnum.ServeP2)
        {
            
            if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.Stand)
            {
                this.GlobalPosition = (gameplayTennisModel.playerAI as PlayerTennisAI).serveBallRef.GlobalPosition;
                gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel.ServeEnum.ThrowBall;
            }                
            else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel.ServeEnum.ThrowBall)
            {             
                if (!tossStarted)
                {                    
                    ballDirectionEnum = BallDirectionEnum.up;
                    tossStarted = true;
                    SpeedMultiplier = 1.75f;
                    Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;
                    Vector3 pos = GlobalPosition;
                    float alvoX = pos.X;
                    float alvoZ = pos.Z;
                    float altura = pos.Y + 8.0f;
                    CalcularVelocidadesParaAlvo(alvoX, alvoZ, altura);
                }
            }
        }
    }

    public void Reset()
    {
        Gravity = 0.0f;
        velocity = Vector3.Zero;
        SpeedMultiplier = 1.0f;
        tossStarted = false;        
        ballDirectionEnum = BallDirectionEnum.neutral;
        gameplayTennisModel.gameEnum = GameEnum.ServeP1;
        ballDirectionEnum = BallDirectionEnum.neutral;
        theBallWasHit = false;
    }

    public void ResetServeP2()
    {
        Gravity = 0.0f;
        velocity = Vector3.Zero;
        SpeedMultiplier = 1.0f;
        tossStarted = false;
        ballDirectionEnum = BallDirectionEnum.neutral;
        gameplayTennisModel.gameEnum = GameEnum.ServeP2;
        ballDirectionEnum = BallDirectionEnum.neutral;
        theBallWasHit = false;
    }
    private void RemoveImpactBoxes()
    {
        foreach (Node3D box in impactBoxes)
        {
            if (IsInstanceValid(box))
                box.QueueFree();
        }

        impactBoxes.Clear();
    }
    private void CreateImpactBox(Vector3 position)
    {
        Node3D box = boxScene.Instantiate<Node3D>();

        GetParent().CallDeferred(Node.MethodName.AddChild, box);

        box.Position = new Vector3(position.X, 0.0f, position.Z);

        impactBoxes.Add(box);
    }
    private void RemoveBoxAI()
    {
        if (IsInstanceValid(boxAI))
        {
            boxAI.QueueFree();
            boxAI = null;
        }
    }
}
