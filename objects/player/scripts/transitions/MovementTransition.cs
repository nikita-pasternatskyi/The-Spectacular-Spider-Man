using Godot;
using MP.StateMachine;

public class MovementTransition : ButtonTransition
{
    [Export] private NodePath _pathToKinematicBody;
    private KinematicBody _kinematicBody;

    protected override void OnReady()
    {
        _kinematicBody = GetNode<KinematicBody>(_pathToKinematicBody);
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