using Godot;
using MP.Extensions;

public class PlayerInput : Node
{
    [Export] private NodePath _pathToSpringArm;

    private SpringArm _springArm;
    private Transform _parentTransform;
    private Vector2 _relativeVec2Input;
    private Vector3 _movementInput;

    public Vector3 AbsoluteMovementInput => _movementInput;
    public Vector2 RelativeMovementInputVec2 => _relativeVec2Input;
    public Vector3 RelativeMovementInput { get; private set; }

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
        _movementInput = _movementInput.Normalized();
        
        RelativeMovementInput = -_parentTransform.basis.z * _movementInput.z + _parentTransform.basis.x * _movementInput.x;
        RelativeMovementInput = RelativeMovementInput.Rotated(Vector3.Up, _springArm.Rotation.y).Normalized();

        _relativeVec2Input.y = RelativeMovementInput.z;
        _relativeVec2Input.x = RelativeMovementInput.x;
    }


}