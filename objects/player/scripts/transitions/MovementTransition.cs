using Godot;
using MP.StateMachine;

public class MovementTransition : ButtonTransition
{
    [Export] private NodePath _pathToPlayerBody;
    private PlayerBody _kinematicBody;

    protected override void Ready()
    {
        _kinematicBody = GetNode<PlayerBody>(_pathToPlayerBody);
    }

    protected override bool AdditionalCheck(bool initialResult = true)
    {
        if (initialResult == true)
        {
            return _kinematicBody.IsOnFloor();
        }
        return false;
    }
}
