using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayStart());
    }
   private void OnTriggerEnter2D(Collider2D collision)
     
   {
       Destroy(gameObject);
       {
         FindObjectOfType<LevelController>().DecreaseLives();
       }
   }

   IEnumerator DelayStart()
     {
         yield return new WaitForSeconds(2);
         GetComponent<Rigidbody2D>().AddForce(new Vector2(250f, 250f));
     }
  
}
