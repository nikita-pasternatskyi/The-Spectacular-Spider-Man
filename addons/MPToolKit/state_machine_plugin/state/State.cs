using Godot;
using MP.Extensions;

namespace MP.StateMachine
{
    public abstract class State : Node
    {
        [Signal] private delegate void StateEntered();
        [Signal] private delegate void StateExit();
        protected BaseStateMachine stateMachine { get; private set; }

        public sealed override void _Ready()
        {
            this.Disable();
        }

        public sealed override void _PhysicsProcess(float delta)
        {
        }

        public sealed override void _Process(float delta)
        {
        }

        public void Init(BaseStateMachine baseStateMachine)
        {
            stateMachine = baseStateMachine;
            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        public virtual void Process(float delta)
        { }

        public virtual void PhysicsProcess(float delta)
         { }

        public void Enter()
        {
            EmitSignal(nameof(StateEntered));
            OnEnter();
        }

        public void Exit()
        {
            EmitSignal(nameof(StateExit));
            OnExit();
        }

        protected virtual void OnEnter()
        {

        }

        protected virtual void OnExit()
        {

        }
    }
}