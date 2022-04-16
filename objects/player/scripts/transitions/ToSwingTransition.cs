using Godot;
using MP.StateMachine;

public class ToSwingTransition : ButtonTransition
{
    [Export] private NodePath _pathToWebShooter;

    private WebShooter _webShooter;

    protected override void Ready()
    {
        _webShooter = GetNode<WebShooter>(_pathToWebShooter);
    }

    protected override bool AdditionalCheck(bool initialResult = true)
    {
        if(initialResult == true)
        {
             return _webShooter.CanSwing();
        }
        return false;
    }


}
