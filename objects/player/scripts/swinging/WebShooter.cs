using Godot;
using System.Collections.Generic;
using MP.Extensions;

public class WebShooter : Spatial
{
    [Export] private int _anglesToRotateDown;
    [Export] private int _angleRadius;
    [Export] private int _angleBetweenRays;
    [Export] private float _perfectDistance;
    [Export] private float _perfectStartAngle;
    [Export] private float _maxDistance;
    [Export] private float _minDistance = 0.1f;

    private RayCast _rayCast;
    private List<SwingPoint> _swingPoints;
    private Vector3 _initialRotation;
    private Vector3[] _rayCastPath;

    private Spatial _parent;

    public SwingPoint CurrentSwingPoint { get; private set; }

    public override void _Ready()
    {
        _rayCast = new RayCast();

        var rayCount = _angleRadius / _angleBetweenRays + 1;
        _swingPoints = new List<SwingPoint>(rayCount);
        _rayCastPath = new Vector3[rayCount];

        _parent = GetParentSpatial();

        _initialRotation = Rotation;

        AddChild(_rayCast);

        CreateRayPath(rayCount);
    }

    private void CreateRayPath(int rayCount)
    {
        for (int i = 0; i < rayCount; i++)
        {
            var angle = _angleBetweenRays * (i - rayCount / 2.0f);
            _rayCastPath[i] = _parent.Transform.basis.z.Rotated(Vector3.Up, Mathf.Deg2Rad(angle)) * _maxDistance;
        }
    }

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
                GD.Print($"Rotating {RotationDegrees}");
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
            _rayCast.CastTo = _rayCastPath[i];
            _rayCast.ForceRaycastUpdate();
            if (_rayCast.IsColliding() == true)
            {
                var point = _rayCast.GetCollisionPoint();
                if (GetParentSpatial().Translation.DistanceSquaredTo(point) < _minDistance * _minDistance)
                {
                    return;
                }
                _swingPoints.Add(new SwingPoint { Position = point, Normal = _rayCast.GetCollisionNormal()});
                continue;
            }
        }
    }

    private void ScorePoints()
    {
        for (int i = 0; i < _swingPoints.Count; i++)
        {
            SwingPoint point = _swingPoints[i];
            var angle = point.Position.Normalized().Angle(_parent.Translation.Normalized());
            angle = Mathf.Deg2Rad(angle);
            var distance = _parent.Translation.DistanceSquaredTo(point.Position);
            var distanceScore = Mathf.Abs(distance - _perfectDistance * _perfectDistance);
            var angleScore = Mathf.Abs(_perfectStartAngle - angle);
            point.Score = angleScore + distanceScore;
        }
    }

    public bool CanSwing()
    {
        ScanWorld();
        if (_swingPoints.IsEmpty())
            return false;
        ScorePoints();
        var winner = new SwingPoint(Vector3.Zero, 0, float.MaxValue);

        foreach (var item in _swingPoints)
        {
            if (item.Score < winner.Score)
                winner = item;
        }

        CurrentSwingPoint = winner;
        _swingPoints.Clear();
        return true;
    }
}