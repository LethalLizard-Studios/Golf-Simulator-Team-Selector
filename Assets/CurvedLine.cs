using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class CurvedLine : Graphic
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 controlPoint;  // Control point for the curve
    public float lineThickness = 5f;
    public int segments = 20; // Number of segments for smoothness

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Vector2 prevPoint = BezierPoint(0);
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector2 point = BezierPoint(t);

            DrawLineSegment(vh, prevPoint, point);

            prevPoint = point;
        }
    }

    private Vector2 BezierPoint(float t)
    {
        // Quadratic Bezier curve formula
        return Mathf.Pow(1 - t, 2) * startPoint +
               2 * (1 - t) * t * controlPoint +
               Mathf.Pow(t, 2) * endPoint;
    }

    private void DrawLineSegment(VertexHelper vh, Vector2 start, Vector2 end)
    {
        Vector2 perpendicular = Vector2.Perpendicular(end - start).normalized * lineThickness / 2;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = start - perpendicular;
        vh.AddVert(vertex);

        vertex.position = start + perpendicular;
        vh.AddVert(vertex);

        vertex.position = end + perpendicular;
        vh.AddVert(vertex);

        vertex.position = end - perpendicular;
        vh.AddVert(vertex);

        int index = vh.currentVertCount;
        vh.AddTriangle(index - 4, index - 3, index - 2);
        vh.AddTriangle(index - 4, index - 2, index - 1);
    }
}
