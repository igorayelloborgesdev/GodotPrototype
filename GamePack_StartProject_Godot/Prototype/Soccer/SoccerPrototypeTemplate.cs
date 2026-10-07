using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public abstract class AbstractEventClass
    {
        public abstract void EventExecute(InputEvent @event);
    }
    public class ConcreteClassMoveBall : AbstractEventClass
    {
        SoccerPrototypePhysicsController soccerPrototypePhysicsController;
        public ConcreteClassMoveBall SetPrototypeBall(SoccerPrototypePhysicsController soccerPrototypePhysicsController)
        {            
            this.soccerPrototypePhysicsController = soccerPrototypePhysicsController;
            return this;
        }
        public override void EventExecute(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent && keyEvent.Pressed)
            {
                if (keyEvent.Keycode == Key.Enter)
                {
                    soccerPrototypePhysicsController.SetNetxState();                    
                }
            }
        }
    }
    public class ClientPrototypeBall
    {
        private AbstractEventClass abstractEventClass = null;
        public ClientPrototypeBall SetAbstractEventClassBuilder(AbstractEventClass abstractEventClass)
        {
            this.abstractEventClass = abstractEventClass;
            return this;
        }
        public void ClientCode(InputEvent @event)
        {
            abstractEventClass.EventExecute(@event);
        }
    }
}
