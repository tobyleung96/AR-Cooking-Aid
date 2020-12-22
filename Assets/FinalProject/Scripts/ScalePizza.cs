
using UnityEngine;
using System.Collections;

public class ScalePizza : MonoBehaviour {
	
	
	public float initialDistance;
	public Vector3 initialScale;
	public static Transform ScaleTransform;
	
	
	void  Update (){
		int fingersOnScreen = 0;
		
		foreach(Touch touch in Input.touches) {
			fingersOnScreen++; 
			
			if(fingersOnScreen == 2){
				
				if(touch.phase == TouchPhase.Began){
					initialDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
					initialScale = ScaleTransform.localScale;
				}
				else{
					float curDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
					
					float scaleFactor = curDistance / initialDistance;
					
					ScaleTransform.localScale = initialScale * scaleFactor; 
				}
			}
		}
	}
	
}