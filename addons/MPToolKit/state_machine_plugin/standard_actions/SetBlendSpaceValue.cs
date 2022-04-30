using MP.AnimatorWrappers;

namespace MP.StateMachine.Actions
{
    public class SetBlendSpaceValue : SetAnimationValue<float>
    {
        public override void Act(float delta)
        {
            AnimatedModel.SetBlendPosition(PropertyName, Value);
        }
    }
}
