using UnityEngine;
using System.Collections;

public class GridOverlay : MonoBehaviour
{

    public Transform trackingObject;

    public bool showMain = true;
    public bool showSub = false;

    public int gridSizeX;
    public int gridSizeY;
    public int gridSizeZ;

    public float smallStep;
    public float largeStep;

    public float startX;
    public float startY;
    public float startZ;

    private float offsetY = 0;
    private float scrollRate = 0.1f;
    private float lastScroll = 0f;

    private Material lineMaterial;

    public Color mainColor = new Color(0f, 1f, 0f, 1f);
    public Color subColor = new Color(0f, 0.5f, 0f, 1f);

    void Start()
    {

    }

    void Update()
    {
        if (null != trackingObject)
        {
            int targetX = (int)(Mathf.Round((trackingObject.position.x - gridSizeX * 0.5f) / (largeStep)) * largeStep);
            int targetZ = (int)(Mathf.Round((trackingObject.position.z - gridSizeZ * 0.5f) / (largeStep)) * largeStep);

            startX = targetX;
            startZ = targetZ;
        }
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnPostRender()
    {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        float step = smallStep;
        Color color = subColor;
        bool doIt = showSub;

        int c = 2;
        while (c-- > 0)
        {
            if (doIt)
            {
                GL.Color(color);

                //Layers
                for (float j = 0; j <= gridSizeY; j += step)
                {
                    //X axis lines
                    for (float i = 0; i <= gridSizeZ; i += step)
                    {
                        GL.Vertex3(startX, startY + j + offsetY, startZ + i);
                        GL.Vertex3(startX + gridSizeX, startY + j + offsetY, startZ + i);
                        if (step == 0)
                            break;
                    }

                    //Z axis lines
                    for (float i = 0; i <= gridSizeX; i += step)
                    {
                        GL.Vertex3(startX + i, startY + j + offsetY, startZ);
                        GL.Vertex3(startX + i, startY + j + offsetY, startZ + gridSizeZ);
                        if (step == 0)
                            break;
                    }
                    if (step == 0)
                        break;
                }

                //Y axis lines
                for (float i = 0; i <= gridSizeZ; i += step)
                {
                    for (float k = 0; k <= gridSizeX; k += step)
                    {
                        GL.Vertex3(startX + k, startY + offsetY, startZ + i);
                        GL.Vertex3(startX + k, startY + offsetY + gridSizeY, startZ + i);
                        if (step == 0)
                            break;
                    }
                    if (step == 0)
                        break;
                }
            }

            step = largeStep;
            color = mainColor;
            doIt = showMain;
        }

        GL.End();
    }
}