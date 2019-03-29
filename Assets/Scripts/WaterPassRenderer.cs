using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPassRenderer : MonoBehaviour
{

    public Camera renderCamera;
    public RenderTexture renderTexture;
    private RenderTexture normalTexture;
    public Shader fastShaderReplacement;
    public Shader normalShaderReplacement;

    public GameObject waterPlane;

    public int framCount = 60;
    private int lastFrameCount;

    // Start is called before the first frame update
    void Start()
    {
        normalTexture = new RenderTexture(renderCamera.pixelWidth, renderCamera.pixelHeight, 24);
        Shader.SetGlobalTexture("_CameraNormalsTexture", normalTexture);

        //renderCamera.targetTexture = renderTexture;
        //renderCamera.SetReplacementShader(fastShaderReplacement, "Opaque");

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RenderTerrainNormals();
        RenderWaterPrePass();
    }

    private void RenderWaterPrePass()
    {
        renderCamera.targetTexture = renderTexture;
        //renderCamera.SetReplacementShader(fastShaderReplacement, "RenderType");

        waterPlane.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", renderTexture);

        renderCamera.Render();
    }

    private void RenderTerrainNormals()
    {
        renderCamera.targetTexture = normalTexture;
        renderCamera.RenderWithShader(normalShaderReplacement, "RenderType");
    }
}
