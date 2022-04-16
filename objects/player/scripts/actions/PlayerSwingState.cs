using Godot;
using MP.StateMachine;
using MP.Extensions;
using System;

[System.Serializable]
public class PlayerSwingState : StateAction
{
    [Signal] private delegate Vector3 Attached();
    [Export] private Vector2 _swingSpeed;
    [Export] private float _rotationSpeed;
    [Export] private float _damp;
    [Export] private float _spring;
    [Export] private float _gravity;
    [Export] private float _antiBuildingForce;
    [Export] private float _crispTurn;
    [Export] private float _enterVelocityClamp;
    [Export] private float _boostPower;

    private Godot.Collections.Array _exclusionArray;

    [Export] private Curve _jumpCurve;
    [Export] private Curve _controlCurve;
    [Export] private float _jumpHeight;
    public float ArcAngle { get; private set; }

    private float _deltaTime;
    private WebShooter _webShooter;
    private PlayerBody _playerBody;
    private PlayerInput _input;
    private SwingPoint _swingPoint;
    private Vector3 _velocity;

    public override void Init(BaseStateMachine baseStateMachine)
    {
        _exclusionArray = new Godot.Collections.Array(this);
        _input = baseStateMachine.GetNodeOfType<PlayerInput>();
        _playerBody = baseStateMachine.GetNodeOfType<PlayerBody>();
        _webShooter = baseStateMachine.GetNodeOfType<WebShooter>();
    }

    private void CorrectVelocity(Vector3 translation)
    {
        var currentDistance = translation.DistanceTo(_swingPoint.Position);
        var distanceError = Math.Abs(currentDistance - _swingPoint.Distance);
        var changeDirection = Vector3.Zero;

        if (currentDistance > _swingPoint.Distance)
            changeDirection = _swingPoint.Position - translation;
        else if (currentDistance < _swingPoint.Distance)
            changeDirection = translation - _swingPoint.Position;
        if (changeDirection != Vector3.Zero)
        {
            _velocity += changeDirection.Normalized() * distanceError;
            _swingPoint.Position -= changeDirection.Normalized() * distanceError * 1 / _spring;
        }
    }

    public override void OnStateExit()
    {
        var jumpForce = Mathf.Sqrt(_jumpHeight * -2 * _playerBody.Gravity);
        _playerBody.Velocity.y = _jumpCurve.Interpolate(ArcAngle / 90) * jumpForce;
        _playerBody.UseGravity = true;
    }

    public override void OnStateEnter()
    {
        _playerBody.UseGravity = false;
        _deltaTime = GetProcessDeltaTime();
        _velocity = _playerBody.Velocity.NormalizedClamp(50) * _deltaTime;
        AntiBuildingForce(_playerBody.Velocity.y * _playerBody.Velocity.y);
        var position = _webShooter.CurrentSwingPoint.Position;
        _swingPoint = new SwingPoint(position, _playerBody.Translation.DistanceTo(position), -1);
        _swingPoint.Normal = _webShooter.CurrentSwingPoint.Normal;
        EmitSignal(nameof(Attached), _swingPoint.Position);
    }

    public override void Act(float delta)
    {
        if (delta == -1)
            return;
        _deltaTime = delta;
        Simulate();
        _playerBody.Velocity = _velocity/_deltaTime;
    }

    private void Simulate()
    {
        var parent = _playerBody;
        var translation = parent.Translation;
        var swingPointPosition = _swingPoint.Position;

        var upVector = (translation - swingPointPosition).Normalized();
        Rotate(swingPointPosition - translation);

        var angle = upVector.Angle(Vector3.Down);
        angle = Mathf.Rad2Deg(angle);
        var moveDirection = _input.RelativeMovementInput;

        var control = _controlCurve.Interpolate(Mathf.Lerp(1, 0, angle / 90));
        var gravityControl = Mathf.Lerp(0, 1, angle / 90);
        ArcAngle = angle;

        moveDirection.z *= control;
        moveDirection.z *= _swingSpeed.y * _deltaTime;
        moveDirection.x *= _swingSpeed.x * _deltaTime;

        var gravity = Vector3.Down * (gravityControl * _gravity * (_deltaTime * _deltaTime));

        _velocity -= _velocity * (_damp * _deltaTime);
        _velocity += gravity;
        _velocity += moveDirection;
        _velocity += AntiBuildingForce(angle/90);

        if(Input.IsActionJustPressed(InputBindings.BOOST))
        {
            _velocity += _playerBody.Transform.basis.z * (_boostPower * _deltaTime);
        }

        _velocity = _velocity.ProjectOnPlane(upVector);

        _swingPoint.Position = _swingPoint.StartPosition;

        CorrectVelocity(translation);
        CollisionDetection();
    }

    private Vector3 AntiBuildingForce(float ratio = -1)
    {
        var projected = _swingPoint.Position;
        projected.y = _playerBody.Translation.y;

        var direction = projected.DirectionTo(_playerBody.Translation);

        var input = _input.RelativeMovementInput;
        input.x *= -1;
        var vec2 = new Vector2(-_swingPoint.Normal.x, _swingPoint.Normal.z);
        var inputRatio = vec2.Dot(_input.RelativeMovementInputVec2) * -1;
        inputRatio = Mathf.Round(inputRatio);
        inputRatio = inputRatio == -1 ? 0 : 1;
        GD.Print(inputRatio);
        if (input == Vector3.Zero)
            inputRatio = 0;

        if(ratio > 0)
            direction *= ratio;

        direction *= _antiBuildingForce * _deltaTime;
        direction *= inputRatio;
        return direction;
    }

    private void CollisionDetection()
    {
        var world = _playerBody.GetWorld().DirectSpaceState;

        var translation = _playerBody.Translation;
        var forwardInt = world.IntersectRay(translation, translation + _playerBody.Transform.basis.z, _exclusionArray);

        if (forwardInt.Count != 0)
        {
            var normal = (Vector3)(forwardInt["normal"]);
            _velocity = _velocity.ProjectOnPlane(normal);
        }
    }

    private void Rotate(Vector3 direction)
    {
        var parent = _playerBody;
        var transform = parent.Transform;

        transform.basis.y = direction.Normalized();
        transform.basis.z = _input.RelativeMovementInput.Normalized();
        if (_input.RelativeMovementInput == Vector3.Zero)
        {
            transform.basis.z = parent.Transform.basis.x.Cross(parent.Transform.basis.y).Normalized();
        }
        transform.basis.x = transform.basis.y.Cross(transform.basis.z).Normalized();
        transform.basis = transform.basis.Orthonormalized();

        parent.Transform = parent.Transform.InterpolateWith(transform, _deltaTime * _rotationSpeed);
    }

}
