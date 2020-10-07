using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
	public LayerMask KillPlaneLayer;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (ContactPoint2D point in collision.contacts)
			if(KillPlaneLayer.ContainsLayer(point.collider.gameObject.layer))
				UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
	
}