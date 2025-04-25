using UnityEngine;

public class BulletHit : MonoBehaviour
{
    [SerializeField] GameObject particle;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision con: " + collision.gameObject.name);

        Instantiate(particle, transform.position, Quaternion.identity);

        Destroy(gameObject);


    }
}
