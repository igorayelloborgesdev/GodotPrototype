using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Scripts.Singleton
{
    public class LanguageSingleton
    {
        public static List<Dictionary<string, string>> language;
        public static Dictionary<string, string> selectedLanguage;
    }
}
