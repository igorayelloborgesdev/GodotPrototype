using GamePackStartProjectGodot.Scripts.Mediator;
using GamePackStartProjectGodot.Scripts.Model;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MainClient
{    
    public void CreateObjects<T>(Control control, string nodeName, List<T> objectsList) where T : Node
    {        
        var nodes = new List<Node>
        {
            control.GetNode<Node>(nodeName)
        };        
        FindObjects<T>(nodes, objectsList);
    }
    private void FindObjects<T>(List<Node> nodes, List<T> objects) where T : Node
    {
        if (nodes.Count > 0)
        {
            foreach (var node in nodes)
            {                
                var obj = node as T;
                if (obj is not null)
                {                    
                    if(node.GetType() == typeof(T))
                        objects.Add(obj); 
                }                    
                FindObjects<T>(node.GetChildren().ToList(), objects);
            }
        }
        return;
    }
    public void CreateObjects<T>(Control control, string nodeName, out T objects) where T : Node
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
    public void CreateObjects<T>(Control control, string nodeNameRoot, string[] nodeArrayName, List<T> objectsList, bool isType) where T : Node
    {
        var nodes = new List<Node>();
        foreach (var item in nodeArrayName)
        {
            var nodeName = nodeNameRoot + "/" + item + (isType ? "/" + typeof(T).Name : "");
            nodes.Add(control.GetNode<Node>(nodeName));
        }
        FindObjects<T>(nodes, objectsList);        
    }    
}