using Godot;
using MP.StateMachine;

public class RotatePlayerToInput : StateAction
{
    [Export] private float _smoothRotation = 5;

    private PlayerBody _playerBody;
    private PlayerInput _playerInput;

    public override void Init(BaseStateMachine stateMachine)
    {
        _playerBody = stateMachine.GetNodeOfType<PlayerBody>();
        _playerInput = stateMachine.GetNodeOfType<PlayerInput>();
    }

    public override void Act(float delta)
    {
        var targetRotation = _playerBody.Translation - _playerInput.RelativeMovementInput;
        _playerBody.Transform = _playerBody.Transform.LookAtSmooth(targetRotation, Vector3.Up, delta * _smoothRotation);
    }
}
