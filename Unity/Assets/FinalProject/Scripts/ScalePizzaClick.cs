using UnityEngine;
using System.Collections;

public class ScalePizzaClick : MonoBehaviour {
	void OnMouseDown() {
		ScalePizza.ScaleTransform = this.transform;
	}
}