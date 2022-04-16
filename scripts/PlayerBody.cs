using Godot;
using MP.Extensions;

public class PlayerBody : KinematicBody
{
    [Export] private NodePath _pathToGroundCheckRay;
    [Export] public bool UseGravity;
    [Export] public float Gravity;
    [Export] public float Damp;

    public Vector3 Velocity;
    public bool OnSlope;
    public bool Grounded;
    public Vector3 GroundNormal { get; private set; }

    private RayCast _rayCast;

    public override void _Ready()
    {
        this.TryGetNodeFromPath(_pathToGroundCheckRay, out _rayCast);
        _rayCast.ExcludeParent = true;
    }

    public override void _PhysicsProcess(float delta)
    {
        CollectGroundInfo();
        Grounded = OnFloor();

        if (UseGravity == true)
            CalculateGravity(delta);
        
        CounterAcceleration(delta);
        MoveAndSlide(Velocity, Vector3.Up);
    }

    private void CalculateGravity(float delta)
    {
        Velocity.y += Gravity * delta;
        if (IsOnFloor() == true)
        {
            Velocity.y = Mathf.Clamp(Velocity.y, 0.1f, float.MaxValue);
            return;
        }
    }

    private void CollectGroundInfo()
    {
        if(_rayCast.IsColliding() == true)
        {
            GroundNormal = _rayCast.GetCollisionNormal();
            var angle = Mathf.Rad2Deg(GroundNormal.Angle(Vector3.Up));
            OnSlope = angle != 0;
            return;
        }
        GroundNormal = Vector3.Up;
        OnSlope = false;
    }

    private bool OnFloor()
    {
        var v = Vector3.Down;
        v.y = -.5f;
        return TestMove(Transform, v, false);
    }

    public void AddForce(Vector3 force)
    {
        Velocity += force;
    }
    
    private void CounterAcceleration(float delta)
    {
        Velocity -= (Velocity * Damp) * delta;
    }

}
