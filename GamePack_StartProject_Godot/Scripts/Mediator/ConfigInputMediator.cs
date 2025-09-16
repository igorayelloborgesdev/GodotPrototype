using GamePackStartProjectGodot.Scripts.Decorator;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Label = Godot.Label;

namespace GamePackStartProjectGodot.Scripts.Mediator
{    
    public abstract class ConfgInputMediator
    {
        public abstract void Send(KeyObj keyObj,
            ConfgInputColleague colleague, int id);
    }    
    public class ConfgInputConcreteMediator : ConfgInputMediator
    {
        ConfgInputConcreteColleague1 colleague1;
        ConfgInputConcreteColleague2 colleague2;
        public ConfgInputConcreteColleague1 Colleague1
        {
            set { colleague1 = value; }
        }
        public ConfgInputConcreteColleague2 Colleague2
        {
            set { colleague2 = value; }
        }
        public override void Send(KeyObj keyObj, ConfgInputColleague colleague, int id)
        {
            if (colleague == colleague1)
            {
                colleague2.Notify(keyObj, id);
            }            
        }
    }
    public abstract class ConfgInputColleague
    {
        protected ConfgInputMediator mediator;        
        public ConfgInputColleague(ConfgInputMediator mediator)
        {
            this.mediator = mediator;
        }
    }
    public class ConfgInputConcreteColleague1 : ConfgInputColleague
    {        
        public ConfgInputConcreteColleague1(ConfgInputMediator mediator)
            : base(mediator)
        {
        }
        public void Send(KeyObj keyObj, int id)
        {
            mediator.Send(keyObj, this, id);
        }        
    }    
    public class ConfgInputConcreteColleague2 : ConfgInputColleague
    {
        protected List<Label> mainMenuButtonsConfigLabelList;        
        protected List<Label> mainMenuButtonsConfigButtonLabelList;        
        protected string colorNormal = "000000";        
        public ConfgInputConcreteColleague2(ConfgInputMediator mediator)
            : base(mediator)
        {
        }

        public ConfgInputConcreteColleague2 BuilderMainMenuButtonsConfigLabelList(List<LangStatic> mainMenuButtonsConfigLabelList)
        {            
            this.mainMenuButtonsConfigLabelList = mainMenuButtonsConfigLabelList.ToList<Label>();
            return this;
        }        
        public ConfgInputConcreteColleague2 BuilderMainMenuButtonsConfigButtonLabelList(List<Label> mainMenuButtonsConfigButtonLabelList)
        {
            this.mainMenuButtonsConfigButtonLabelList = mainMenuButtonsConfigButtonLabelList;
            return this;
        }        
        public void Send(KeyObj keyObj, int id)
        {
            mediator.Send(keyObj, this, id);
        }
        public void Notify(KeyObj keyObj, int id)
        {            
            SetLabelColor(mainMenuButtonsConfigLabelList, colorNormal);
            SetLabelColor(mainMenuButtonsConfigButtonLabelList, colorNormal);
            SetLabelText(mainMenuButtonsConfigButtonLabelList[id], keyObj.keyName);            
            SetKeyObj(keyObj, id);
        }
        private void SetLabelColor(List<Label> labelList, string colorName)
        {
            foreach (var label in labelList)
            {
                if (label != null)
                {
                    label.AddThemeColorOverride("font_color", new Color(colorName));
                }
            }
        }
        private void SetLabelText(Label label, string text)
        {            
            if (label != null)
            {
                label.Text = text;
            }         
        }
        private void SetKeyObj(KeyObj keyObj, int id)
        {
            ConfigSingleton.saveConfigDTO.keysControlArray[id] = keyObj;
        }
    }
}
