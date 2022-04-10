using Godot;

public class DebugDraw : Spatial
{
    [Export] private SpatialMaterial _leftRayMaterial;
    [Export] private SpatialMaterial _centerRayMaterial;
    [Export] private SpatialMaterial _rightRayMaterial;
    private ImmediateGeometry _leftRay;
    private ImmediateGeometry _centerRay;
    private ImmediateGeometry _rightRay;

    public override void _Ready()
    {
        _leftRay = new ImmediateGeometry();
        _rightRay = new ImmediateGeometry();
        _centerRay = new ImmediateGeometry();


        AddChild(_rightRay);
        AddChild(_centerRay);
        AddChild(_leftRay);
        _centerRay.MaterialOverride = _centerRayMaterial;
        _rightRay.MaterialOverride = _rightRayMaterial;
        _leftRay.MaterialOverride = _leftRayMaterial;
    }

    private void DrawRayLeft(Vector3 start, Vector3 direction, float length)
    {
        _leftRay.Begin(Mesh.PrimitiveType.Lines);
        _leftRay.AddVertex(start);
        _leftRay.AddVertex(start + direction.Normalized() * length);
        _leftRay.End();
    }
    private void DrawRayCenter(Vector3 start, Vector3 direction, float length)
    {
        _centerRay.Begin(Mesh.PrimitiveType.Lines);
        _centerRay.AddVertex(start);
        _centerRay.AddVertex(start + direction.Normalized() * length);
        _centerRay.End();
    }
    private void DrawRayRight(Vector3 start, Vector3 direction, float length)
    {
        _rightRay.Begin(Mesh.PrimitiveType.Lines);
        _rightRay.AddVertex(start);
        _rightRay.AddVertex(start + direction.Normalized() * length);
        _rightRay.End();
    }


}
