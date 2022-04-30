using Godot;
using MP.Extensions;

namespace MP.StateMachine.Actions
{
    public class ReplicateField : StateAction
    {
        [Export] private NodePath _outputnodePath;
        [Export] private string _outputField;
        private Node _outputNode;

        [Export] private NodePath _importNodePath;
        [Export] private string _importField;
        private Node _importNode;

        public override void Init(BaseStateMachine stateMachine)
        {
            this.TryGetNodeFromPath(_outputnodePath, out _outputNode);
            this.TryGetNodeFromPath(_importNodePath, out _importNode);
        }

        public override void Act(float delta)
        {
            _importNode.Set(_importField, _outputNode.Get(_outputField));
        }
    }
}
