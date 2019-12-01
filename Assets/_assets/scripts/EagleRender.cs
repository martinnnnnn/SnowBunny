
using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
//public class GreyScale : MonoBehaviour
//{

//    public float intensity;
//    private Material material;

//    // Creates a private material used to the effect
//    void Awake()
//    {
//        material = new Material(Shader.Find("Custom/DepthShader"));
//    }

//    // Postprocess the image
//    void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        //if (intensity == 0)
//        //{
//        //    Graphics.Blit(source, destination);
//        //    return;
//        //}

//        //material.SetFloat("_bwBlend", intensity);
//        Graphics.Blit(source, destination, material);
//    }
//}

//[ExecuteInEditMode, ImageEffectAllowedInSceneView]
//public class EagleRender : MonoBehaviour
//{
//    public Texture2D eagle;
//    public Shader eagleRenderShader;
//    public RenderTexture outcamTexture;
//    public RenderTexture mainCameraTexture;

//    private Material material;

//    void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        //if (material == null)
//        //{
//        //    material = new Material(eagleRenderShader);
//        //    material.hideFlags = HideFlags.HideAndDontSave;
//        //}

//        RenderTexture r = RenderTexture.GetTemporary(
//            source.width, source.height, 0, source.format
//        );

//        //material.SetTexture("_EagleTexture", eagle);

//        Graphics.Blit(source, r);
//        Graphics.Blit(r, destination);

//    }
//}

[ExecuteInEditMode]
public class EagleRender : MonoBehaviour
{
    public Material processingImageMaterial;
    public Texture2D eagle;
    public RenderTexture eagleRender;

    void OnRenderImage(RenderTexture imageFromRenderedImage, RenderTexture imageDisplayedOnScreen)
    {
        if (processingImageMaterial != null)
        {
            processingImageMaterial.SetTexture("_EagleTex", eagleRender);
            Graphics.Blit(imageFromRenderedImage, imageDisplayedOnScreen, processingImageMaterial);
        }
    }
}