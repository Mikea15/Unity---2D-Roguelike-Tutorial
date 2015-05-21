using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager Instance = null;

	[SerializeField] private AudioSource _fxSource;
	[SerializeField] private AudioSource _musicSource;
	public AudioSource MusicSource {
		get { return _musicSource; }
	}

	[SerializeField] float _lowPitchRange = 0.95f;
	[SerializeField] float _highPitchRange = 1.05f;
	
	void Awake () 
	{
		if( Instance == null )
			Instance = this;
		else if( Instance != this )
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void PlaySingle( AudioClip clip )
	{
		_fxSource.clip = clip;
		_fxSource.Play();
	}

	public void RandomizeFx( AudioClip[] clips )
	{
		int index = Random.Range (0, clips.Length);
		float pitch = Random.Range (_lowPitchRange, _highPitchRange);

		_fxSource.pitch = pitch;
		_fxSource.clip = clips[index];
		_fxSource.Play();
	}
}
