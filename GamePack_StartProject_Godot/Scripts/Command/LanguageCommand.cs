using GamePackStartProjectGodot.Scripts.Model;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Command
{
    public abstract class LanguageCommand
    {
        protected LanguageReceiver receiver;     
        public LanguageCommand(LanguageReceiver receiver)
        {
            this.receiver = receiver;
        }
        public abstract void Execute();
    }
    public class LanguageConcreteCommand : LanguageCommand
    {        
        public LanguageConcreteCommand(LanguageReceiver receiver) :
            base(receiver)
        {
        }
        public override void Execute()
        {
            receiver.Action();
        }
    }
    public class LanguageReceiver
    {
        List<LangStatic> langStaticList = new List<LangStatic>();
        public void Action()
        {
            foreach (var langS in langStaticList)
            {
                langS.ChangeTextByKey();
            }
        }
        public LanguageReceiver LangStaticListBuilder(List<LangStatic> langStaticList)
        { 
            this.langStaticList = langStaticList;
            return this;
        }
    }    
}
