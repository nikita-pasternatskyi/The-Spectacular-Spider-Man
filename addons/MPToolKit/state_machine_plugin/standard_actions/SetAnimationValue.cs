using Godot;
using MP.AnimatorWrappers;
using MP.Extensions;

namespace MP.StateMachine.Actions
{
    public abstract class SetAnimationValue<T> : StateAction
    {
        [Export] private NodePath _animatedModelPath;
        [Export] protected string PropertyName;
        [Export] protected T Value;

        protected AnimatedModel AnimatedModel => _animatedModel;
        private AnimatedModel _animatedModel;
        
        public override void Init(BaseStateMachine stateMachine)
        {
            this.TryGetNodeFromPath(_animatedModelPath, out _animatedModel);
        }
    }
}
