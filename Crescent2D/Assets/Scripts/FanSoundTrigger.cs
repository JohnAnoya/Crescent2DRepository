using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSoundTrigger : MonoBehaviour
{
	public AudioClip fanSnd;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			AudioManager.instance.alterPitchEffect(fanSnd);
		}
	}
}
