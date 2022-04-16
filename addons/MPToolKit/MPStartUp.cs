using Godot;
[Tool]
public class MPStartUp : EditorPlugin
{
    private const string StateMachineName = "StateMachine";
    private const string NodeBaseName = "Node";
    private const string StateName = "State";
    private const string AnimatorName = "AnimatorModel";
    private const string BasePath = "addons/MPToolKit/";

    public override void _EnterTree()
    {
        var stateMachineScript = GD.Load<Script>(BasePath + "state_machine_plugin/state_machine/BaseStateMachine.cs");
        var stateMachineTexture = GD.Load<Texture>(BasePath + "icons/state_machine.svg");
        AddCustomType(StateMachineName, NodeBaseName, stateMachineScript, stateMachineTexture);

        
        var stateScript = GD.Load<Script>(BasePath + "state_machine_plugin/state/State.cs");
        var stateTexture = GD.Load<Texture>(BasePath + "icons/state.svg");
        AddCustomType(StateName, NodeBaseName, stateScript, stateTexture);

        var animatorScript = GD.Load<Script>(BasePath + "animator_model/AnimatorModel.cs");
        var animatorTexture = GD.Load<Texture>(BasePath + "icons/animator.svg");
        AddCustomType(AnimatorName, "Spatial", animatorScript, animatorTexture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType(StateMachineName);
        RemoveCustomType(StateName);
        RemoveCustomType(AnimatorName);
    }
}
