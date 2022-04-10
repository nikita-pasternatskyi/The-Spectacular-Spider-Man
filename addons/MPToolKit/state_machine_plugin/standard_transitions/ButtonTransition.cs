using Godot;
using MP.InputWrapper;

namespace MP.StateMachine
{
    public class ButtonTransition : Transition
    {
        [Export] private ButtonState _condition;
        [Export] private Buttons[] _buttonConditions;
        [Export] private bool _allTriggered;

        public sealed override bool Check()
        {
            if (_allTriggered == true)
            {
                return CheckButtons(false);
            }
            
            return CheckButtons(true);
        }

        private bool CheckButtons(bool awaitedResult)
        {
            for (int i = 0; i < _buttonConditions.Length; i++)
            {
                Buttons item = _buttonConditions[i];
                var button = InputBindings.Bindings[item];
                if (GetResult(button) == awaitedResult)
                    return AdditionalCheck(awaitedResult);
            }
            return AdditionalCheck(!awaitedResult);
        }

        protected virtual bool AdditionalCheck(bool initialResult = true) { return initialResult; }

        private bool GetResult(string item)
        {
            switch (_condition)
            {
                case ButtonState.JustPressed:
                    return Input.IsActionJustPressed(item);
                case ButtonState.JustReleased:
                    return Input.IsActionJustReleased(item);
                case ButtonState.Pressed:
                    return Input.IsActionPressed(item);
                case ButtonState.Released:
                    return Input.IsActionPressed(item) == false;
            }
            return false;
        }
    }
}