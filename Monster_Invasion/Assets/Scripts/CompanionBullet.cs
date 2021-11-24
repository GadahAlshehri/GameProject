using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionBullet : MonoBehaviour
{
    public float destroyAfter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyAfter -= Time.deltaTime;
        if (destroyAfter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
