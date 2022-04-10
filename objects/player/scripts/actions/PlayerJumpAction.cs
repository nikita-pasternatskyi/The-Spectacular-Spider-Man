using Godot;
using MP.StateMachine;

public class PlayerJumpAction : StateAction
{
    [Signal] private delegate void Jumped();
    [Export] private float _jumpHeight;

    private PlayerBody _body;

    public override void Init(BaseStateMachine stateMachine)
    {
        _body = stateMachine.GetNodeOfType<PlayerBody>();
    }

    public override void Act(float delta)
    {
        EmitSignal(nameof(Jumped));
        _body.Velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _body.Gravity);
    }
}
