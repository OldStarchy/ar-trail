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

    private Color mainColor = new Color(0f, 1f, 0f, 1f);
    private Color subColor = new Color(0f, 0.5f, 0f, 1f);

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
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
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
                    }

                    //Z axis lines
                    for (float i = 0; i <= gridSizeX; i += step)
                    {
                        GL.Vertex3(startX + i, startY + j + offsetY, startZ);
                        GL.Vertex3(startX + i, startY + j + offsetY, startZ + gridSizeZ);
                    }
                }

                //Y axis lines
                for (float i = 0; i <= gridSizeZ; i += step)
                {
                    for (float k = 0; k <= gridSizeX; k += step)
                    {
                        GL.Vertex3(startX + k, startY + offsetY, startZ + i);
                        GL.Vertex3(startX + k, startY + offsetY + gridSizeY, startZ + i);
                    }
                }
            }

            step = largeStep;
            color = mainColor;
            doIt = showMain;
        }

        GL.End();
    }
}