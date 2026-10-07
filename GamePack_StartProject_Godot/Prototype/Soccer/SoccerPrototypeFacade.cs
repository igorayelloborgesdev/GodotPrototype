using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypeFacade
    {
        public void Init(Node node, SoccerPrototypeModel soccerPrototypeModel)
        {
            new SoccerPrototypeFactory().CreateObjects<MeshInstance3D>(node, soccerPrototypeModel.ballName, out soccerPrototypeModel.ball);
        }
        public void InitEvents(SoccerPrototypeModel soccerPrototypeModel, SoccerPrototypePhysicsController soccerPrototypePhysicsController)
        {
            soccerPrototypeModel.concreteClassMoveBall = new ConcreteClassMoveBall().SetPrototypeBall(soccerPrototypePhysicsController);
            soccerPrototypeModel.client = new ClientPrototypeBall().SetAbstractEventClassBuilder(soccerPrototypeModel.concreteClassMoveBall);
        }
    }
}
