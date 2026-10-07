using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Soccer
{
    public class SoccerPrototypeFactory
    {
        public void CreateObjects<T>(Node control, string nodeName, out T objects) where T : Node
        {
            var nodes = new List<Node>
            {
                control.GetNode<Node>(nodeName)
            };
            FindObjects<T>(nodes, out objects);
        }
        private void FindObjects<T>(List<Node> nodes, out T objects) where T : Node
        {
            if (nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    var obj = node as T;
                    if (obj is not null)
                    {
                        if (node.GetType() == typeof(T))
                        {
                            objects = obj;
                            return;
                        }
                    }
                    FindObjects<T>(node.GetChildren().ToList(), out objects);
                }
            }
            objects = null;
            return;
        }
    }
}
