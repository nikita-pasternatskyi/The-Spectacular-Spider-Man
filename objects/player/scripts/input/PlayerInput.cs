using Godot;
using MP.Extensions;

public class PlayerInput : Node
{
    [Export] private NodePath _pathToSpringArm;

    private SpringArm _springArm;
    private Transform _parentTransform;
    private Vector3 _movementInput;
    private Vector3 _relativeInput;
    private Vector3 _wallCrawlInput;
    public Vector3 AbsoluteMovementInput => _movementInput;
    public Vector3 RelativeMovementInput => _relativeInput;
    public Vector3 WallCrawlMovementInput => _wallCrawlInput;

    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Captured);

        this.TryGetNodeFromPath(_pathToSpringArm, out _springArm);
        
        _parentTransform = GetParent<KinematicBody>().Transform;
    }

    public override void _Input(InputEvent @event)
    {
        _movementInput.z = Input.GetActionStrength(InputBindings.MOVE_FORWARD) - Input.GetActionStrength(InputBindings.MOVE_BACK);
        _movementInput.x = Input.GetActionStrength(InputBindings.MOVE_RIGHT) - Input.GetActionStrength(InputBindings.MOVE_LEFT);

        //_wallCrawlInput = _movementInput;
        //_wallCrawlInput = -_springArm.Transform.basis.z * _movementInput.z + _springArm.Transform.basis.x * _movementInput.x;

        _relativeInput = -_parentTransform.basis.z * _movementInput.z + _parentTransform.basis.x * _movementInput.x;
        _relativeInput = _relativeInput.Rotated(Vector3.Up, _springArm.Rotation.y).Normalized();
    }


}