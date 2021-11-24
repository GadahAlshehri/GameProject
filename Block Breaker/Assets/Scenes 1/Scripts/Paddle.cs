using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(0f, -4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left") && transform.position.x > -5.5f)
        {
           GetComponent<Rigidbody2D>().velocity = new Vector2(-10f, 0f);
        }
        else if (Input.GetKey("right") && transform.position.x< 5.5f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(10f, 0f);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
        
    
    }
}
