using Godot;
using System;

public class GameState : Node
{
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed(InputBindings.FULLSCREEN))
           OS.WindowFullscreen = !OS.WindowFullscreen;
    }
}
