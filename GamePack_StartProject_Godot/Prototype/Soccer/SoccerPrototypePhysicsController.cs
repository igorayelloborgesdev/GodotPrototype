using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GamePackStartProjectGodot.Prototype.Soccer.SoccerPrototypePhysicsModel;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypePhysicsController
    {
        public SoccerPrototypePhysicsModel soccerPrototypePhysicsModel = new SoccerPrototypePhysicsModel();
        public void SetNetxState()
        {            
            if ((int)soccerPrototypePhysicsModel.physicsStates == Enum.GetNames(typeof(PhysicsStatesEnum)).Length)
            {
                soccerPrototypePhysicsModel.physicsStates = (PhysicsStatesEnum)1;
                return;
            }
            soccerPrototypePhysicsModel.physicsStates++;            
        }
    }
}
