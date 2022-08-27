using System;
using UnityEngine;

public static class SelectionBoundingBox
{
    public static MeshCollider TriggerSelectedObjects(GameObject go, LayerMask groundLayer, Vector2 startPos, Vector2 endPos)
    {
        Vector2[] corners = SelectionBoundingBox.GetBoundingBox(startPos, endPos);

        Vector3[] verts = new Vector3[4];
        Vector3[] vecs = new Vector3[4];

        Debug.Log(corners.Length);

        for (int i = 0; i < corners.Length; i++)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(corners[i]);
            if (Physics.Raycast(ray, out hit, groundLayer))
            {
                verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                vecs[i] = ray.origin - hit.point;
            }
        }

        Mesh selectionMesh = GenerateSelectionMesh(verts, vecs);

        MeshCollider selectionBox = go.AddComponent<MeshCollider>();
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;

        return selectionBox;
    }

     //create a bounding box (4 corners in order) from the start and end mouse position
    private static Vector2[] GetBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    // Generate a mesh from the 4 bottom points
    private static Mesh GenerateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }


   
}
