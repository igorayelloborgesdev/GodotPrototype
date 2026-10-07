using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypePhysicsModel
    {
        public enum PhysicsStatesEnum
        {
            Init = 1,
            Running = 2,
            Finish = 3
        }
        public PhysicsStatesEnum physicsStates { get; set; } = PhysicsStatesEnum.Init;
    }
}
