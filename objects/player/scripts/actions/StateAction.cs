using Godot;
using MP.StateMachine;

public abstract class StateAction : Node
{
    [Export] public bool OnEnter;
    [Export] public bool OnExit;
    [Export] public bool OnUpdate;
    [Export] public bool OnFixedUpdate;

    public virtual void Init(BaseStateMachine stateMachine)
    {

    }

    public abstract void Act(float delta);

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}

