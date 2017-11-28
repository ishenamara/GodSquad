using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

	public Transform player;
	// Use this for initialization
	void Start () {
		StartCoroutine (Target ());
	}
	
	// Update is called once per frame
	IEnumerator Target () {
		while (true) {
			if (Vector3.Distance (transform.position, player.position) < 10) {
				SendMessage ("Shoot", (player.position - transform.position).normalized);
				yield return new WaitForSeconds (0.5f);
			}
			yield return null;
		}
	}
}
