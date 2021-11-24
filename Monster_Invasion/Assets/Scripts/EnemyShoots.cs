using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoots : MonoBehaviour
{ 
	[SerializeField]
	GameObject bullet;

	float fireRate = 0;
	float nextFire;

	bool isShooting = false;

	// Use this for initialization
	void Start () {
		StartCoroutine(DelayShooting());

		nextFire = Time.time;
	}

// Update is called once per frame
void Update()
{
	CheckIfTimeToFire();

}

void CheckIfTimeToFire()
{
		fireRate = 3f;

	if (Time.time > nextFire && isShooting==true)
	{
			SoundManger.ShootingEnemy();
			Instantiate(bullet, transform.position, Quaternion.identity);
		nextFire = Time.time + fireRate;
	}

}

	 IEnumerator DelayShooting()
	{
		yield return new WaitForSeconds(10f);
				isShooting = true;

	}
}
