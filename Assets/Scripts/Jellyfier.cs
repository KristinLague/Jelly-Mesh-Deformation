using UnityEngine;

public class Jellyfier : MonoBehaviour
{
	//A value that describes how fast our jelly object will be bouncing
    public float bounceSpeed;
    public float fallForce;

    //We need this value to eventually stop bouncing back and forth.
    public float stiffness;

    //We need our Meshfilter to get a hold of the mesh;
    private MeshFilter meshFilter;
    private Mesh mesh;


    //We need to keep track of our vertices. 
    //This means not only the current stat of them but also there original position and so forth;
    Vector3[] initialVertices;
    Vector3[] currentVertices;

    Vector3[] vertexVelocities;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        //fallForce = Random.Range(25, 80);

        //Getting our vertices (initial and their current state(which is initial since we havent done anything yet, duh))
        initialVertices = mesh.vertices;

        //Obviously we are never changing the actual count of vertices so these two Arrays will always have the same length
        currentVertices = new Vector3[initialVertices.Length];
        vertexVelocities = new Vector3[initialVertices.Length];
        for (int i = 0; i < initialVertices.Length; i++)
        {
            currentVertices[i] = initialVertices[i];
        }
    }

	private void Update()
	{
        UpdateVertices();
    }

	private void UpdateVertices()
	{
		//We are looping through every vertice  update them depending on their velocity.
        for (int i = 0; i < currentVertices.Length; i++)
		{
            //Before we add the current velocity to the vertice we need to make sure that
            //we consider the fact that our object is a jelly and should be able to bounce back 
            //to do so we first calculate the displacement value. 
            //Since we saved the initial form of the mesh we can use this to revert back to the inital 
            //position over time
            Vector3 currentDisplacement = currentVertices[i] - initialVertices[i];
            vertexVelocities[i] -= currentDisplacement * bounceSpeed * Time.deltaTime;

            //In order for us to be able to stop bouncing at one point we need to reduce
            //the velocity over time. 
            vertexVelocities[i] *= 1f - stiffness * Time.deltaTime;
            currentVertices[i] += vertexVelocities[i] * Time.deltaTime;

        }

		//We then need to set our mesh.vertices to the current vertices 
		//in order to be able to see a change.
        mesh.vertices = currentVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

    }

    public void OnCollisionEnter(Collision other)
    {
        ContactPoint[] collisonPoints = other.contacts;
        for (int i = 0; i < collisonPoints.Length; i++)
        {
            Vector3 inputPoint = collisonPoints[i].point + (collisonPoints[i].point * .1f);
            ApplyPressureToPoint(inputPoint, fallForce);
        }
    }

    public void ApplyPressureToPoint(Vector3 _point, float _pressure)
    {
        //We need to loop through every single vertice and apply the pressure to it.
        for (int i = 0; i < currentVertices.Length; i++)
        {
            ApplyPressureToVertex(i, _point, _pressure);
        }
    }

    public void ApplyPressureToVertex(int _index, Vector3 _position, float _pressure)
    {
        //In order to know how much pressure we need to apply to each vertice we need to 
        //calculate the distance between our vertice and the point where our extended finger (mouse thingey) 
        //touched our mesh
        Vector3 distanceVerticePoint = currentVertices[_index] - transform.InverseTransformPoint(_position);

        //now begins fun physiquee part.... we need to make use of the Inverse Square Law
        //we do this by dividing the pressure by the distance squared into an adapted pressure
        //TODO: CHANGE
        float adaptedPressure = _pressure / (1f + distanceVerticePoint.sqrMagnitude);

        //What is left to do now is to use this pressure to calculate a velocity for vertices.
        //TODO - First claculate acceleration with mass acceleration = force / mass and then get the
        //velocity by calculating acceleration * Time.deltaTime;
        float velocity = adaptedPressure * Time.deltaTime;
        //Our velocity now still needs a direction, we can calculate this using the
        //normalized distance vertex point from earlier
        vertexVelocities[_index] += distanceVerticePoint.normalized * velocity;













        

    }
}
