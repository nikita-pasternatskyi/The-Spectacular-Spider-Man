using Godot;
using MP.Extensions;
using System.Collections.Generic;

namespace MP.StateMachine
{
    public sealed class State : Node
    {
        [Signal] private delegate void StateEntered();
        [Signal] private delegate void StateExit();
        private BaseStateMachine _stateMachine;

        private List<StateAction> _enterActions;
        private List<StateAction> _updateActions;
        private List<StateAction> _fixedUpdateActions;
        private List<StateAction> _exitActions;

        public override void _Ready()
        {
            this.Disable();
        }

        public void Init(BaseStateMachine baseStateMachine)
        {
            _stateMachine = baseStateMachine;

            _enterActions = new List<StateAction>();
            _fixedUpdateActions = new List<StateAction>();
            _updateActions = new List<StateAction>();
            _exitActions= new List<StateAction>();

            var actionsNode = FindNode("Actions", false);
            var target = actionsNode == null ? this : actionsNode;

            var actions = target.GetChildren<StateAction>();
            foreach (var action in actions)
            {
                AddActionToAList(action);
                action.Init(baseStateMachine);
            }
        }

        private void AddActionToAList(StateAction action)
        {
            if(action.OnEnter == true)
            {
                _enterActions.Add(action);
            }
            else if(action.OnExit == true)
            {
                _exitActions.Add(action);
            }
            else if(action.OnFixedUpdate == true)
            {
                _fixedUpdateActions.Add(action);
            }
            else if(action.OnUpdate == true)
            {
                _updateActions.Add(action);
            }
        }

        public void Process(float delta) => CallActions(_updateActions, delta);

        public void PhysicsProcess(float delta) => CallActions(_fixedUpdateActions, delta);

        public void Enter()
        {
            EnterStateCallback();
            CallActions(_enterActions);
            EmitSignal(nameof(StateEntered));
        }

        public void Exit()
        {
            ExitStateCallback();
            CallActions(_exitActions);
            EmitSignal(nameof(StateExit));
        }

        private void CallActions(in List<StateAction> actionList, float delta = 1)
        {
            for (int i = 0; i < actionList.Count; i++)
            {
                StateAction item = actionList[i];
                item.Act(delta);
            }
        }

        private void EnterStateCallback()
        {
            for (int i = 0; i < _enterActions.Count; i++)
            {
                StateAction item = _enterActions[i];
                item.OnStateEnter();
            }
        }
        private void ExitStateCallback()
        {
            for (int i = 0; i < _exitActions.Count; i++)
            {
                StateAction item = _exitActions[i];
                item.OnStateExit();
            }
        }
    }
}