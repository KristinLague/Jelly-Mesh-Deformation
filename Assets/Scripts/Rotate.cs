using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Axis
{
	Forward = 0,
	Left = 1,
	Right = 2,
	Up = 3,
	Down = 4
}
public class Rotate : MonoBehaviour {

    public bool rotate;
    public float speed;
    public Axis axis;

   
    void Update () {

		if(!rotate)
		{
            return;
        }

		switch(axis)
		{
			case Axis.Forward:
				gameObject.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                break;
			
			case Axis.Left:
				gameObject.transform.Rotate(Vector3.left, speed * Time.deltaTime);
                break;

			case Axis.Right:
				gameObject.transform.Rotate(Vector3.right, speed * Time.deltaTime);
                break;

			case Axis.Up:
				gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime);
                break;

			case Axis.Down:
				gameObject.transform.Rotate(Vector3.down, speed * Time.deltaTime);
                break;

        }
        
        
    }
}
