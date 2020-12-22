using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowSize : MonoBehaviour
{

    public TextMeshPro TextSize;
    // public TextMeshPro TextSmallMediumLarge;

    private GameObject Camera; 
    private Collider m_Collider;
    private float m_Size; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        m_Collider = GetComponent<Collider>();
        m_Size = m_Collider.bounds.size.x;
        // m_Size = m_Size * 100;
        
        TextSize.text = m_Size.ToString("F2") + 'm';
        TextSize.transform.rotation = Camera.transform.rotation;

        // if (m_Size < 0.35)
        // {
        //     TextSmallMediumLarge.text = "This is a Small Pizza";
        //     TextSmallMediumLarge.transform.rotation = Camera.transform.rotation;
        // }

        // else if (m_Size > 0.65)
        // {
        //     TextSmallMediumLarge.text = "This is a Large Pizza";
        //     TextSmallMediumLarge.transform.rotation = Camera.transform.rotation;

        // }
        // else
        // {
        //     TextSmallMediumLarge.text = "This is a Medium Pizza";
        //     TextSmallMediumLarge.transform.rotation = Camera.transform.rotation;
        // }


    }

    public float GetSize(){
        return m_Size;
    }
}