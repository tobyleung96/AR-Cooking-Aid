using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    //TODO: Add the particle prefab reference here.
    [SerializeField]
    GameObject m_ParticlePrefab;

    int k_ParticleNumber = 30;

    float k_ParticleMinSize = 0.005f;

    float k_ParticleMaxSize = 0.03f;

    Vector3 m_RotateDegreePerFrame;

    void Awake()
    {
        // m_RotateDegreePerFrame = new Vector3(15f, 30f, 45f);
    }

    void Update()
    {
        //TODO: Rotate the trophy by "rotateDegreePerFrame".
        // transform.Rotate(m_RotateDegreePerFrame * Time.deltaTime, Space.Self);
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     //TODO: Triggers Explode() when hit by the Ball.
    //     if (other.gameObject.CompareTag("Ball")){
    //         // gameObject.SetActive(false);
    //         Explode();
    //         Debug.Log("collided with trophy");
    //     }

    // }
    
    public void Explode()
    {
        for(int i=0; i<k_ParticleNumber; i++){
            //TODO: Randomize the spark prefab's localScale between particleMinSize and particleMaxSize
            float randomSize = Random.Range(k_ParticleMinSize, k_ParticleMaxSize);
            //TODO: Instantiate the prefab
            Instantiate(m_ParticlePrefab, transform.position, transform.rotation);
            m_ParticlePrefab.transform.localScale = new Vector3( randomSize, randomSize, randomSize);
        }

    }
}
