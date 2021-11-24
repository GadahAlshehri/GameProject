using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
            if (!transform.parent.GetComponent<PlayerController>().shielded)
            {
                transform.parent.GetComponent<PlayerController>().ChangeHealth();
                SoundManger.HitPlayer();
            }
    }
}
