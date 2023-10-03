using System;
using UnityEngine;
using static Weapon;
public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public Sound[] sounds;

	public AudioSource ShootingChannel;

	public AudioClip M1911;
	public AudioClip Heavy;

	void Start()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	public void Play(string sound)
	{

		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s != null)
		{
			s.source.Play();
		}
	}

	

	public void Stop(string sound)
	{

		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Stop();
	}



	public void PlayShootingSound(WeaponModel weapon)
	{
		switch (weapon)
		{
			case WeaponModel.M1911:
				ShootingChannel.PlayOneShot(M1911);
				break;
			case WeaponModel.Heavy:
				ShootingChannel.PlayOneShot(Heavy);
				break;
		}
	}



	public void PlayReloadSound(WeaponModel weapon)
	{
		switch (weapon)
		{
			case WeaponModel.M1911:
				Play("M1911_Reload");
				break;
			case WeaponModel.Heavy:
				Play("Heavy_Reload");
				break;
		}
	}
}
