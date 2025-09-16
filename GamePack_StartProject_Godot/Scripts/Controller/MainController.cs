using GamePackStartProjectGodot.Scripts.DTO;
using GamePackStartProjectGodot.Scripts.Facade;
using GamePackStartProjectGodot.Scripts.Model;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Controller
{
    public class MainController
    {
        private MainMenuFacade mainMenuFacade = new MainMenuFacade();
        private MainButtonModel mainButtonModel = new MainButtonModel();
        public void Init(Control control)
        {
            mainMenuFacade.Init(control, mainButtonModel);            
        }
        public void Update(double delta)
        {
            mainMenuFacade.Update(delta, mainButtonModel);            
        }
    }
}
