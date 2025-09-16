using GamePackStartProjectGodot.Scripts.DTO;
using GamePackStartProjectGodot.Scripts.Model;
using GamePackStartProjectGodot.Scripts.Observer;
using GamePackStartProjectGodot.Scripts.Singleton;
using GamePackStartProjectGodot.Scripts.Util;
using Godot;
using System;
using System.Collections.Generic;

public abstract class MainMenuButtonComponent
{
    public abstract void Operation<T>(int id) where T : Node;
    public abstract void Operation<T>() where T : Node;
    public abstract MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList) where T : Node;
    public abstract MainMenuButtonComponent SetObserverBuilder<T>(T observer) where T : Node;
    public abstract MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel);
    public abstract MainMenuSubject GetMainMenuSubject();    
}
public class MainMenuButtonConcreteDecorator : MainMenuButtonComponent
{
    protected List<Node> observerList = new List<Node>();
    protected Node observerTitle = new Node();
    protected MainMenuSubjectConcreteSubject mainMenuSubjectConcreteSubject = new MainMenuSubjectConcreteSubject();
    protected Dictionary<int, string> menuLanguage = new Dictionary<int, string>() 
    {
        { 0, "main"},
        { 1, "tutorial"},
        { 2, "config"},
        { 3, "load"},
        { 4, "credits"},
        { 5, "quit"}
    };
    public override void Operation<T>(int id)
    {        
        foreach (var node in observerList) 
        {
            var obj = node as Control;   
            if (obj != null)
                obj.Hide();            
        }
        var objShow = observerList[id] as Control;
        objShow.Show(); 
        mainMenuSubjectConcreteSubject.SubjectState = LanguageSingleton.selectedLanguage[menuLanguage[id]];
        mainMenuSubjectConcreteSubject.KeyState = menuLanguage[id];
        mainMenuSubjectConcreteSubject.Notify();        
    }
    public override void Operation<T>(){ }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {
        this.observerList.AddRange(observerList);
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {
        this.observerTitle = observer;        
        mainMenuSubjectConcreteSubject.Attach(new MainMenuConcreteObserver(this.observerTitle, mainMenuSubjectConcreteSubject));
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel) { return this; }

    public override MainMenuSubject GetMainMenuSubject()
    { 
        return mainMenuSubjectConcreteSubject; 
    }
}

public class MainMenuQuitButtonConcreteDecorator : MainMenuButtonComponent
{    
    protected Node observerTitle = new Node();
    protected Control mainMenuSubject;
    public override void Operation<T>(int id){}
    public override void Operation<T>() {
        this.observerTitle.GetTree().Quit();
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {        
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {
        this.observerTitle = observer;
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel) { return this; }
    public override MainMenuSubject GetMainMenuSubject()
    {
        return null;
    }
}

public class ConfigButtonConcreteDecorator : MainMenuButtonComponent
{
    protected List<Node> observerList = new List<Node>();
    protected Node observer;
    protected Node observerTitle;
    protected MainMenuSubjectConcreteSubject mainMenuSubjectConcreteSubject = new MainMenuSubjectConcreteSubject();
    protected Dictionary<int, string> menuLanguage = new Dictionary<int, string>()
    {
        { 0, "input"},
        { 1, "language"}        
    };
    public override void Operation<T>(int id)
    {
        foreach (var node in observerList)
        {
            var objControl = node as Control;
            if (objControl != null)
                objControl.Hide();
        }
        var objShow = observerList[id] as Control;
        objShow.Show();
        var obj = observer as Control;
        if (obj != null)
            obj.Show();
        mainMenuSubjectConcreteSubject.SubjectState = LanguageSingleton.selectedLanguage[menuLanguage[id]];
        mainMenuSubjectConcreteSubject.KeyState = menuLanguage[id];
        mainMenuSubjectConcreteSubject.Notify();
    }
    public override void Operation<T>() 
    {
        var obj = observer as Control;
        if (obj != null)
            obj.Hide();
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {
        this.observerList.AddRange(observerList);
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {        
        var objLabel = observer as Label;
        if (objLabel != null)
        {
            this.observerTitle = observer;
            mainMenuSubjectConcreteSubject.Attach(new MainMenuConcreteObserver(this.observerTitle, mainMenuSubjectConcreteSubject));
            return this;
        }
        var objControl = observer as Control;
        if (objControl != null)
            this.observer = observer;
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel) { return this; }
    public override MainMenuSubject GetMainMenuSubject()
    {
        return mainMenuSubjectConcreteSubject;
    }
}

public class ConfigButtonSaveConfigConcreteDecorator : MainMenuButtonComponent
{
    protected MainButtonModel mainButtonModel;
    protected Dictionary<int, string> menuLanguage = new Dictionary<int, string>()
    {
        { 0, "configSaved"},
        { 1, "configSavedError"},
        { 2, "saveConfig"}    
    };
    public override void Operation<T>(int id)
    {
        
    }
    public override void Operation<T>()
    {
        if (SaveLoad.SaveConfig<SaveConfigDTO>(ConfigSingleton.saveConfigDTO))        
            mainButtonModel.mainMenuScreenModalControlModalScreenControlLabel.Text = LanguageSingleton.selectedLanguage[menuLanguage[0]];        
        else        
            mainButtonModel.mainMenuScreenModalControlModalScreenControlLabel.Text = LanguageSingleton.selectedLanguage[menuLanguage[1]];        
        mainButtonModel.mainMenuScreenModalControlTitleNinePatchRectTitleLabel.Text = LanguageSingleton.selectedLanguage[menuLanguage[2]];
        mainButtonModel.mainMenuScreenModalControl.Show();
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {        
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {        
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel)
    {
        this.mainButtonModel = mainButtonModel;
        mainButtonModel.mainMenuScreenModalControl.Hide();
        return this;
    }
    public override MainMenuSubject GetMainMenuSubject()
    {
        return null;
    }
}
public class ConfigButtonRestoreConfigConcreteDecorator : MainMenuButtonComponent
{
    protected MainButtonModel mainButtonModel;
    protected string colorNormal = "000000";
    protected string colorSelected = "e7a706";
    protected Dictionary<int, string> menuLanguage = new Dictionary<int, string>()
    {
        { 0, "restoreConfig"},
        { 1, "restoredConfig"}      
    };
    public override void Operation<T>(int id)
    {

    }
    public override void Operation<T>()
    {
        if(ConfigSingleton.saveConfigDTO is null)
            ConfigSingleton.saveConfigDTO = new SaveConfigDTO();
        ConfigSingleton.saveConfigDTO.keysControlArray.Clear();
        ConfigSingleton.saveConfigDTO.keysControlArray.AddRange(ConfigDefaultInputs.keysControlArray);
        ConfigSingleton.saveConfigDTO.keyboardJoystick = 0;
        mainButtonModel.configDTO.idKeyInput = 0;
        for (int i = 0; i < ConfigSingleton.saveConfigDTO.keysControlArray.Count; i++)
        {
            mainButtonModel.inputKeyConfgInputConcreteColleague1.Send(ConfigSingleton.saveConfigDTO.keysControlArray[i], i);
        }
        if(ConfigSingleton.saveConfigDTO.languageId == 0)
            SaveLoad.DeleteConfigFile();    
        else
            SaveLoad.SaveConfig<SaveConfigDTO>(ConfigSingleton.saveConfigDTO);

        mainButtonModel.mainMenuScreenModalControlTitleNinePatchRectTitleLabel.Text = LanguageSingleton.selectedLanguage[menuLanguage[0]];
        mainButtonModel.mainMenuScreenModalControlModalScreenControlLabel.Text = LanguageSingleton.selectedLanguage[menuLanguage[1]];
        mainButtonModel.mainMenuScreenModalControl.Show();
        (mainButtonModel.mainMenuButtonsConfigKeyLabelList[0] as Godot.Label).AddThemeColorOverride("font_color", new Color(colorSelected));
        (mainButtonModel.mainMenuButtonsConfigKeyLabelList[1] as Godot.Label).AddThemeColorOverride("font_color", new Color(colorNormal));
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel) 
    { 
        this.mainButtonModel = mainButtonModel;
        return this; 
    }
    public override MainMenuSubject GetMainMenuSubject()
    {
        return null;
    }
}
public class ConfigButtonModalConfigConcreteDecorator : MainMenuButtonComponent
{
    protected MainButtonModel mainButtonModel;
    public override void Operation<T>(int id)
    {

    }
    public override void Operation<T>()
    {
        mainButtonModel.mainMenuScreenModalControl.Hide();
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(List<T> observerList)
    {
        return this;
    }
    public override MainMenuButtonComponent SetObserverBuilder<T>(T observer)
    {
        return this;
    }
    public override MainMenuButtonComponent SetMainButtonModelBuilder(MainButtonModel mainButtonModel)
    {
        this.mainButtonModel = mainButtonModel;
        return this;
    }
    public override MainMenuSubject GetMainMenuSubject()
    {
        return null;
    }
}
