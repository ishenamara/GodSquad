using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour {
	public float speed = 200f;
	public GameObject prefab;

	// Use this for initialization
	void Start () {
		//prefab = Resources.Load ("projectile") as GameObject;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot(Vector3 direction)
	{
		GameObject projectile = Instantiate (prefab) as GameObject;
		projectile.transform.position = transform.position + direction * 2;
		Rigidbody rb = projectile.GetComponent<Rigidbody>();
		rb.velocity = direction * speed;
	}
}
