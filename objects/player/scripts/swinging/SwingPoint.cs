using Godot;

public class SwingPoint
{
    public readonly Vector3 StartPosition;
    public float Distance;
    public Vector3 Position;
    public float Score;

    public SwingPoint(Vector3 startPosition, float distance)
    {
        StartPosition = startPosition;
        Distance = distance;
        Position = StartPosition;
    }
    public SwingPoint(Vector3 startPosition, float distance, float score)
    {
        Score = score;
        StartPosition = startPosition;
        Distance = distance;
        Position = StartPosition;
    }
}