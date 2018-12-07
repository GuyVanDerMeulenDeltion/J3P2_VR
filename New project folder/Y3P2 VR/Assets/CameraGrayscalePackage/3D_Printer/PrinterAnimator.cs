using UnityEngine;

public class PrinterAnimator : MonoBehaviour {

    public Material m_Material;
    public float m_fBuildingDuration = 5;

    private float m_fMinHeightOfBuilding;
    private float m_fMaxHeightOfBuilding;
    private float m_fCalc;
    private float m_fObjectHeight;

    private void Awake()
    {
        if (m_Material == null)
            m_Material = GetComponent<MeshRenderer>().material;
        m_fObjectHeight = GetComponent<MeshRenderer>().bounds.size.y;
        m_fMaxHeightOfBuilding = transform.position.y + m_fObjectHeight + m_Material.GetFloat("_BuildGap");
        m_fMinHeightOfBuilding = transform.position.y - (m_fObjectHeight / 2);
    }

    private void Update()
    {
        if(m_Material != null)
        {
            m_fCalc = Mathf.Lerp(m_fMinHeightOfBuilding, m_fMaxHeightOfBuilding, Time.time / m_fBuildingDuration);
            m_Material.SetFloat("_CreationY", m_fCalc);
        }
        
    }
}
