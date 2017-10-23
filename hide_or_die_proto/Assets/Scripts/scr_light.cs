using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_light : MonoBehaviour {
	public AudioClip sound_switch;
	public AudioClip sound_off;
	public AudioClip[] dying_noise;
	public float power;

	private AudioSource audios;
	private Light lights;
	private float low_light;
	private float prev_int;

	void Awake() {
		audios = GetComponent<AudioSource>();
		lights = GetComponent<Light>();
		low_light = 0;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			lights.enabled = !lights.enabled;
			if (lights.enabled) {
				audios.clip = sound_switch;
				audios.PlayOneShot (audios.clip);
			} else {
				audios.clip = sound_off;
				audios.PlayOneShot (audios.clip);
			}
		}
		if (lights.enabled && lights.intensity > 0) {
			lights.intensity -= power;
		}
		if (lights.intensity < 0.5) {
			if (low_light > 800 && lights.enabled) {
				int n = Random.Range(1, dying_noise.Length);
				audios.clip = dying_noise[n];
				audios.PlayOneShot(audios.clip);
				// move picked sound to index 0 so it's not picked next time
				dying_noise[n] = dying_noise[0];
				dying_noise[0] = audios.clip;
				if (lights.intensity != 0) {
					prev_int = lights.intensity;
					lights.intensity = 0;
					low_light = Random.Range (786, 789);
				} else {					
					float temp = Random.Range (0, 10);
					lights.intensity = prev_int;
					if (temp < 4) {
						low_light = Random.Range (786, 789);
					} else {						
						low_light = 0;
					}
				}
			}
			low_light += Random.Range (3, 10);
		}
	}
}
