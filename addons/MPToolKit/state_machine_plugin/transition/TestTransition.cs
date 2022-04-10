using Godot;
namespace MP.StateMachine
{
    public class TestTransition : Transition
    {
        [Export] private bool _condition;

        public override bool Check()
        {
            return _condition;
        }
    }
}