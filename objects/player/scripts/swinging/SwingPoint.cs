using Godot;

public struct SwingPoint
{
    public readonly Vector3 StartPosition;
    public Vector3 Normal;
    public float Distance;
    public Vector3 Position;
    public float Score;

    public SwingPoint(Vector3 startPosition, float distance, float score)
    {
        Score = score;
        StartPosition = startPosition;
        Distance = distance;
        Position = StartPosition;
        Normal = Vector3.Zero;
    }
}