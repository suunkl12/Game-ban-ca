using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundScroll : MonoBehaviour
{
    public float m_ScrollSpeed = 0.1f;

    private MeshRenderer m_MeshReder;
    private Vector2 m_SavedOffset;

	private void Awake()
	{
        m_MeshReder = GetComponent<MeshRenderer>();
        m_SavedOffset = m_MeshReder.sharedMaterial.GetTextureOffset("_MainTex");

    }

    // Update is called once per frame
    void Update()
    {
        float x = Time.time * m_ScrollSpeed;
        Vector2 m_offSet = new Vector2(x, 0);

        m_MeshReder.sharedMaterial.SetTextureOffset("_MainTex", m_offSet);
    }

	private void OnDisable()
	{
        m_MeshReder.sharedMaterial.SetTextureOffset("_MainTex", m_SavedOffset);
    }
}
