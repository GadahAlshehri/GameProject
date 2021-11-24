using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed =15f;
   
    private Vector3 dir;
    void Start()
    {
        dir = GameObject.Find("Direction").transform.position;
        transform.position = GameObject.Find("FirePoint").transform.position;
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, dir, speed * Time.deltaTime);
        Destroy(GameObject.Find("Bullet(Clone)"),0.7f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        Destroy(gameObject);

        if (other.gameObject.tag == "Companion")
        {
         
            if(!GameObject.FindObjectOfType<Companion>().IsShielded())
                GameObject.FindObjectOfType<Companion>().ChangeHealth(gameObject);
            Destroy(gameObject);
        }
    }

    
}
