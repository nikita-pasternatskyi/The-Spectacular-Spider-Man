using Godot;
using MP.StateMachine;

public class ToSwingTransition : Transition
{
    [Export] private NodePath _pathToWebShooter;
    [Export] private string _buttonName = "throwWeb";

    private WebShooter _webShooter;

    protected override void Ready()
    {
        _webShooter = GetNode<WebShooter>(_pathToWebShooter);
    }

    public override bool Check()
    {
        if(Input.IsActionJustPressed(_buttonName) == true)
        {
            if (_webShooter.CanSwing() == true)
                return true;
        }
        return false;
    }


}
