using GamePackStartProjectGodot.Scripts.Command;
using GamePackStartProjectGodot.Scripts.DTO;
using GamePackStartProjectGodot.Scripts.Observer;
using GamePackStartProjectGodot.Scripts.Singleton;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Decorator
{
    public abstract class ConfigButtonComponent
    {
        public abstract void Operation<T>(int id, ConfigDTO configDTO) where T : Node;        
        public abstract ConfigButtonComponent SetObserverBuilder<T>(List<T> inputListLabel, List<T> inputButtonListLabel) where T : Node;
        public abstract ConfigButtonComponent SetCommandBuilder(LanguageCommand languageCommand);
        public abstract ConfigButtonComponent SetMainMenuSubjectBuilder(List<MainMenuSubject> mainMenuSubject);
    }

    public class ConfigInputButtonConcreteDecorator : ConfigButtonComponent
    {
        protected List<Node> inputListLabel = new List<Node>();
        protected List<Node> inputButtonListLabel = new List<Node>();        
        protected string colorNormal = "000000";
        protected string colorSelected = "e7a706";        
        public override void Operation<T>(int id, ConfigDTO configDTO)
        {
            configDTO.idKey = id;            
            SetLabelColor(inputListLabel, colorNormal);
            SetLabelColor(inputButtonListLabel, colorNormal);
            if (configDTO.idKey != -1 && configDTO.idKey < inputButtonListLabel.Count )
            {
                configDTO.isAssign = true;
                SetLabelColor(inputListLabel[configDTO.idKey], colorSelected);
                SetLabelColor(inputButtonListLabel[configDTO.idKey], colorSelected);
            }                                                
        }
        public override ConfigButtonComponent SetObserverBuilder<T>(List<T> inputListLabel, List<T> inputButtonListLabel)
        {
            this.inputListLabel.AddRange(inputListLabel);
            this.inputButtonListLabel.AddRange(inputButtonListLabel);
            return this;
        }
        public override ConfigButtonComponent SetCommandBuilder(LanguageCommand languageCommand)
        {
            return this;
        }
        public override ConfigButtonComponent SetMainMenuSubjectBuilder(List<MainMenuSubject> mainMenuSubject)
        {
            return this;
        }
        private void SetLabelColor(List<Node> labelList, string colorName)
        {
            foreach (var label in labelList)
            {
                if (label != null)
                {
                    (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));                                        
                }                
            }
        }
        private void SetLabelColor(Node label, string colorName)
        {            
            if (label != null)
            {
                (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));
            }            
        }
    }

    public class ConfigInputButtonKeyConcreteDecorator : ConfigButtonComponent
    {
        protected List<Node> inputListLabel = new List<Node>();        
        protected string colorNormal = "000000";
        protected string colorSelected = "e7a706";
        public override void Operation<T>(int id, ConfigDTO configDTO)
        {
            configDTO.idKeyInput = id;
            SetLabelColor(inputListLabel, colorNormal);            
            if (configDTO.idKeyInput != -1)
            {
                configDTO.isAssign = true;
                SetLabelColor(inputListLabel[configDTO.idKeyInput], colorSelected);
                if(ConfigSingleton.saveConfigDTO is not null)
                    ConfigSingleton.saveConfigDTO.keyboardJoystick = id;
            }
        }
        public override ConfigButtonComponent SetObserverBuilder<T>(List<T> inputListLabel, List<T> inputButtonListLabel)
        {
            this.inputListLabel.AddRange(inputListLabel);            
            return this;
        }
        public override ConfigButtonComponent SetCommandBuilder(LanguageCommand languageCommand)
        {
            return this;
        }
        public override ConfigButtonComponent SetMainMenuSubjectBuilder(List<MainMenuSubject> mainMenuSubject)
        {
            return this;
        }
        private void SetLabelColor(List<Node> labelList, string colorName)
        {
            foreach (var label in labelList)
            {
                if (label != null)
                {
                    (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));
                }
            }
        }
        private void SetLabelColor(Node label, string colorName)
        {
            if (label != null)
            {
                (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));
            }
        }
    }

    public class ConfigLanguageButtonKeyConcreteDecorator : ConfigButtonComponent
    {
        protected List<Node> inputListLabel = new List<Node>();
        protected string colorNormal = "000000";
        protected string colorSelected = "e7a706";
        protected LanguageCommand languageCommand;
        protected List<MainMenuSubject> mainMenuSubjectList = new List<MainMenuSubject>();
        public override void Operation<T>(int id, ConfigDTO configDTO)
        {                        
            if (id != -1)
            {                
                ConfigSingleton.saveConfigDTO.languageId = id;
                SaveLoad.SaveConfig<SaveConfigDTO>(ConfigSingleton.saveConfigDTO);
            }            
            SetLabelColor(inputListLabel, colorNormal);
            SetLabelColor(inputListLabel[ConfigSingleton.saveConfigDTO.languageId], colorSelected);
            LanguageSingleton.selectedLanguage = LanguageSingleton.language[ConfigSingleton.saveConfigDTO.languageId];
            this.languageCommand.Execute();
            foreach (var mainMenuSubject in this.mainMenuSubjectList) 
            {
                mainMenuSubject.NotifyByKey();
            }            
        }
        public override ConfigButtonComponent SetObserverBuilder<T>(List<T> inputListLabel, List<T> inputButtonListLabel)
        {
            this.inputListLabel.AddRange(inputListLabel);
            return this;
        }
        public override ConfigButtonComponent SetCommandBuilder(LanguageCommand languageCommand)
        {
            this.languageCommand = languageCommand;
            return this;
        }
        public override ConfigButtonComponent SetMainMenuSubjectBuilder(List<MainMenuSubject> mainMenuSubject)
        {
            this.mainMenuSubjectList = mainMenuSubject;
            return this;
        }
        private void SetLabelColor(List<Node> labelList, string colorName)
        {
            foreach (var label in labelList)
            {
                if (label != null)
                {
                    (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));
                }
            }
        }
        private void SetLabelColor(Node label, string colorName)
        {
            if (label != null)
            {
                (label as Godot.Label).AddThemeColorOverride("font_color", new Color(colorName));
            }
        }
    }

}
