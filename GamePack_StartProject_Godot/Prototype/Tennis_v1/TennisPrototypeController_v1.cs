using GamePackStartProjectGodot.Prototype.Tennis;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public class TennisPrototypeController_v1
    {
        private TennisPrototypeFacade_v1 gameplayFacade = new TennisPrototypeFacade_v1();
        private GameplayTennisModel_v1 gameplayModel = new GameplayTennisModel_v1();
        public void Init(Node control)
        {
            gameplayFacade.Init(control, gameplayModel);
        }
        public void Update(double delta)
        {            
            gameplayFacade.Update(delta, gameplayModel);
        }
        public void PhysicsProcess(double delta)
        {
            gameplayFacade.PhysicsProcess(delta, gameplayModel);
        }
    }
}
