using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private InputPackage p = new InputPackage();
	private Camera main;
	private LayerMask groundMask;

	private void Start() {
		main = Camera.main;
		groundMask = 1 << LayerMask.NameToLayer("Ground");
	}

	private void Update() {
		AddCommonControls();
	}

	protected void AddCommonControls() {	
		p.Vertical = Input.GetAxis("Vertical");
		p.Horizontal = Input.GetAxis("Horizontal");
		p.Throw = Input.GetButton("Throw");
		p.Roll = Input.GetButtonDown("Roll");
		p.Attack = Input.GetButtonDown("Attack");


		Ray ray = main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out var rayHit, 20, groundMask)) {
			Debug.DrawLine(main.transform.position, rayHit.point, Color.yellow);
			p.MousePositionWorldSpace = rayHit.point;
		}
		GameManager.Instance.HandleInput(p);
	}
}

public class InputPackage {
	public float Horizontal { get; set; }
	public float Vertical { get; set; }
	public bool Roll { get; set; }
	public bool Attack { get; set; }
	public bool Throw { get; set; }

	public Vector3 MousePositionWorldSpace { get; set; }
}
