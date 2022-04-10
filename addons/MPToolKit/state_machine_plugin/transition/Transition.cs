using Godot;
using MP.Extensions;

namespace MP.StateMachine
{
    public abstract class Transition : Node
    {
        [Export] private NodePath _toStatePath;

        public State ToState { get; private set; }

        public sealed override void _Ready()
        {
            this.Disable();
            ToState = GetNode<State>(_toStatePath);
            Ready();
        }

        protected virtual void Ready() { }

        public abstract bool Check();
    }
}