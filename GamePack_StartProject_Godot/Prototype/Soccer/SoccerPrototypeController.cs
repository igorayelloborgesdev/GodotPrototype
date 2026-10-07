using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypeController
    {
        private SoccerPrototypeFacade soccerPrototypeFacade = new SoccerPrototypeFacade();
        private SoccerPrototypeModel soccerPrototypeModel = new SoccerPrototypeModel();
        private SoccerPrototypePhysicsController soccerPrototypePhysicsController = new SoccerPrototypePhysicsController();
        public void Init(Node node)
        {
            soccerPrototypeFacade.Init(node, soccerPrototypeModel);
            soccerPrototypeFacade.InitEvents(soccerPrototypeModel, soccerPrototypePhysicsController);
        }
        public void Update(double delta)
        {
            GD.Print(soccerPrototypePhysicsController.soccerPrototypePhysicsModel.physicsStates);
        }
        public void Input(InputEvent @event)
        {
            soccerPrototypeModel.client.ClientCode(@event);
        }
    }
}
