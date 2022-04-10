using Godot;
using MP.Extensions;
using MP.StateMachine;

public enum MoveConstrainType
{
    Hard, Soft
}

public enum ActionPlace : byte
{
    Enter, Exit, Update, FixedUpdate
}

public abstract class StateAction : Node
{
    public virtual void Init(BaseStateMachine stateMachine)
    {

    }

    public abstract void Act();
}

public class PlayerJumpState : StateAction
{
    [Signal] private delegate void Jumped();
    [Export] private float _jumpHeight;

    private PlayerBody _body;

    public override void Init(BaseStateMachine stateMachine)
    {
        _body = stateMachine.GetNodeOfType<PlayerBody>();
    }

    public override void Act()
    {
        EmitSignal(nameof(Jumped));

        _body.Velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _body.Gravity);
    }
}

public class PlayerMovementState : State
{
    [Export] private MoveConstrainType _moveConstrainType;
    [Export] private float _enterDamp;
    [Export] private float _exitDamp;
    [Export] private float _smoothRotation = 5;
    [Export] private float _maxSpeed = 2;
    [Export] private float _acceleration;

    public float CurrentSpeed { get; private set; }

    protected PlayerInput PlayerInput { get; private set; }
    protected PlayerBody PlayerBody { get; private set; }

    protected sealed override void OnInit()
    {
        PlayerInput = stateMachine.GetNodeOfType<PlayerInput>();
        PlayerBody = stateMachine.GetNodeOfType<PlayerBody>();
    }

    public sealed override void PhysicsProcess(float delta)
    {
        var onSlope = PlayerBody.OnSlope;
        var grounded = PlayerBody.Grounded;
        var groundNormal = PlayerBody.GroundNormal;

        PlayerBody.UseGravity = !onSlope;

        var curSpeed = GetInput().ProjectOnPlane(groundNormal) * _acceleration;
        PlayerBody.AddForce(curSpeed * delta);

        ConstrainSpeed(onSlope, grounded);

        var targetRotation = PlayerBody.Translation - GetInput();
        PlayerBody.Transform = PlayerBody.Transform.LookAtSmooth(targetRotation, Vector3.Up, delta * _smoothRotation);
    }

    protected virtual Vector3 GetInput()
    {
        return PlayerInput.RelativeMovementInput;
    }

    private void ConstrainSpeed(bool OnSlope, bool Grounded)
    {
        if (OnSlope)
        {
            if (PlayerBody.Velocity.LengthSquared() > _maxSpeed * _maxSpeed)
            {
                PlayerBody.Velocity = PlayerBody.Velocity.Normalized() * _maxSpeed;
                return;
            }
        }

        Vector3 flatVelocity = new Vector3(PlayerBody.Velocity.x, 0, PlayerBody.Velocity.z);
        CurrentSpeed = flatVelocity.Length() / _maxSpeed;
        if (flatVelocity.LengthSquared() > _maxSpeed * _maxSpeed)
        {
            switch (_moveConstrainType)
            {
                case MoveConstrainType.Hard:
                    Vector3 limitedVelocity = flatVelocity.Normalized() * _maxSpeed;
                    limitedVelocity.y = PlayerBody.Velocity.y;
                    PlayerBody.Velocity = limitedVelocity;
                    break;
                case MoveConstrainType.Soft:
                    Vector3 forceToApply = flatVelocity.Normalized() * _maxSpeed - PlayerBody.Velocity;
                    forceToApply.y = 0;
                    PlayerBody.AddForce(forceToApply);
                    break;
            }
        }
    }

    protected sealed override void OnEnter()
    {
        PlayerBody.Damp = _enterDamp;
        MoveEnter();
    }

    protected sealed override void OnExit()
    {
        PlayerBody.Damp = _exitDamp;
        MoveExit();
    }

    protected virtual void MoveEnter() { }
    protected virtual void MoveExit() { }
}