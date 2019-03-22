using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    void Start() {
		var mr = GetComponent<MeshRenderer>();
		float atten = 0;
		foreach (Light l in GameManager.Instance.InGameManager.ActiveLights) {
			float normalizedDist = ((transform.position - l.transform.position).magnitude / l.range);
			atten += 1.5f * l.intensity / (1.0f + 25.0f * normalizedDist * normalizedDist);
		}
		atten = Mathf.Clamp01(atten);
		float attenuation;
		if (atten > 0.18f) {
			attenuation = 1;
		}
		else {
			attenuation = Mathf.Clamp01(-339.39f * Mathf.Pow(atten, 3) + 101.82f * Mathf.Pow(atten, 2) - 1.88f * atten + 0.001f);
		}
		var mpb = new MaterialPropertyBlock();
		mpb.SetFloat("_Attenuation", attenuation);
		mr.SetPropertyBlock(mpb);
	}
}
