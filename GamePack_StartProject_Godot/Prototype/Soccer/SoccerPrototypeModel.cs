using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypeModel
    {
        #region Node Name        
        public string ballName { get; set; } = "Ball";        
        #endregion                                
        #region Variables
        public MeshInstance3D ball;
        #endregion
        #region Events
        public ClientPrototypeBall client;
        public ConcreteClassMoveBall concreteClassMoveBall;
        #endregion
    }
}
