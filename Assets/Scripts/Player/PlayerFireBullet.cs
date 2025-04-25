using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System.Linq;

public class PlayerFireBullet : NetworkBehaviour
{
    [SerializeField] private GameObject bulletServerPrefab;
    [SerializeField] private GameObject bulletClientPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed = 10f;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 spawnPos = spawnPoint.position + spawnPoint.forward * 0.7f;
            Vector3 direction = transform.forward;

            SpawnDummyBullet(spawnPos, direction);
            FireBulletServerRpc(spawnPos, direction);
        }
    }

    private void SpawnDummyBullet(Vector3 pos, Vector3 dir)
    {
        GameObject visual = Instantiate(bulletClientPrefab, pos, Quaternion.LookRotation(dir));

        if (visual.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = dir * bulletSpeed;
    }

    [Rpc(SendTo.Server)]
    private void FireBulletServerRpc(Vector3 pos, Vector3 dir)
    {
        GameObject bullet = Instantiate(bulletServerPrefab, pos, Quaternion.LookRotation(dir));
        bullet.GetComponent<NetworkObject>().Spawn(true);

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = dir * bulletSpeed;

        // Mandamos a todos los clientes (incluyendo el dueño)
        FireBulletClientRpc(pos, dir);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FireBulletClientRpc(Vector3 pos, Vector3 dir)
    {
        if (IsOwner) return; // evitamos que el dueño dispare 2 veces
        SpawnDummyBullet(pos, dir);
    }
}
