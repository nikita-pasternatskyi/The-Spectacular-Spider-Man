using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetOneShotBoolean : SetAnimationValue<bool>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetOneShotBool(PropertyName, Value);
        }
    }
}
