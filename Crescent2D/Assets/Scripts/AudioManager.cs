using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
	public AudioSource soundEffectSource;
	public AudioSource musicSource;

	public AudioSource deathMusicSource;

	public bool levelMusic = true;
	public bool deathMusic = false;

	public static AudioManager instance = null;

	public Character character;

	// will be used to alter the pitch of audio effects for variation of sound.
	public float lowPitch = 0.95f;
	public float highPitch = 1.05f;

	// Start is called before the first frame update
	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}
	}


	// when called will play a single audio clip
	public void PlayAudioClip(AudioClip audio)
	{
		levelMusic = true;
		deathMusic = false;
		soundEffectSource.clip = audio;
		musicSource.PlayOneShot(audio);
	}


	// will alter the clip of multiple audio tracks using the low and high pitch variables 
	public void alterPitchEffect(params AudioClip [] audioClips)
	{
		int randomNum = Random.Range(0, audioClips.Length);
		float randomPitch = Random.Range(lowPitch, highPitch);

		soundEffectSource.pitch = randomPitch;
		soundEffectSource.clip = audioClips[randomNum];
		soundEffectSource.Play();
	}

	public void DeathMusicPlay()
	{
		if (musicSource.isPlaying)
		{
			levelMusic = false;
			musicSource.Stop();
		}
		if (!deathMusicSource.isPlaying && deathMusic == false)
		{
			deathMusicSource.Play();
			deathMusic = true;
		}
	}

}
