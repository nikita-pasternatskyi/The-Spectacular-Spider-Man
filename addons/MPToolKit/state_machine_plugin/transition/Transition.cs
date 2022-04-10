using Godot;

namespace MP.StateMachine
{
    public abstract class Transition : Node
    {
        [Export] private NodePath _toStatePath;

        public State ToState { get; private set; }

        public override void _Ready()
        {
            ToState = GetNode<State>(_toStatePath);
            OnReady();
        }

        protected virtual void OnReady() { }

        public abstract bool Check();
    }
}