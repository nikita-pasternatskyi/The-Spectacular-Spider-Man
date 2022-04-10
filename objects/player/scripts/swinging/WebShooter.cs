using Godot;
using System.Collections.Generic;
using MP.Extensions;
using MP.StateMachine;

public class WebShooter : Spatial
{
    [Export] private NodePath _pathToStateMachine;
    [Export] private int _anglesToRotateDown;
    [Export] private int _angleRadius;
    [Export] private int _angleBetweenRays;
    [Export] private float _maxDistance;
    [Export] private Vector3 _perfectPointD;

    private RayCast _rayCast;
    private List<SwingPoint> _swingPoints;
    private Vector3 _initialRotation;
    private Vector3[] _rayCastPath;

    private Vector3 _perfectPoint;

    public SwingPoint CurrentSwingPoint { get; private set; }

    public override void _Ready()
    {
        _rayCast = new RayCast();

        _swingPoints = new List<SwingPoint>();

        var rayCount = _angleRadius / _angleBetweenRays + 1;
        _rayCastPath = new Vector3[rayCount];

        _initialRotation = Rotation;

        AddChild(_rayCast);

        CreateRayPath(rayCount);
    }

    private void CreateRayPath(int rayCount)
    {
        var parent = GetParent<KinematicBody>();
        for (int i = 0; i < rayCount; i++)
        {
            var angle = _angleBetweenRays * (i - rayCount / 2.0f);
            _rayCastPath[i] = parent.Transform.basis.z.Rotated(Vector3.Up, Mathf.Deg2Rad(angle)) * _maxDistance;
        }
    }

    public override void _PhysicsProcess(float delta) => _swingPoints.Clear();

    private void ScanWorld()
    {
        CheckFOV();
        TryRotateSwingPoints(_swingPoints);
    }

    private void TryRotateSwingPoints(List<SwingPoint> listToCheck)
    {
        if (listToCheck.IsEmpty())
        {
            for (int i = 0; i < _anglesToRotateDown; i++)
            {
                RotateX(Mathf.Deg2Rad(1));
                CheckFOV();
                if (listToCheck.IsEmpty() == true)
                {
                    continue;
                }
            }
            Rotation = _initialRotation;
        }
    }

    private void CheckFOV()
    {
        for (int i = 0; i < _rayCastPath.Length; i++)
        {
            CheckRay(_rayCastPath[i], _swingPoints);
        }
    }

    private bool CheckRay(Vector3 path, List<SwingPoint> list)
    {
        _rayCast.CastTo = path;
        _rayCast.ForceRaycastUpdate();
        if (_rayCast.IsColliding() == true)
        {
            var point = _rayCast.GetCollisionPoint();
            var distance = ToGlobal(Translation).DistanceTo(point);
            list.Add(new SwingPoint(point, ToGlobal(Translation).DistanceTo(point), Mathf.Abs((_perfectPoint - point).Length())));
            return true;
        }
        return false;
    }

    public bool CanSwing()
    {
        var position = GetParent<KinematicBody>().Translation;
        var forward = GetParent<KinematicBody>().Transform.basis.z.Normalized();
        var res = _perfectPointD.z * forward;
        res.y = _perfectPointD.y;
        var yVelocity = GetNode<BaseStateMachine>(_pathToStateMachine).GetNodeOfType<PlayerBody>().Velocity.y;
        _perfectPoint = forward * yVelocity + position + new Vector3(0, res.y, 0);
        //CurrentSwingPoint = new SwingPoint(_perfectPoint, position.DistanceTo(_perfectPoint));
        ScanWorld();
        var previous = new SwingPoint(Vector3.Zero, 0, float.MaxValue);

        if (_swingPoints.IsEmpty())
            return false;

        foreach (var item in _swingPoints)
        {
            if (item.Score < previous.Score)
                previous = item;
        }
        CurrentSwingPoint = previous;

        return true;
    }
}