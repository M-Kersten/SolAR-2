using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class Circle : MonoBehaviour
{
    public int segments;
    public float radius;
    LineRenderer line;
    
    [ContextMenu("CreateCircle")]
    void CreatePoints ()
    {
        line = gameObject.GetComponent<LineRenderer>();
       
        line.SetVertexCount (segments + 1);
        line.useWorldSpace = false;
        float x;
        float y = 0f;
        float z;
       
        float angle = 20f;
       
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
                   
            line.SetPosition (i,new Vector3(x,y,z) );
                   
            angle += (360f / segments);
        }
    }
}
