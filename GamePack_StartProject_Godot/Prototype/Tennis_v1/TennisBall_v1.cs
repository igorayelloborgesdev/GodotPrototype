using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public partial class TennisBall_v1 : CharacterBody3D
    {


        //Quadra de saibro: Bounce = 0.95f
        //Quadra dura: Bounce = 0.80f
        //Quadra de grama: Bounce = 0.65f
        //Quadra de sintetico (carpet): Bounce = 0.5f
        //Quadra de saibro: Speed = 1.5f
        //Quadra dura: Speed = 1.7f
        //Quadra de grama: Speed= 1.9f
        //Quadra de sintetico (carpet): Speed = 2.1f    

        //Quadra de saibro: Speed = 1.5f
        //Quadra dura: Speed = 1.7f
        //Quadra de grama: Speed= 1.9f
        //Quadra de sintetico (carpet): Speed = 2.1f    

        public GameplayTennisModel_v1 gameplayTennisModel = null;
        private float SpeedMultiplierInit = 1.5f;
        private float SpeedMultiplier = 1.5f;
        private const float BounceFactor = 0.95f;

        private float BaseGravity = -9.8f;        
        private const float Friction = 0.98f;
        private const float FrictionXZ = 1.0f;
        private const float alturaMaximaDesejada = 10.0f;
        private const float bestHitPointMin = 5.0f;
        private const float bestHitPointMax = 6.5f;

        private List<TrajectoryPoint> trajectoryPoint = new();
        public Vector3 bestHitPosition { get; set; }
        private PackedScene boxScene = null;
        private List<Node3D> impactBoxes = new();

        #region Reset
        private bool tossStarted = false;
        private enum BallDirectionEnum
        {
            neutral = 0,
            up = 1,
            down = 2
        }
        private BallDirectionEnum ballDirectionEnum = BallDirectionEnum.neutral;
        private float Gravity = 0.0f;
        private Vector3 velocity = Vector3.Zero;
        public bool theBallWasHit = false;

        public enum BallBounceCourtEnum
        {
            neutral = 0,
            P1 = 1,
            P2 = 2
        }
        public BallBounceCourtEnum ballBounceCourtEnum = BallBounceCourtEnum.neutral;

        public GameplayTennisModel_v1.GameEnum gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
        #endregion
        public bool isfirstServe = true;

        public override void _Ready()
        {
            boxScene = GD.Load<PackedScene>("res://Prototype/Tennis/ImpactBox.tscn");
            for (int i = 0; i < 2; i++)
                CreateImpactBox(Vector3.Zero);           
        }
        public override void _PhysicsProcess(double delta)
        {
            if (gameplayTennisModel.playerTennisModel is null)
                return;
            BallStates(delta);

            //TESTE input
            if (Input.IsKeyPressed(Key.A) && gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.Stand 
                && gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1)
            {
                gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel_v1.ServeEnum.ThrowBall;
            }
            if (Input.IsKeyPressed(Key.Key1))
            {
                gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP1;
                gameplayTennisModel.isServeP1 = true;
                this.gameplayTennisModel.isReset = true;
                this.gameplayTennisModel.Reset();
            }
            if (Input.IsKeyPressed(Key.Key2))
            {
                gameplayTennisModel.gameEnum = GameplayTennisModel_v1.GameEnum.ServeP2;
                gameplayTennisModel.isServeP1 = false;
                //(this.gameplayTennisModel.playerAI as PlayerTennisAI_v1).speedServeStand = (this.gameplayTennisModel.playerAI as PlayerTennisAI_v1).DefineServeStartTime();
                this.gameplayTennisModel.isReset = true;
                this.gameplayTennisModel.Reset();
            }
            if (Input.IsKeyPressed(Key.Q))
            {                
                this.gameplayTennisModel.isServeLeft = true;
                this.gameplayTennisModel.isReset = true;
                this.gameplayTennisModel.Reset();
            }
            if (Input.IsKeyPressed(Key.W))
            {                
                this.gameplayTennisModel.isServeLeft = false;
                this.gameplayTennisModel.isReset = true;
                this.gameplayTennisModel.Reset();
            }
            if (Input.IsKeyPressed(Key.R))
            {
                this.gameplayTennisModel.isReset = true;
                this.gameplayTennisModel.Reset();
            }
        }
        private void BallStates(double delta)
        {            
            

            if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 || gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP2)
            {
                if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.Stand)
                {
                    this.GlobalPosition = gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 ?
                        (gameplayTennisModel.player as PlayerTennis_v1).serveBallRef.GlobalPosition :
                        (gameplayTennisModel.playerAI as PlayerTennisAI_v1).serveBallRef.GlobalPosition;
                }
                else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.ThrowBall)
                {
                    if (!tossStarted)
                    {
                        ballDirectionEnum = BallDirectionEnum.up;
                        tossStarted = true;                        
                        Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;
                        Vector3 pos = GlobalPosition;
                        float alvoX = pos.X;
                        float alvoZ = pos.Z;
                        float altura = pos.Y + 8.0f;
                        CalcularVelocidadesParaAlvo(alvoX, alvoZ, altura);
                    }
                    ThrowBallFunction(delta);
                }
                else if (gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.HitBall)
                {
                    SpeedMultiplier = SpeedMultiplierInit 
                        + (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 ?
                        (gameplayTennisModel.player as PlayerTennis_v1).servePower :
                        (gameplayTennisModel.playerAI as PlayerTennisAI_v1).servePower);
                    Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;                    

                    gameplayTennisModel.gameEnum = gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1 ?
                        GameplayTennisModel_v1.GameEnum.InGameP2 : GameplayTennisModel_v1.GameEnum.InGameP1;

                    gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel_v1.ServeEnum.InGame;                                    
                }
            }
            else if ((gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP1 || gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2) 
                && gameplayTennisModel.playerTennisModel.serveEnum == PlayerTennisModel_v1.ServeEnum.InGame)
            {
                if (gameplayTennisModel.gameEnum != gameEnum &&
                (gameplayTennisModel.gameEnum != GameplayTennisModel_v1.GameEnum.ServeP1 && gameplayTennisModel.gameEnum != GameplayTennisModel_v1.GameEnum.ServeP2))
                {                    
                    if (!isfirstServe)
                    {                                                
                        var specialIncrease = 0.0f;
                        var backHandForeHandIncrease = 0.0f;
                        if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2)
                        {
                            specialIncrease = (gameplayTennisModel.isSpecial ? 2.0f : 0.0f);
                            backHandForeHandIncrease = gameplayTennisModel.isBackhand ? (gameplayTennisModel.player as PlayerTennis_v1).servePower : 0.0f;
                        }                        
                        else
                        {
                            gameplayTennisModel.isSpecial = false;
                            backHandForeHandIncrease = (gameplayTennisModel.playerAI as PlayerTennisAI_v1).servePower;
                        }
                            
                        ShowHideBallTrail(gameplayTennisModel.isSpecial);

                        SpeedMultiplier = SpeedMultiplierInit + specialIncrease + backHandForeHandIncrease;
                        Gravity = BaseGravity * SpeedMultiplier * SpeedMultiplier;

                        GD.Print("-------------------------------");//<-
                        GD.Print(gameplayTennisModel.gameEnum);
                        GD.Print(gameplayTennisModel.isBackhand);
                        GD.Print(gameplayTennisModel.isSpecial);
                        GD.Print(SpeedMultiplier);

                    }
                    isfirstServe = false;
                    gameEnum = gameplayTennisModel.gameEnum;
                }

                BallBounce(delta);                
            }

        }

        private void BallBounce(double delta)
        {            

            if (gameplayTennisModel.playerTennisModel.serveEnumWait == PlayerTennisModel_v1.ServeEnumWait.Init && (GlobalPosition.Z < 1.5f && GlobalPosition.Z > -1.5f))
            {
                gameplayTennisModel.playerTennisModel.serveEnumWait = PlayerTennisModel_v1.ServeEnumWait.Act;                
            }

            if (theBallWasHit)
            {                
                theBallWasHit = false;
                var backHandPlayer = gameplayTennisModel.backHandPosition;
                CalcularVelocidadesParaAlvo(backHandPlayer.X, backHandPlayer.Z, alturaMaximaDesejada);
                trajectoryPoint = PredictTrajectory();

                TrajectoryPoint hit = GetBestHitPoint(bestHitPointMin, bestHitPointMax, gameplayTennisModel.gameEnum);

                var count = 0;
                foreach (Vector3 impact in PredictBounces(2))
                {
                    impactBoxes[count].GlobalPosition = impact;
                    count++;
                }

                if (hit != null)
                {
                    bestHitPosition = hit.Position == Vector3.Zero ? impactBoxes[0].GlobalPosition : hit.Position;
                }                
                
            }

            velocity.Y += Gravity * (float)delta;
            velocity.X *= FrictionXZ;
            velocity.Z *= FrictionXZ;
            Velocity = velocity;
            MoveAndSlide();

            UpdateBallTrail(this.GlobalPosition);

            if (IsOnFloor())
            {
                if (this.GlobalPosition.Z >= 0.0)
                {
                    if(ballBounceCourtEnum == BallBounceCourtEnum.P1)
                        gameplayTennisModel.isPlayFinished = true;
                    ballBounceCourtEnum = BallBounceCourtEnum.P1;
                }
                else
                {
                    if (ballBounceCourtEnum == BallBounceCourtEnum.P2)
                        gameplayTennisModel.isPlayFinished = true;
                    ballBounceCourtEnum = BallBounceCourtEnum.P2;
                }                
                

                velocity.Y = -velocity.Y * BounceFactor;

                velocity.X *= Friction;
                velocity.Z *= Friction;

                if (velocity.Length() < 0.1f * SpeedMultiplier)
                    velocity = Vector3.Zero;
            }


            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                KinematicCollision3D collision = GetSlideCollision(i);

                
                if (collision.GetCollider() is StaticBody3D staticBody)
                {                    
                    if (staticBody.GetParent().Name.ToString().Contains("CourtLimit", StringComparison.OrdinalIgnoreCase))
                    {
                        gameplayTennisModel.isPlayFinished = true;
                    }
                }                
            }
        }

        private void UpdateBallTrail(Vector3 newPoint)
        {            
            for (int i = gameplayTennisModel.ballTrailPathList.Count - 1; i > 0; i--)
            {
                gameplayTennisModel.ballTrailPathList[i].Position =
                    gameplayTennisModel.ballTrailPathList[i - 1].Position;
            }
            gameplayTennisModel.ballTrailPathList[0].Position = newPoint;
        }

        private void ShowHideBallTrail(bool isShow)
        {
            for (int i = 0; i < gameplayTennisModel.ballTrailPathList.Count; i++)            
                gameplayTennisModel.ballTrailPathList[i].Visible = isShow;                         
        }

        private void CreateImpactBox(Vector3 position)
        {
            Node3D box = boxScene.Instantiate<Node3D>();
            GetParent().CallDeferred(Node.MethodName.AddChild, box);
            box.Position = new Vector3(position.X, 0.0f, position.Z);
            impactBoxes.Add(box);
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
        private TrajectoryPoint GetBestHitPoint(float minHeight, float maxHeight, GameplayTennisModel_v1.GameEnum gameEnum)
        {
            foreach (var p in trajectoryPoint)
            {
                if (gameEnum == GameplayTennisModel_v1.GameEnum.InGameP2)
                {
                    if (p.Velocity.Y > 0 &&
                        p.Position.Y >= minHeight &&
                        p.Position.Y <= maxHeight &&
                        p.Position.Z < 0)
                    {
                        return p;
                    }
                }
                else
                {
                    if (p.Velocity.Y > 0 &&
                        p.Position.Y >= minHeight &&
                        p.Position.Y <= maxHeight &&
                        p.Position.Z > 0)
                    {
                        return p;
                    }
                }
                
            }

            return new TrajectoryPoint();
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

        private void ThrowBallFunction(double delta)
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
                if (gameplayTennisModel.gameEnum == GameplayTennisModel_v1.GameEnum.ServeP1)
                {
                    if (this.GlobalPosition.Y < (gameplayTennisModel.player as PlayerTennis_v1).ServeBallGoodMaxRef.GlobalPosition.Y &&
                        this.GlobalPosition.Y > (gameplayTennisModel.player as PlayerTennis_v1).ServeBallGoodMinRef.GlobalPosition.Y)
                    {
                        gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel_v1.ServeEnum.HitBall;
                    }
                }
                else
                {
                    if (this.GlobalPosition.Y < (gameplayTennisModel.playerAI as PlayerTennisAI_v1).ServeBallGoodMaxRef.GlobalPosition.Y &&
                        this.GlobalPosition.Y > (gameplayTennisModel.playerAI as PlayerTennisAI_v1).ServeBallGoodMinRef.GlobalPosition.Y)
                    {
                        gameplayTennisModel.playerTennisModel.serveEnum = PlayerTennisModel_v1.ServeEnum.HitBall;
                    }
                }
            }
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

        public TennisBall_v1 GameplayTennisModelBuilder(GameplayTennisModel_v1 gameplayTennisModel)
        {
            this.gameplayTennisModel = gameplayTennisModel;            
            return this;
        }
        public TennisBall_v1 SetServePointBuilder()
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

        public TennisBall_v1 SetCourtPointBuilder()
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

        public TennisBall_v1 SetBoxSceneBuilder()
        {
            this.gameplayTennisModel.boxScene = GD.Load<PackedScene>(this.gameplayTennisModel.boxScene_Name);
            return this;
        }


        public void Reset(int playerID)
        {
            Gravity = 0.0f;
            velocity = Vector3.Zero;            
            tossStarted = false;
            ballDirectionEnum = BallDirectionEnum.neutral;
            gameplayTennisModel.gameEnum = (GameplayTennisModel_v1.GameEnum)playerID;
            ballDirectionEnum = BallDirectionEnum.neutral;
            theBallWasHit = false;
            ballBounceCourtEnum = BallBounceCourtEnum.neutral;
            isfirstServe = true;
            SpeedMultiplier = SpeedMultiplierInit;
            ShowHideBallTrail(false);
        }

    }
}
