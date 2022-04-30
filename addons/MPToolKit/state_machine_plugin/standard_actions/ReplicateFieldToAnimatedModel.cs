using Godot;
using MP.AnimatorWrappers;
using MP.Extensions;

namespace MP.StateMachine.Actions
{
    public class ReplicateFieldToAnimatedModel : StateAction
    {
        [Export] private AnimParametersType _parameterType;
        [Export] private NodePath _importNodePath;
        [Export] private string _importField;
        private Node _importNode;

        [Export] private NodePath _animatedModelPath;
        private AnimatedModel _animatedModel;
        [Export] private string _propertyName;

        public override void Init(BaseStateMachine stateMachine)
        {
            this.TryGetNodeFromPath(_importNodePath, out _importNode);
            this.TryGetNodeFromPath(_animatedModelPath, out _animatedModel);
        }

        public override void Act(float delta)
        {
            var value = _importNode.Get(_importField);
            _animatedModel.SetParameter(_parameterType, _propertyName, value);
        }
    }
}
