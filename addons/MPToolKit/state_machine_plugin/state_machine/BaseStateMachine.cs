using Godot;
using System.Collections.Generic;
using MP.Extensions;

namespace MP.StateMachine
{
    public abstract class BaseStateMachine : Node, IStateMachine
    {
        [Export] private NodePath _defaultStatePath;
        private State _defaultState;
        private State _currentState;
        private Transitions _currentStateTransitions;
        private Dictionary<State, Transitions> _states;
        private Dictionary<System.Type, Node> _nodes;

        public T GetNodeOfType<T>() where T:Node
        {
            if(_nodes.TryGetValue(typeof(T), out Node value))
            {
                return (T)value;
            }

            if(this.TryGetNodeInMeAndParent<T> (out T res) == false)
            {
                GD.PrintErr($"No node of type {typeof(T)} was found!");
                return null;
            }
            _nodes.Add(typeof(T), res);
            return res;
        }

        public sealed override void _Ready()
        {
            OnInit();

            _nodes = new Dictionary<System.Type, Node>();

            _states = new Dictionary<State, Transitions>();
            this.TryGetNodeFromPath(_defaultStatePath, out _defaultState);

            if ((_defaultState is State) == false)
                throw new System.InvalidCastException(nameof(_defaultStatePath));

            foreach (var child in this.GetChildren<State>())
            {
                List<Transition> stateTransitions = new List<Transition>();

                var children = new List<Transition>();

                if (child.FindNode("Transitions") != null)
                    children = child.GetNode("Transitions").GetChildren<Transition>();
                else
                    children = child.GetChildren<Transition>();

                if(children.IsEmpty())
                {
                    GD.Print($"{child.Name} State has no transitions!");
                }

                foreach (var transition in children)
                {
                    stateTransitions.Add(transition);
                }

                child.Init(this);
                _states.Add(child, new Transitions(stateTransitions));
            }
            OnReady();
            ChangeState(_defaultState);
        }

        public sealed override void _Process(float delta)
        {
            var currentTransitionState = _currentStateTransitions.Check();
            if (currentTransitionState.change == true)
                ChangeState(currentTransitionState.newState);
            _currentState.Process(delta);
            OnProcess(delta);
        }
        public sealed override void _PhysicsProcess(float delta)
        {
            if(_currentState == null)
            {
                GD.PrintErr("Current state is null!");
            }
            _currentState.PhysicsProcess(delta);
            OnPhysicsProcess(delta);
        }

        private void ChangeState(State newState)
        {
            if (_states.ContainsKey(newState) == false)
            {
                GD.Print($"{newState.Name} was not added to the dictionary! Therefore it is not this stateMachine state!");
                return;
            }

            _currentState?.Exit();
            _currentState = newState;
            _currentStateTransitions = _states[newState];
            _currentState.Enter();
        }

        protected virtual void OnInit() { }
        protected virtual void OnReady() { }
        protected virtual void OnProcess(float delta) { }
        protected virtual void OnPhysicsProcess(float delta) { }

    }
}