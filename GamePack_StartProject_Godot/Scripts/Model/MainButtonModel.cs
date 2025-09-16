using GamePackStartProjectGodot.Scripts.Command;
using GamePackStartProjectGodot.Scripts.DTO;
using GamePackStartProjectGodot.Scripts.Mediator;
using GamePackStartProjectGodot.Scripts.Observer;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Model
{
    public class MainButtonModel
    {
        #region Node Name
        public string upperMenuControlNodeName { get; set; } = "UpperMenuControl";
        public string mainMenuControlsNodeName { get; set; } = "MainMenuScreenControl";
        public string upperMenuControl_TitleNinePatchRect_TitleLabelNodeName { get; set; } = "UpperMenuControl/TitleNinePatchRect/TitleLabel";
        public string mainMenuScreenControl_MainMenuScreenControl5_QuitGameButtonName { get; set; } = "MainMenuScreenControl/MainMenuScreenControl5/QuitGameButton";
        public string mainMenuScreenControl_MainMenuScreenControl2_LanguageGameButtonName { get; set; } = "MainMenuScreenControl/MainMenuScreenControl2/LanguageGameButton";
        public string mainMenuScreenControl_MainMenuScreenControl2_InputGameButtonName { get; set; } = "MainMenuScreenControl/MainMenuScreenControl2/InputGameButton";
        public string mainMenuScreenConfigControlName { get; set; } = "MainMenuScreenConfigControl";
        public string mainMenuScreenConfigControl_BackButtonName { get; set; } = "MainMenuScreenConfigControl/BackButton";
        public string mainMenuScreenConfigControl_TitleNinePatchRect_TitleLabelName { get; set; } = "MainMenuScreenConfigControl/TitleNinePatchRect/TitleLabel";
        public string mainMenuScreenConfigControl_InputScreenControl { get; set; } = "MainMenuScreenConfigControl/InputScreenControl";
        public string[] mainMenuScreenConfigControl_InputScreenControl_ArrayLabel { get; set; } = {
            "InputUpTitleNinePatchRect",
            "InputDownTitleNinePatchRect",
            "InputLeftTitleNinePatchRect",
            "InputRightTitleNinePatchRect",
            "InputButton1TitleNinePatchRect",
            "InputButton2TitleNinePatchRect",
            "InputPauseTitleNinePatchRect"
        };
        public string[] mainMenuScreenConfigControl_InputScreenControl_ArrayButton { get; set; } = {
            "InputUpButton",
            "InputDownButton",
            "InputLeftButton",
            "InputRightButton",
            "InputButton1Button",
            "InputButton2Button",
            "InputPauseButton"
        };
        public string mainMenuScreenConfigControl_SaveButtonLabel { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/SaveButton";
        public string mainMenuScreenConfigControl_RestoreButtonLabel { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/RestoreButton";
        public string mainMenuScreenModalControlLabel { get; set; } = "MainMenuScreenModalControl";
        public string mainMenuScreenModalControl_ModalScreenControl_Label { get; set; } = "MainMenuScreenModalControl/ModalScreenControl/Label";
        public string mainMenuScreenModalControl_BackButton { get; set; } = "MainMenuScreenModalControl/BackButton";
        public string mainMenuScreenModalControl_TitleNinePatchRect_TitleLabel { get; set; } = "MainMenuScreenModalControl/TitleNinePatchRect/TitleLabel";
        public string mainMenuScreenConfigControl_JoystickButtonLabel { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/JoystickButton";
        public string mainMenuScreenConfigControl_KeyboardButtonLabel { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/KeyboardButton";
        public string mainMenuScreenConfigControl_JoystickButton_TitleNinePatchRect_Label { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/JoystickButton/TitleNinePatchRect/Label";
        public string mainMenuScreenConfigControl_KeyboardButton_TitleNinePatchRect_Label { get; set; } = "MainMenuScreenConfigControl/InputScreenControl/KeyboardButton/TitleNinePatchRect/Label";        
        public string mainMenuScreenControl_Label { get; set; } = "MainMenuScreenControl";
        public string mainMenuScreenControl_ConfigButton_Label { get; set; } = "MainMenuScreenControl/MainMenuScreenControl2";
        public string mainMenuScreenControl_InputScreenControl_Label { get; set; } = "MainMenuScreenConfigControl/InputScreenControl";
        public string mainMenuScreenConfigControl_LanguageScreenControl_LangButton_Control { get; set; } = "MainMenuScreenConfigControl/LanguageScreenControl";
        public string[] mainMenuScreenConfigControl_LanguageScreenControl_LangButton_Array_Label { get; set; } =
        {
            "Lang0Button",
            "Lang1Button",
        };
        public string[] mainMenuScreenConfigControl_LanguageScreenControl_LangLabel_Array_Label { get; set; } =
        {
            "Lang0Button/TitleNinePatchRect",
            "Lang1Button/TitleNinePatchRect",
        };
        #endregion
        #region UI
        public List<Button> mainMenuButtonsList { get; set; } = new List<Button>();
        public List<Control> mainMenuControlsList { get; set; } = new List<Control>();
        public Label titleLabel;
        public Button quitGameButton;
        public Button languageGameButton;
        public Button inputGameButton;
        public Control mainMenuScreenConfigControl;
        public Button mainMenuScreenConfigControl_BackButton;
        public Label mainMenuScreenConfigControlTitleNinePatchRectTitleLabel;
        public List<Control> mainMenuScreenConfigControlList { get; set; } = new List<Control>();
        public List<LangStatic> mainMenuButtonsConfigLabelList { get; set; } = new List<LangStatic>();
        public List<Button> mainMenuButtonsConfigButtonList { get; set; } = new List<Button>();
        public List<Label> mainMenuButtonsConfigButtonLabelList { get; set; } = new List<Label>();
        public Button mainMenuScreenConfigControl_SaveButton;
        public Button mainMenuScreenConfigControl_RestoreButton;
        public Control mainMenuScreenModalControl;
        public Label mainMenuScreenModalControlModalScreenControlLabel;
        public Button mainMenuScreenModalControlBackButton;
        public Label mainMenuScreenModalControlTitleNinePatchRectTitleLabel;
        public Button mainMenuScreenConfigControl_JoystickButton;
        public Button mainMenuScreenConfigControl_KeyboardButton;
        public LangStatic mainMenuScreenConfigControl_JoystickButton_Label;
        public LangStatic mainMenuScreenConfigControl_KeyboardButton_Label;
        public List<Label> mainMenuButtonsConfigKeyLabelList { get; set; } = new List<Label>();
        public List<Button> mainMenuButtonsConfigKeyButtonList { get; set; } = new List<Button>();
        public List<LangStatic> langStaticList = new List<LangStatic>();
        public List<Button> mainMenuButtonsLangKeyButtonList { get; set; } = new List<Button>();
        public List<Label> mainMenuButtonsLangKeyLabelList { get; set; } = new List<Label>();
        #endregion
        #region Variables
        public ConfigDTO configDTO { get; set; } = new ConfigDTO();
        public ConfgInputConcreteMediator confgInputConcreteMediator = new ConfgInputConcreteMediator();
        public ConfgInputConcreteColleague1 inputKeyConfgInputConcreteColleague1;
        public ConfgInputConcreteColleague2 inputKeyLabelConfgInputConcreteColleague1;
        public LanguageReceiver languageReceiver = new LanguageReceiver();
        public LanguageCommand languageCommand { get; set; }
        public List<MainMenuSubject> mainMenuSubjectList = new List<MainMenuSubject>();
        #endregion
    }
}
