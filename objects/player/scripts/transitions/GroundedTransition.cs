using Godot;
using MP.StateMachine;
using MP.Extensions;

public class GroundedTransition : Transition
{
    [Export] private NodePath _pathToPlayerBody;
    [Export] private bool _awaitedResult;
    
    protected PlayerBody PlayerBody{get; private set;}

    protected override void Ready()
    {
        this.TryGetNodeFromPath(_pathToPlayerBody, out PlayerBody pb);
        PlayerBody = pb;
    }

    public override bool Check()
    {
        return AdditionalCheck(PlayerBody.Grounded == _awaitedResult);
    }

    protected virtual bool AdditionalCheck(bool result)
    {
        return result;
    }
}
