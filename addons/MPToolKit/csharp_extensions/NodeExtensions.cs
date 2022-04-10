using Godot;
using System.Collections.Generic;

namespace MP.Extensions
{
    public static class NodeExtensions
    {
        public static void Disable(this Node me)
        {
            me.SetProcess(false);
            me.SetPhysicsProcess(false);
        }

        public static void Enable(this Node me)
        {
            me.SetProcess(true);
            me.SetPhysicsProcess(true);
        }

        public static List<T> GetChildren<T>(this Node me)
        {
            var array = me.GetChildren();
            List<T> result = new List<T>();
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] is T)
                    result.Add((T)array[i]);
            }
            return result;
        }

        public static bool TryGetNodeInMeAndParent<T>(this Node me, out T result, bool parentInclusive = true, bool meInclusive = false) where T : Node
        {
            result = null;

            // me
            if(meInclusive == true)
            {
                if (me is T)
                {
                    result = me as T;
                    return true;
                }
            }

            var results = me.GetChildren<T>();
            if (results.Count > 1)
            {
                GD.PrintErr($"there are two nodes of type {typeof(T)}!");
                return false;
            }

            if (results.IsEmpty() == false)
            {
                result = results[0];
                return true;
            }

            GD.Print($"there are no nodes of type {typeof(T)} in children!");
            
            //parent
            if (parentInclusive == true)
            {
                if (me.GetParent() is T)
                {
                    result = me.GetParent() as T;
                    return true;
                }
            }

            results = me.GetParent().GetChildren<T>();

            if (results.Count > 1)
            {
                GD.PrintErr($"there are two nodes in parent {me.GetParent().Name} of type {typeof(T)}!");
                return false;
            }

            if (results.IsEmpty())
            {
                GD.PrintErr($"there are no nodes in parent {me.GetParent().Name} of type {typeof(T)}!");
                return false;
            }

            result = results[0];
            return true;
        }


        public static bool TryGetNodeFromPath<T>(this Node me, NodePath nodePath, out T result) where T : Node
        {
            result = null;
            if (nodePath == null || nodePath.IsEmpty() == true)
            {
                GD.PrintErr($"{me.Name} to {typeof(T)} nodePath is empty!");
                return false;
            }

            var node = me.GetNodeOrNull(nodePath);

            if (node == null)
            {
                GD.PrintErr($"{me.Name} does not have a node under {nodePath}");
                return false;
            }

            if (typeof(T).IsAssignableFrom(node.GetType()) == false)
            {
                GD.PrintErr($"Cannot convert from type {typeof(T)} to {node.GetType()}");
                return false;
            }

            result = (T)node;
            return true;
        }
    }
}