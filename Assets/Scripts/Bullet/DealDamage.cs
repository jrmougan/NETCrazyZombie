using UnityEngine;
using Unity.Netcode;

public class DealDamage : MonoBehaviour
{


    public int PLAYER_DAMAGE = 10;



    void OnCollisionEnter(Collision collision)
    {
        // saber con que colisiona
        Debug.Log("Colision con: " + collision.gameObject.name);



        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("ApplyDamage", PLAYER_DAMAGE);

        }
    }

}
