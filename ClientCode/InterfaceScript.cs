using UnityEngine;
using System.Collections;

public class InterfaceScript : MonoBehaviour {
	
	void Start () {
	
	}

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				if(hit.transform.parent != null) {
					if(hit.transform.parent.gameObject.name == "Card") {
						CardScript cs = hit.transform.parent.gameObject.GetComponent<CardScript>();
						cs.flip();
					}
				}
			}
		}
	}
}
