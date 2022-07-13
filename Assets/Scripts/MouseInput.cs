using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    public float pressureForce;
	//We need to apply an offset when adding force to our 
	//nesh to ensure that the vertices arent being pushed apart but rather 
	//pushed inwards - therefore we are combining this offset with the input points normal
    public float pressureOffset;

    private Ray mouseRay;
    private RaycastHit raycastHit;

    private void Update()
	{
		//We are using the mouse button as our poking device
		if(Input.GetMouseButton(0))
		{
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			//We need to check if we are clicking on a mesh we can deform
			if(Physics.Raycast(mouseRay, out raycastHit))
			{
                Jellyfier jellyfier = raycastHit.collider.GetComponent<Jellyfier>();
				if(jellyfier != null)
				{
                    Vector3 inputPoint = raycastHit.point + (raycastHit.normal * pressureOffset);
                    jellyfier.ApplyPressureToPoint(inputPoint, pressureForce);
                }
            }
			
        }
	}
}
