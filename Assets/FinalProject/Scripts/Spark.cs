
using System.Collections;
using UnityEngine;

public class Spark : MonoBehaviour
{
    Vector3 m_IntialVelocity;

    // Randomize the direction of intial velocity
    bool m_Randomize = true;

    // Tweak the speed how a spark will become smaller
    const float k_ShrinkRate = 10f; //was 5f before, but I change it to 2f to make the shrinking more visible

    // A timer to destroy this spark when it reaches to 0
    const float k_Delay = 1.0f; 
    
    void Awake()
    {
        m_IntialVelocity = new Vector3(0f, 0.2f, 0f);
        m_Randomize = true;

        if(m_Randomize){
            m_IntialVelocity = Random.rotation * m_IntialVelocity;
        }

        //TODO: Assign the start velocity to the spark's rigidbody velocity
        GetComponent<Rigidbody>().velocity = m_IntialVelocity;

        //TODO: Invoke a destroy function after a certain time
        Invoke("FadeAway", k_Delay);
        

    }

    void Update()
    {
        //TODO: Make the spark shrink by "shrinkRate".                          
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 
                                    Time.deltaTime * k_ShrinkRate);
    }

    void FadeAway()
    {
        //TODO: Destroy the spark
        Destroy(gameObject);
    }
}
