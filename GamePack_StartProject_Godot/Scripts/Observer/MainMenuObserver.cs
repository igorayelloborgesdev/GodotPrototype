using GamePackStartProjectGodot.Scripts.Singleton;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Observer
{
    public abstract class MainMenuSubject
    {
        private MainMenuObserver observerTitle = null;
        public void Attach(MainMenuObserver observer)
        {
            observerTitle = observer;
        }        
        public void Notify()
        {
            if (observerTitle is not null)
                observerTitle.Update();
        }
        public void NotifyByKey()
        {
            if (observerTitle is not null)
                observerTitle.UpdateByKey();
        }
    }
    public class MainMenuSubjectConcreteSubject : MainMenuSubject
    {
        private string subjectState;        
        public string SubjectState
        {
            get { return subjectState; }
            set { subjectState = value; }
        }
        private string keyState;
        public string KeyState
        {
            get { return keyState; }
            set { keyState = value; }
        }
    }
    public abstract class MainMenuObserver
    {
        public abstract void Update();
        public abstract void UpdateByKey();
    }
    public class MainMenuConcreteObserver : MainMenuObserver
    {
        private Node node = null;
        private MainMenuSubjectConcreteSubject mainMenuSubjectConcreteSubject = null;
        public MainMenuConcreteObserver(Node node, MainMenuSubjectConcreteSubject mainMenuSubjectConcreteSubject)
        {
            this.node = node;
            this.mainMenuSubjectConcreteSubject = mainMenuSubjectConcreteSubject;
        }
        public override void Update()
        {
            var obj = node as Label;
            if (obj is not null && this.mainMenuSubjectConcreteSubject.SubjectState is not null)
                obj.Text = this.mainMenuSubjectConcreteSubject.SubjectState.ToString();                
        }
        public override void UpdateByKey()
        {
            var obj = node as Label;
            if (obj is not null && this.mainMenuSubjectConcreteSubject.SubjectState is not null)
                obj.Text = LanguageSingleton.selectedLanguage[this.mainMenuSubjectConcreteSubject.KeyState.ToString()];
        }
    }
}
