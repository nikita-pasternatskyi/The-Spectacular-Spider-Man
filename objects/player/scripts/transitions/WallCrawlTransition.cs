using Godot;
using MP.Extensions;
using MP.StateMachine;

public class WallCrawlTransition : ButtonTransition
{
    [Export] private WallCrawlWallDetector _wallCrawlWallDetector;

    protected override void Ready()
    {
        GetParent().GetParent().TryGetNodeInMeAndParent<WallCrawlWallDetector>(out _wallCrawlWallDetector);
    }

    protected override bool AdditionalCheck(bool initialResult = true)
    {
        if (initialResult == true)
        {
            if (_wallCrawlWallDetector.CanCrawl == true)
            {
                return true;
            }
        }

        return false;
    }

}