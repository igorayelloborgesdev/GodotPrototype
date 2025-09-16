using GamePackStartProjectGodot.Scripts.Command;
using GamePackStartProjectGodot.Scripts.Decorator;
using GamePackStartProjectGodot.Scripts.DTO;
using GamePackStartProjectGodot.Scripts.Factory;
using GamePackStartProjectGodot.Scripts.Model;
using GamePackStartProjectGodot.Scripts.Observer;
using GamePackStartProjectGodot.Scripts.Util;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GamePackStartProjectGodot.Scripts.Facade
{
    public class MainMenuFacade
    {
        InputConfigSubSystem inputConfigSubSystem;

        public void Init(Control control, MainButtonModel mainButtonModel)
        {
            new MainClient().CreateObjects<Label>(control, mainButtonModel.upperMenuControl_TitleNinePatchRect_TitleLabelNodeName, 
                out mainButtonModel.titleLabel);            
            new MainClient().CreateObjects<Control>(control, mainButtonModel.mainMenuControlsNodeName, mainButtonModel.mainMenuControlsList);
            mainButtonModel.mainMenuControlsList.RemoveAt(0);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.upperMenuControlNodeName, mainButtonModel.mainMenuButtonsList);            
            AssignEventToButton(mainButtonModel.mainMenuButtonsList, new MainMenuButtonConcreteDecorator()
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuControlsList).SetObserverBuilder<Label>(mainButtonModel.titleLabel), mainButtonModel);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenControl_MainMenuScreenControl5_QuitGameButtonName,
                out mainButtonModel.quitGameButton);
            AssignEventToButton(mainButtonModel.quitGameButton, new MainMenuQuitButtonConcreteDecorator()
                .SetObserverBuilder<Control>(control), mainButtonModel);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenControl_MainMenuScreenControl2_LanguageGameButtonName,
                out mainButtonModel.languageGameButton);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenControl_MainMenuScreenControl2_InputGameButtonName,
                out mainButtonModel.inputGameButton);
            new MainClient().CreateObjects<Control>(control, mainButtonModel.mainMenuScreenConfigControlName,
                out mainButtonModel.mainMenuScreenConfigControl);
            new MainClient().CreateObjects<Label>(control, mainButtonModel.mainMenuScreenConfigControl_TitleNinePatchRect_TitleLabelName,
                out mainButtonModel.mainMenuScreenConfigControlTitleNinePatchRectTitleLabel);

            new MainClient().CreateObjects<Control>(control, mainButtonModel.mainMenuScreenConfigControlName, mainButtonModel.mainMenuScreenConfigControlList);
            mainButtonModel.mainMenuScreenConfigControlList.RemoveAt(0);

            AssignEventToButton(mainButtonModel.inputGameButton,
                new ConfigButtonConcreteDecorator()
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuScreenConfigControl)
                .SetObserverBuilder<Label>(mainButtonModel.mainMenuScreenConfigControlTitleNinePatchRectTitleLabel)
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuScreenConfigControlList)
                , 0 ,mainButtonModel);
            AssignEventToButton(mainButtonModel.languageGameButton,
                new ConfigButtonConcreteDecorator()
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuScreenConfigControl)
                .SetObserverBuilder<Label>(mainButtonModel.mainMenuScreenConfigControlTitleNinePatchRectTitleLabel)
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuScreenConfigControlList)
                , 1, mainButtonModel);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenConfigControl_BackButtonName,
                out mainButtonModel.mainMenuScreenConfigControl_BackButton);            
            AssignEventToButton(mainButtonModel.mainMenuScreenConfigControl_BackButton,
                new ConfigButtonConcreteDecorator()
                .SetObserverBuilder<Control>(mainButtonModel.mainMenuScreenConfigControl), mainButtonModel);

            mainButtonModel.mainMenuScreenConfigControl.Hide();

            new MainClient().CreateObjects<LangStatic>(control,
                mainButtonModel.mainMenuScreenConfigControl_InputScreenControl,
                mainButtonModel.mainMenuScreenConfigControl_InputScreenControl_ArrayLabel,
                mainButtonModel.mainMenuButtonsConfigLabelList,
                true);

            new MainClient().CreateObjects<Button>(control,
                mainButtonModel.mainMenuScreenConfigControl_InputScreenControl,                
                mainButtonModel.mainMenuButtonsConfigButtonList);

            new MainClient().CreateObjects<Label>(control,
                mainButtonModel.mainMenuScreenConfigControl_InputScreenControl,
                mainButtonModel.mainMenuScreenConfigControl_InputScreenControl_ArrayButton,
                mainButtonModel.mainMenuButtonsConfigButtonLabelList,
                false);

            AssignEventToButton(mainButtonModel.mainMenuButtonsConfigButtonList, new ConfigInputButtonConcreteDecorator()
                .SetObserverBuilder<Label>(mainButtonModel.mainMenuButtonsConfigLabelList.ToList<Label>(), mainButtonModel.mainMenuButtonsConfigButtonLabelList), 
                 mainButtonModel.configDTO, -1, false);            

            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenConfigControl_SaveButtonLabel, 
                out mainButtonModel.mainMenuScreenConfigControl_SaveButton);            
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenConfigControl_RestoreButtonLabel,
                out mainButtonModel.mainMenuScreenConfigControl_RestoreButton);            

            new MainClient().CreateObjects<Control>(control, mainButtonModel.mainMenuScreenModalControlLabel,
                out mainButtonModel.mainMenuScreenModalControl);
            new MainClient().CreateObjects<Label>(control, mainButtonModel.mainMenuScreenModalControl_ModalScreenControl_Label,
                out mainButtonModel.mainMenuScreenModalControlModalScreenControlLabel);
            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenModalControl_BackButton,
                out mainButtonModel.mainMenuScreenModalControlBackButton);
            new MainClient().CreateObjects<Label>(control, mainButtonModel.mainMenuScreenModalControl_TitleNinePatchRect_TitleLabel,
                out mainButtonModel.mainMenuScreenModalControlTitleNinePatchRectTitleLabel);

            AssignEventToButton(mainButtonModel.mainMenuScreenConfigControl_SaveButton,
                new ConfigButtonSaveConfigConcreteDecorator()
                .SetMainButtonModelBuilder(mainButtonModel), mainButtonModel);
            AssignEventToButton(mainButtonModel.mainMenuScreenConfigControl_RestoreButton,
                new ConfigButtonRestoreConfigConcreteDecorator()
                .SetMainButtonModelBuilder(mainButtonModel), mainButtonModel);

            AssignEventToButton(mainButtonModel.mainMenuScreenModalControlBackButton,
                new ConfigButtonModalConfigConcreteDecorator()
                .SetMainButtonModelBuilder(mainButtonModel), mainButtonModel);

            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenConfigControl_JoystickButtonLabel,
                out mainButtonModel.mainMenuScreenConfigControl_JoystickButton);

            new MainClient().CreateObjects<Button>(control, mainButtonModel.mainMenuScreenConfigControl_KeyboardButtonLabel,
                out mainButtonModel.mainMenuScreenConfigControl_KeyboardButton);

            new MainClient().CreateObjects<LangStatic>(control, mainButtonModel.mainMenuScreenConfigControl_JoystickButton_TitleNinePatchRect_Label,
                out mainButtonModel.mainMenuScreenConfigControl_JoystickButton_Label);

            new MainClient().CreateObjects<LangStatic>(control, mainButtonModel.mainMenuScreenConfigControl_KeyboardButton_TitleNinePatchRect_Label,
                out mainButtonModel.mainMenuScreenConfigControl_KeyboardButton_Label);
            
            mainButtonModel.mainMenuButtonsConfigKeyButtonList.Add(mainButtonModel.mainMenuScreenConfigControl_KeyboardButton);
            mainButtonModel.mainMenuButtonsConfigKeyButtonList.Add(mainButtonModel.mainMenuScreenConfigControl_JoystickButton);
            
            mainButtonModel.mainMenuButtonsConfigKeyLabelList.Add(mainButtonModel.mainMenuScreenConfigControl_KeyboardButton_Label);
            mainButtonModel.mainMenuButtonsConfigKeyLabelList.Add(mainButtonModel.mainMenuScreenConfigControl_JoystickButton_Label);

            AssignEventToButton(mainButtonModel.mainMenuButtonsConfigKeyButtonList, new ConfigInputButtonKeyConcreteDecorator()
                .SetObserverBuilder<Label>(mainButtonModel.mainMenuButtonsConfigKeyLabelList, mainButtonModel.mainMenuButtonsConfigKeyLabelList),
                 mainButtonModel.configDTO, 0, true);

            new InputConfigClient().CreateObjects(mainButtonModel);
            inputConfigSubSystem = new InputConfigSubSystem().ConfigInputInitBuilder(mainButtonModel);
            
            new MainClient().CreateObjects<Button>(control,
                 mainButtonModel.mainMenuScreenConfigControl_LanguageScreenControl_LangButton_Control,
                 mainButtonModel.mainMenuScreenConfigControl_LanguageScreenControl_LangButton_Array_Label,
                 mainButtonModel.mainMenuButtonsLangKeyButtonList,
                 false);

            new MainClient().CreateObjects<Label>(control,
                 mainButtonModel.mainMenuScreenConfigControl_LanguageScreenControl_LangButton_Control,
                 mainButtonModel.mainMenuScreenConfigControl_LanguageScreenControl_LangLabel_Array_Label,
                 mainButtonModel.mainMenuButtonsLangKeyLabelList,
                 true);

            new MainClient().CreateObjects<LangStatic>(control, mainButtonModel.mainMenuScreenControl_Label, mainButtonModel.langStaticList);
            new MainClient().CreateObjects<LangStatic>(control, mainButtonModel.mainMenuScreenControl_ConfigButton_Label, mainButtonModel.langStaticList);
            new MainClient().CreateObjects<LangStatic>(control, mainButtonModel.mainMenuScreenControl_InputScreenControl_Label, mainButtonModel.langStaticList);

            mainButtonModel.languageCommand = new LanguageConcreteCommand(mainButtonModel.languageReceiver.LangStaticListBuilder(mainButtonModel.langStaticList));

            AssignEventToButton(mainButtonModel.mainMenuButtonsLangKeyButtonList, new ConfigLanguageButtonKeyConcreteDecorator()
                .SetObserverBuilder<Label>(mainButtonModel.mainMenuButtonsLangKeyLabelList, mainButtonModel.mainMenuButtonsLangKeyLabelList)
                .SetCommandBuilder(mainButtonModel.languageCommand)
                .SetMainMenuSubjectBuilder(mainButtonModel.mainMenuSubjectList)
                , mainButtonModel.configDTO, -1, false);

        }
        private void AssignEventToButton(List<Button> mainMenuButtonsList, MainMenuButtonComponent mainMenuButtonComponent, MainButtonModel mainButtonModel)
        {            
            int count = 0;            
            foreach (var mainMenuButton in mainMenuButtonsList)
            {                
                int countNew = count;                
                mainMenuButton.Pressed += () => { mainMenuButtonComponent.Operation<Button>(countNew); };
                count++;
            }            
            mainMenuButtonComponent.Operation<Button>(0);
            SetMainMenuSubjectList(mainMenuButtonComponent, mainButtonModel);
        }
        private void AssignEventToButton(Button mainMenuButtons, MainMenuButtonComponent mainMenuButtonComponent, MainButtonModel mainButtonModel)
        {
            mainMenuButtons.Pressed += () => { mainMenuButtonComponent.Operation<Button>(); };
            SetMainMenuSubjectList(mainMenuButtonComponent, mainButtonModel);
        }
        private void AssignEventToButton(Button mainMenuButtons, MainMenuButtonComponent mainMenuButtonComponent, int id, MainButtonModel mainButtonModel)
        {
            mainMenuButtons.Pressed += () => { mainMenuButtonComponent.Operation<Button>(id); };
            SetMainMenuSubjectList(mainMenuButtonComponent, mainButtonModel);
        }
        private void AssignEventToButton(List<Button> mainMenuButtonsList, ConfigButtonComponent configButtonComponent, ConfigDTO configDTO, int initId, bool isInput)
        {
            int count = 0;            
            foreach (var mainMenuButton in mainMenuButtonsList)
            {
                int countNew = count;
                mainMenuButton.Pressed += () => { configButtonComponent.Operation<Button>(countNew, configDTO); };
                count++;
            }            
            if(isInput)
                configButtonComponent.Operation<Button>(ConfigSingleton.saveConfigDTO is null ? 0 : ConfigSingleton.saveConfigDTO.keyboardJoystick, configDTO);
            else
                configButtonComponent.Operation<Button>(initId, configDTO);

        }
        public void Update(double delta, MainButtonModel mainButtonModel)
        {
            if(inputConfigSubSystem is not null)
                inputConfigSubSystem.ConfigInput(mainButtonModel);
        }
        private void SetMainMenuSubjectList(MainMenuButtonComponent mainMenuButtonComponent, MainButtonModel mainButtonModel)
        {
            if (mainMenuButtonComponent.GetMainMenuSubject() is not null)            
                mainButtonModel.mainMenuSubjectList.Add(mainMenuButtonComponent.GetMainMenuSubject());            
        }
    }
    public class InputConfigSubSystem
    {
        KeyAbstract keyInput = new KeyboardInput();
        public void ConfigInput(MainButtonModel mainButtonModel)
        {            
            KeyObj keyObj = null;
            if (mainButtonModel.configDTO.isAssign)
            {
                if(mainButtonModel.configDTO.idKeyInput == 0)
                    keyInput = new KeyboardInput();
                else
                    keyInput = new JoystickInput();

                keyObj = keyInput.GetKeyPressed();
                if (keyObj is not null)
                {                    
                    mainButtonModel.configDTO.isAssign = false;
                    mainButtonModel.inputKeyConfgInputConcreteColleague1.Send(keyObj, mainButtonModel.configDTO.idKey);
                }                
            }            
        }
        public InputConfigSubSystem ConfigInputInitBuilder(MainButtonModel mainButtonModel)
        {            
            if (ConfigSingleton.saveConfigDTO is null)
            {
                ConfigSingleton.saveConfigDTO = new SaveConfigDTO();
                ConfigSingleton.saveConfigDTO.keysControlArray.AddRange(ConfigDefaultInputs.keysControlArray);
            }
            for (int i = 0; i < ConfigSingleton.saveConfigDTO.keysControlArray.Count; i++)
            {
                mainButtonModel.inputKeyConfgInputConcreteColleague1.Send(ConfigSingleton.saveConfigDTO.keysControlArray[i], i);
            }
            return this;
        }
        public InputConfigSubSystem KeyInputBuilder(KeyAbstract keyInput)
        {
            this.keyInput = keyInput;
            return this;
        }
    }
}
