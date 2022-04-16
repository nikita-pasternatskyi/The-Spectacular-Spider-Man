using Godot;
using MP.Extensions;
using MP.StateMachine;

public enum MoveConstrainType
{
    Hard, Soft
}

public class PlayerMovementAction : StateAction
{
    [Export] private MoveConstrainType _moveConstrainType;
    [Export] private float _maxSpeed = 2;
    [Export] private float _acceleration;

    public float CurrentSpeed { get; private set; }

    protected PlayerInput PlayerInput { get; private set; }
    protected PlayerBody PlayerBody { get; private set; }

    public override void Init(BaseStateMachine stateMachine)
    {
        PlayerInput = stateMachine.GetNodeOfType<PlayerInput>();
        PlayerBody = stateMachine.GetNodeOfType<PlayerBody>();
    }

    public sealed override void Act(float delta)
    {
        var onSlope = PlayerBody.OnSlope;
        var grounded = PlayerBody.Grounded;
        var groundNormal = PlayerBody.GroundNormal;

        PlayerBody.UseGravity = !onSlope;

        var curSpeed = GetInput().ProjectOnPlane(groundNormal) * _acceleration;
        PlayerBody.AddForce(curSpeed * delta);

        ConstrainSpeed(onSlope, grounded);

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
                    Vector3 forceToApply = (flatVelocity.Normalized() * _maxSpeed - PlayerBody.Velocity) * GetPhysicsProcessDeltaTime();
                    forceToApply.y = 0;
                    PlayerBody.AddForce(forceToApply);
                    break;
            }
        }
    }
}