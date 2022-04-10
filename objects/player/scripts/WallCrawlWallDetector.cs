using Godot;

public class WallCrawlWallDetector : RayCast
{
    [Export] private float _minimumAngleForWall = 90;
    
    public Vector3 CurrentWallNormal { get; private set; }
    
    public bool CanCrawl { get; private set; }

    public override void _PhysicsProcess(float delta)
    {
        if(IsColliding())
        {
            var currentNormal = GetCollisionNormal();
            if (Mathf.Rad2Deg(Vector3.Up.AngleTo(currentNormal)) >= _minimumAngleForWall) 
            {
                CanCrawl = true;
                CurrentWallNormal = currentNormal;
            }
        }
    }

}
