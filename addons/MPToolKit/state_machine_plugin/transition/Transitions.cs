using Godot.Collections;
using System.Collections.Generic;

namespace MP.StateMachine
{
    public sealed class Transitions : Godot.Object
    {
        private List<Transition> _transitions;

        public Transitions(List<Transition> array)
        {
            _transitions = array;
        }

        public (bool change, State newState) Check()
        {
            foreach(var transition in _transitions)
            {
                if (transition.Check() == true)
                    return (true, transition.ToState);
            }
            return (false, null);
        }
    }
}