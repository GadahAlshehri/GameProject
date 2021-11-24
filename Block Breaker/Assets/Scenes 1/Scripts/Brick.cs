using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int health = 1;
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
        
    }

   private void OnCollisionEnter2D(Collision2D collision)
   {
       DecreaseHealth();
   }
  
   private void DecreaseHealth()
   {
      health--;
     if(health <= 0)
     {
         FindObjectOfType<LevelLoader>().DeductBrick();
     Destroy(gameObject);
     }
     else
     {
         GetComponent<SpriteRenderer>().color = Color.red;
     }
       FindObjectOfType<LevelController>().IncreaseScore();
   }
   
    
}

