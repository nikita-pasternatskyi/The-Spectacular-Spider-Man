using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetAnimationEnumValue : SetAnimationValue<int>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetAnimatorEnum(PropertyName, Value);
        }
    }
}
