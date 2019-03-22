using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	private CapsuleCollider capsuleCollider;
	private Rigidbody rigid;

	private Animator anim;

	private Vector3 direction;

	private Vector3 currentMove;
	private Vector3 targetMove;

	private LayerMask collisionLayermask;
	private readonly static float skinWidth = 0.02f;

	private bool Sitting = false;

	private TilePosition currentTile;


	void Start() {
		capsuleCollider = GetComponent<CapsuleCollider>();
		//controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();

		collisionLayermask = ~(1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("SitArea"));
    }

    void Update() {
		// Movement
		var timeModifiedMove = currentMove * Time.deltaTime;
		var modifiedMoveAmount = timeModifiedMove.magnitude;
	
		List <RaycastHit> hits = Physics.CapsuleCastAll(
									transform.position + Vector3.up * capsuleCollider.bounds.extents.y,
									transform.position - Vector3.up * capsuleCollider.bounds.extents.y,
									capsuleCollider.radius * transform.lossyScale.x, 
									timeModifiedMove,
									modifiedMoveAmount, 
									collisionLayermask).ToList(); 
		if(hits.Count > 0) {
			if(hits[0].distance > skinWidth) {
				transform.position += currentMove.normalized * (hits[0].distance - skinWidth);
			}
		}
		else {
			transform.position += timeModifiedMove;
		}

		// Terrain Generation
		TilePosition tp = GetTiledPosition();
		if(tp != this.currentTile) {
			this.currentTile = tp;
			GameManager.Instance.InGameManager.GenerateTilesForPosition(tp);
		}

		// Aesthetics
		anim.SetFloat("Speed", currentMove.magnitude / 2f);
		anim.SetFloat("WalkingSpeed", Mathf.Max(1f, currentMove.magnitude / 2f));

		float atten = 0;
		foreach(Light l in GameManager.Instance.InGameManager.ActiveLights) {
			float normalizedDist = ((transform.position - l.transform.position).magnitude/l.range);
			atten += 1.5f * l.intensity / (1.0f + 25.0f * normalizedDist * normalizedDist);
		}
		atten = Mathf.Clamp01(atten);
		float attenuation;
		if(atten > 0.18f) {
			attenuation = 1;
		}
		else {
			attenuation = Mathf.Clamp01(-339.39f * Mathf.Pow(atten, 3) + 101.82f * Mathf.Pow(atten, 2) - 1.88f * atten + 0.001f);
		}
		Shader.SetGlobalFloat("_PlayerAttenuation", attenuation);
	}

	private TilePosition GetTiledPosition() {
		return new TilePosition(Mathf.RoundToInt(transform.position.x / 40), Mathf.RoundToInt(transform.position.z / 40));
	}

	private void FixedUpdate() {

	}

	public void HandleInput(InputPackage p) {
		if(Sitting) {
			if(p.Roll) {
				Stand();
			}
		}
		else {
			transform.rotation = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, p.MousePositionWorldSpace - transform.position, Vector3.up), 0);
			var velocity = new Vector3(p.Horizontal, 0, p.Vertical) * 4;
			currentMove = velocity;

			if(p.Roll) {
				TrySit();
			}
		}
	}

	public void TrySit() {	
		Collider[] hits = Physics.OverlapCapsule(
							transform.position + Vector3.up * capsuleCollider.bounds.extents.y,
							transform.position - Vector3.up * capsuleCollider.bounds.extents.y,
							capsuleCollider.radius * transform.lossyScale.x, 
							1 << LayerMask.NameToLayer("SitArea"));
		if(hits.Length > 0) {
			Sitting = true;
			anim.SetBool("Sitting", true);
		}
	}

	public void Stand() {
		Sitting = false;
		anim.SetBool("Sitting", false);
	}
}
