
using UnityEngine;

public class enemy : MonoBehaviour {

	public float health = 50f;

	public void TakeDamage (float amout)
	{

		health -= amout;
		if (health <=0f)
		{
			Die();

		}
	}
	void Die ()
	{
		Destroy (gameObject);
	}
}
