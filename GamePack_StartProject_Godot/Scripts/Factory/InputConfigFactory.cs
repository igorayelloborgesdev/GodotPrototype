using GamePackStartProjectGodot.Scripts.Mediator;
using GamePackStartProjectGodot.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Factory
{
    public class InputConfigClient
    {
        public void CreateObjects(MainButtonModel mainButtonModel) 
        {
            mainButtonModel.inputKeyConfgInputConcreteColleague1 = new ConfgInputConcreteColleague1(mainButtonModel.confgInputConcreteMediator);
            mainButtonModel.inputKeyLabelConfgInputConcreteColleague1 = new ConfgInputConcreteColleague2(mainButtonModel.confgInputConcreteMediator);
            mainButtonModel.confgInputConcreteMediator.Colleague1 = mainButtonModel.inputKeyConfgInputConcreteColleague1;
            mainButtonModel.confgInputConcreteMediator.Colleague2 = mainButtonModel.inputKeyLabelConfgInputConcreteColleague1;
            mainButtonModel.inputKeyLabelConfgInputConcreteColleague1.BuilderMainMenuButtonsConfigLabelList(mainButtonModel.mainMenuButtonsConfigLabelList);            
            mainButtonModel.inputKeyLabelConfgInputConcreteColleague1.BuilderMainMenuButtonsConfigButtonLabelList(mainButtonModel.mainMenuButtonsConfigButtonLabelList);            
        }
    }
}
