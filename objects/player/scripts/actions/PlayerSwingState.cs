using Godot;
using MP.StateMachine;
using MP.Extensions;
using System;

public class RopePoint
{
    public Vector3 Position, PrevPosition;
    public bool Locked;
}

public class Rope
{
    public RopePoint PointA, PointB;
    public float Length;
}

[System.Serializable]
public class PlayerSwingState : StateAction
{
    [Signal] private delegate Vector3 Attached();
    [Export] private float _rotationSpeed;
    [Export] private bool _constrainStickMinLength;
    [Export] private int _numIterations;
    [Export] private Vector3 _perfectPoint;
    [Export] private float _damp;
    [Export] private Vector2 _swingSpeed;
    [Export] private float _gravity;

    public float ArcAngle { get; private set; }

    private float _deltaTime;
    private Spatial _transform;
    private Vector3 _perfectPointPosition;
    private Vector3 _movementDirection;
    private RopePoint[] _swingPoints;
    private Rope[] _webRopes;
    private PlayerBody _playerBody;
    private PlayerInput _input;

    private void CalculatePerfectPoint(Vector3 position)
    {
        _perfectPointPosition = _perfectPoint;
        _perfectPointPosition += position;
    }

    public override void Init(BaseStateMachine baseStateMachine)
    {
        _input = baseStateMachine.GetNodeOfType<PlayerInput>();
        _playerBody = baseStateMachine.GetNodeOfType<PlayerBody>();
        _swingPoints = new RopePoint[2];
        _webRopes = new Rope[1];
        _transform = baseStateMachine.GetParent() as Spatial;
    }

    private void Simulate(float deltaTime)
    {
        foreach (RopePoint point in _swingPoints)
        {
            if (point.Locked == false)
            {
                var preSimPos = point.Position;
                var velocity = point.Position - point.PrevPosition;
                velocity -= (velocity * _damp) * deltaTime;
                point.Position += velocity;

                var upVector = (point.Position - _swingPoints[0].Position).Normalized();
                Rotate(_swingPoints[0].Position - point.Position);

                var angle = upVector.Angle(Vector3.Down);
                angle = Mathf.Rad2Deg(angle);
                GD.Print(angle);
                var moveDirection = _movementDirection;

                var control = Mathf.Lerp(1, 0, angle / 90);
                var gravityControl = Mathf.Lerp(0, 1, angle / 90);
                ArcAngle = angle;

                moveDirection.z *= control;
                moveDirection.z *= _swingSpeed.y * deltaTime;
                moveDirection.x *= _swingSpeed.x * deltaTime;

                var gravity = Vector3.Down * (gravityControl * _gravity * (deltaTime * _deltaTime));

                point.Position += moveDirection;
                point.Position += gravity;

                point.PrevPosition = preSimPos;
            }
        }

        for (int i = 0; i < _numIterations; i++)
        {

            foreach (var rope in _webRopes)
            {
                Vector3 ropeCentre = (rope.PointA.Position + rope.PointB.Position) / 2;
                Vector3 ropeDirection = (rope.PointA.Position - rope.PointB.Position).Normalized();
                float length = (rope.PointA.Position - rope.PointB.Position).Length();

                if (length > rope.Length || _constrainStickMinLength)
                {
                    if (rope.PointA.Locked == false)
                    {
                        rope.PointA.Position = ropeCentre + ropeDirection * rope.Length / 2;
                    }

                    if (rope.PointB.Locked == false)
                    {
                        rope.PointB.Position = ropeCentre - ropeDirection * rope.Length / 2;
                    }
                }

            }
        }
    }

    private RopePoint FindSwingPoint()
    {
        return new RopePoint { Locked = true, Position = _perfectPointPosition };
    }

    private RopePoint CreatePlayerPoint()
    {
        var curPos = _transform.Translation + _playerBody.Velocity * _deltaTime;
        return new RopePoint { Locked = false, Position = curPos, PrevPosition = _transform.Translation };
    }

    public override void OnStateEnter()
    {
        CalculatePerfectPoint(_transform.Translation);
        _playerBody.UseGravity = false;
        _swingPoints[0] = FindSwingPoint();
        _swingPoints[1] = CreatePlayerPoint();
        EmitSignal(nameof(Attached), _swingPoints[0].Position);
        _webRopes[0] = new Rope
        {
            PointA = _swingPoints[0],
            PointB = _swingPoints[1],
            Length = _swingPoints[1].Position.DistanceTo(_swingPoints[0].Position)
        };
    }

    public override void Act(float delta)
    {
        if (delta == -1)
            return;
        Simulate(delta);
        _deltaTime = delta;
        _movementDirection = _input.RelativeMovementInput;
        var force = (_swingPoints[1].Position - _swingPoints[1].PrevPosition) / _deltaTime;
        _playerBody.Velocity = force;
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
