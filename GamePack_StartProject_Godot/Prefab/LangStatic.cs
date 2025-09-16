using GamePackStartProjectGodot.Scripts.Singleton;
using Godot;
using System;

public partial class LangStatic : Label
{
    [Export]
    public string langKey { get; set; }
    public void ChangeTextByKey()
    {
        this.Text = LanguageSingleton.selectedLanguage[langKey];
    }
}
