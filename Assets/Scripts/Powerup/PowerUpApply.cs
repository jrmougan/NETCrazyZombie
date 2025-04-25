using UnityEngine;
using Unity.Netcode;

public class PowerUpApply : NetworkBehaviour
{
    const int POWER = 50;

    [SerializeField] AudioClip clip;


    void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<Health>();
        if (IsServer)
        {
            if (other.CompareTag("Player"))
            {
                health.ApplyDamageRpc(-POWER);

                AudioSource.PlayClipAtPoint(clip, transform.position);

                GetComponent<NetworkObject>().Despawn();
                Destroy(gameObject);
            }
        }
    }
}
