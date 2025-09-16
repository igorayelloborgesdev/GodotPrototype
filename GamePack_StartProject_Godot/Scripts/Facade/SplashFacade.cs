using GamePackStartProjectGodot.Scripts.Singleton;
using GamePackStartProjectGodot.Scripts.Util;
using Godot;
using System;

public class SplashFacade
{
    public void Init()
    {
        InitConfigDefaultInputs();
        LoadConfig();
        LoadLanguage();
    }
    private void LoadConfig()
    {
        ConfigSingleton.saveConfigDTO = SaveLoad.LoadConfig<SaveConfigDTO>();
    }    
    public void InitConfigDefaultInputs()
    {
        ConfigDefaultInputs.Init();
    }
    public void LoadLanguage()
    {
        LanguageSingleton.language = SaveLoad.LoadXML("text", "id");   
        LanguageSingleton.selectedLanguage = LanguageSingleton.language[ConfigSingleton.saveConfigDTO is null? 0 : ConfigSingleton.saveConfigDTO.languageId];
    }

}
