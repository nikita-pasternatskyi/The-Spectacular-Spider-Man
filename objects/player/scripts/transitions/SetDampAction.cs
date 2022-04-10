using Godot;
using MP.StateMachine;

public class SetDampAction : StateAction
{
    [Export] private float _damp;
    private PlayerBody _playerBody;

    public override void Init(BaseStateMachine stateMachine)
    {
        _playerBody = stateMachine.GetNodeOfType<PlayerBody>();
    }

    public override void Act(float delta)
    {
        _playerBody.Damp = _damp;
    }
}