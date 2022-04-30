using Godot;
using MP.Extensions;
using System;

namespace MP.AnimatorWrappers
{
    public class AnimatedModel : Spatial
    {
        [Export] private NodePath _pathToAnimTree;
        [Export] private NodePath _pathToAnimationPlayer;
        private AnimationPlayer _animationPlayer;
        private AnimationTree _tree;

        private const string RootName = "parameters/";
        private const string BlendParameterName = "/blend_position";
        private const string EnumParameterName = "/current";
        private const string OneShotParameterName = "/active";
        private const string TimeScaleParameterName = "/scale";

        public override void _Ready()
        {
            this.TryGetNodeFromPath(_pathToAnimationPlayer, out _animationPlayer);
            this.TryGetNodeFromPath(_pathToAnimTree, out _tree);
        }

        public void PlayAnimation(in string name, float transitionDuration, float playbackSpeed)
        {
            if(_animationPlayer.CurrentAnimation != name)
                _animationPlayer.Play(name, transitionDuration, playbackSpeed);
        }

        public void SetBlendPosition(in string path, object value)
        {
            _tree.Set(RootName + path + BlendParameterName, value);
        }

        public void SetAnimatorEnum(string propertyName, object value)
        {
            _tree.Set(RootName + propertyName + EnumParameterName, value);
        }

        public void SetOneShotBool(string propertyName, object value)
        {
            _tree.Set(RootName + propertyName + OneShotParameterName, value);
        }

        public void SetAnimatorTimeScale(string propertyName, object value)
        {
            _tree.Set(RootName + propertyName + TimeScaleParameterName, value);
        }

        public void SetParameter(AnimParametersType parameterType, in string name, object value)
        {
            switch (parameterType)
            {
                case AnimParametersType.BlendSpace1D:
                    SetBlendPosition(name, value);
                    break;
                case AnimParametersType.AnimationEnum:
                    SetAnimatorEnum(name, value);
                    break;
                case AnimParametersType.TimeScale:
                    SetAnimatorTimeScale(name, value);
                    break;
                case AnimParametersType.OneShotBoolean:
                    SetOneShotBool(name, value);
                    break;
            }
        }
    }
}
