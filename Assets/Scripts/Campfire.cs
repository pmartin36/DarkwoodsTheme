using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(AudioSource))]
public class Campfire : MonoBehaviour
{
	public Light lightSource;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponentInChildren<Light>();
		StartCoroutine(LightAnimation());
		GameManager.Instance.InGameManager.AddLight(this.lightSource);
	}

	private void OnDestroy() {
		GameManager.Instance?.InGameManager.RemoveLight(this.lightSource);
	}

	private IEnumerator LightAnimation() {
		while(true) {
			float time = 0;
			//float targetLightIntensity = lightSource.intensity > 1.75f ?
			//	Random.Range(1.25f, 1.75f) :
			//	Random.Range(1.75f, 2.25f);
			float targetLightIntensity = Random.Range(1.25f, 2.25f);
			float timeToTarget = Random.Range(2f, 3f);
			float startingIntensity = lightSource.intensity;

			while(time < timeToTarget) {	
				lightSource.intensity = Mathf.Lerp(startingIntensity, targetLightIntensity, time / timeToTarget);
				lightSource.transform.localPosition = new Vector3(lightSource.transform.localPosition.x, 1.5f + lightSource.intensity / 4f, lightSource.transform.localPosition.z);
				time += Time.deltaTime;
				yield return null;
			}
		}
	}
}
