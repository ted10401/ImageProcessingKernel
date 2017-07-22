using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class KernelPostProcessing : MonoBehaviour
{
    public enum ConvolutionKernel
    {
        Identity,
        BoxBlur,
        GaussianBlur3x3,
        Sharpen,
        Emboss3x3,
        EdgeEnhance,
        EdgeDetection_1,
        EdgeDetection_2,
        EdgeDetection_3,
        GradientRoberts2x2,
        GradientPrewitt3x3,
        GradientSobel3x3,
    }

    [SerializeField]
    private ConvolutionKernel m_kernel = ConvolutionKernel.Identity;

    private Shader m_shader;
    private Material m_material;

    private void Awake()
    {
        if (!CheckSupport())
        {
            enabled = false;
            return;
        }

        UpdateMaterial();
    }


    private bool CheckSupport()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            Debug.LogWarning("[PostProcessing] - The platform didn't support image effects.");
            return false;
        }

        return true;
    }


    private Material UpdateMaterial()
    {
        m_shader = Shader.Find("Hidden/Kernel/" + m_kernel.ToString());

        if (null == m_shader ||
            !m_shader.isSupported)
        {
            return null;
        }

        if (null == m_material ||
            (null != m_material && m_material.shader != m_shader))
        {
            m_material = new Material(m_shader);
        }

        return m_material;
    }


    public virtual void OnValidate()
    {
        UpdateMaterial();
    }


    public virtual void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit (source, destination, m_material);
    }
}
