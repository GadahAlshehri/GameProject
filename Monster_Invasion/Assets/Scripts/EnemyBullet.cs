using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

	GameObject Enemybullet;
	[SerializeField]
	float moveSpeed = 6f;

	Rigidbody2D rb;

	Companion target;
	Vector2 moveDirection;
	//PlayerController Shield = new PlayerController();

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		target = GameObject.FindObjectOfType<Companion>();
		if((target != null))
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
		rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
		Destroy(gameObject,5f);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		

		if ( other.gameObject.tag == "Companion")
		{
			Debug.Log("attack");
			

			PlayerController Pc = GameObject.FindObjectOfType<PlayerController>();

			if (!target.IsShielded())
			{
				target.ChangeHealth(gameObject);
			}
			
			Destroy(gameObject);

			 }
		if (other.gameObject.tag == "Shield")
		{
			Destroy(gameObject);
		}


	}
}
