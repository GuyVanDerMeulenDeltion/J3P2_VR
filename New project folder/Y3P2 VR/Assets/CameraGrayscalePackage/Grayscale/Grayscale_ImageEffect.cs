using UnityEngine;

[ExecuteInEditMode]
public class Grayscale_ImageEffect : MonoBehaviour {

    public Material m_Material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(m_Material != null)
            Graphics.Blit(source, destination, m_Material);
    }
}
