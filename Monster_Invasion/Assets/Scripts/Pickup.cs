using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupTypes
    {
        Heal, DoubleBullets, ShootingCompanion
    }
    public PickupTypes pickUpType;
    bool followCompanion;
    GameObject companion;
    public void FollowCompanion(bool startFollowing)
    {
        followCompanion = startFollowing;
    }
    public bool IsFollowingCompanion()
    {
        return followCompanion;
    }
    // Start is called before the first frame update
    void Start()
    {
        companion = GameObject.FindGameObjectWithTag("Companion");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Companion")
        {
            SoundManger.PickUp();
            if (this.pickUpType == PickupTypes.ShootingCompanion)
            {
                companion.GetComponent<Companion>().StartShooting();
                Destroy(gameObject);
            }
        }
    }
    public void StartEffect()
    {
        if (this.pickUpType.Equals(PickupTypes.Heal))
        {
            Heal();
        }
        if (this.pickUpType == PickupTypes.DoubleBullets)
        {
            GameObject.FindObjectOfType<PlayerController>().ChangeCurrentBullet(2);
        }
        /*if (this.pickUpType == PickupTypes.ShootingCompanion)
        {
            Debug.Log("Collected Shooting Companion pickup!");
            StartCoroutine(companion.GetComponent<Companion>().StartShooting());
        }*/
        Destroy(gameObject);
    }
    
    public void Heal()
    {
        SoundManger.Heal();
        GameObject.FindObjectOfType<PlayerController>().health += 5;
        GameObject.FindGameObjectWithTag("PlayerHP").GetComponent<BarSystem>().Heal(5);
        GameObject.FindObjectOfType<Companion>().health += 5;
        GameObject.FindGameObjectWithTag("Companion").transform.GetChild(0).GetChild(0).GetComponent<BarSystem>().Heal(5);
    }
    // Update is called once per frame
    void Update()
    {
        if (followCompanion)
        {
            transform.position = companion.transform.position + new Vector3(0, 1.5f, 0);
        }
    }
}
