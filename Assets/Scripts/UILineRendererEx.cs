using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRendererEx : Graphic
{
    public List<Vector2> points;

    float width;
    float height;

    public float thickness = 10.0f;

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        vertexHelper.Clear();


        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        if (points==null || points.Count < 2)
        {
            return;
        }
        float angle = 0;

        for (int i =0; i<points.Count; i++)
        {
            Vector2 point = points[i];

            if (i < points.Count - 1)
            {
                angle = GetAngle(points[i], points[i + 1]) + 45f;
            }

            DrawVerticesForPoint(point, vertexHelper, angle);
        }

        for (int i = 0; i < points.Count-1; i++)
        {
            int index = i * 2;
            vertexHelper.AddTriangle(index + 0, index + 1, index + 3);
            vertexHelper.AddTriangle(index + 3, index + 2, index + 0);
        }
    }

    private void DrawVerticesForPoint(Vector2 point, VertexHelper helper, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0,0,angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y);

        helper.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) *  new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y);

        helper.AddVert(vertex);
    }

    private float GetAngle(Vector2 a, Vector2 b)
    {
        return (float)(Mathf.Atan2(b.y-a.y,b.x-a.x)*(180/Mathf.PI));
    }
}
