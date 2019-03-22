using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireLogCrack : MonoBehaviour
{
	private AudioSource audioSource;
	private Light lightSource;
	private ParticleSystem ps;

	private float nextLogCrackTime;

	// Start is called before the first frame update
	void Start()
    {
		lightSource = GetComponentInChildren<Light>();
		audioSource = GetComponent<AudioSource>();
		ps = GetComponentInChildren<ParticleSystem>();

		GameManager.Instance.InGameManager.AddLight(this.lightSource);

		lightSource.enabled = false;
		lightSource.intensity = 0;
		Play();
    }

	private void OnDestroy() {
		GameManager.Instance?.InGameManager.RemoveLight(this.lightSource);
	}

	public void Play() {
		ps.Emit(UnityEngine.Random.Range(20, 35));
		nextLogCrackTime = Time.time + UnityEngine.Random.Range(15, 20);
		StartCoroutine(LightUpThenDampen());	
	}

    // Update is called once per frame
    void Update()
    {
		if (Time.time > nextLogCrackTime) {
			Play();
		}
	}

	private IEnumerator LightUpThenDampen() {
		lightSource.enabled = true;
		while(lightSource.intensity < 0.5f) {
			lightSource.intensity += Time.deltaTime;
		}
		audioSource.Play();
		while (lightSource.intensity > 0.1f) {
			lightSource.intensity += -lightSource.intensity * 1.5f * Time.deltaTime;
			yield return null;
		}
		lightSource.intensity = 0;
		lightSource.enabled = false;
	}
}
