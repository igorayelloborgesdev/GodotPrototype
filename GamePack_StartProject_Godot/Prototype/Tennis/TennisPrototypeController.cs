using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis
{
    public class TennisPrototypeController
    {
        private TennisPrototypeFacade gameplayFacade = new TennisPrototypeFacade();
        private GameplayTennisModel gameplayModel = new GameplayTennisModel();        
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
