using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_light : MonoBehaviour {
	public AudioClip sound_switch;
	private AudioSource audios;
	private Light lights;
	void Awake() {
		audios = GetComponent<AudioSource>();
		lights = GetComponent<Light>();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			lights.enabled = !lights.enabled;
			audios.clip = sound_switch;
			audios.Play();
		}
		if (lights.enabled && lights.intensity > 0) {
			lights.intensity -= 0.0005f;
		}
	}
}
