using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Meshes
{
    UnitySphere = 0,
    GenericSphereLow = 1,
	GenericSphereHigh = 2,
	IcoSphereLow = 3,
	IcoSphereHigh = 4
}

public enum MeshMaterial
{
    Checker = 0,
    Jelly = 1,
    Colorchange = 2
}

public class DemoUI : MonoBehaviour 
{
    public Toggle rotate;
    public Dropdown axis;
    public Slider rotationSpeed;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<Material> materialPrefabs = new List<Material>();

    public Dropdown meshes;
    public Dropdown materials;

    public Slider pressure;
    public Slider stiffness;
    public Slider bounceSpeed;


    private GameObject currentJellyGameObject;
    private Jellyfier currentJellyfier;
    private Rotate currentRotate;


    private void Start()
	{
        currentJellyfier = FindObjectOfType<Jellyfier>();
        currentJellyGameObject = currentJellyfier.gameObject;
        currentRotate = currentJellyGameObject.GetComponent<Rotate>();

        SetupButtons();

    }

	private void SetupButtons()
	{
        rotate.isOn = currentRotate.rotate;
        rotate.onValueChanged.AddListener(delegate { currentRotate.rotate = rotate.isOn; });

        rotationSpeed.value = currentRotate.speed;
        rotationSpeed.minValue = 1f;
        rotationSpeed.maxValue = 90f;
        rotationSpeed.onValueChanged.AddListener(delegate { currentRotate.speed = rotationSpeed.value; });

        List<Dropdown.OptionData> axisOptions = new List<Dropdown.OptionData>();
        axisOptions.Add(new Dropdown.OptionData("Vector3.forward"));
		axisOptions.Add(new Dropdown.OptionData("Vector3.left"));
		axisOptions.Add(new Dropdown.OptionData("Vector3.right"));
		axisOptions.Add(new Dropdown.OptionData("Vector3.up"));
		axisOptions.Add(new Dropdown.OptionData("Vector3.down"));

		axis.ClearOptions();
        axis.AddOptions(axisOptions);
        axis.value = 0;
        axis.onValueChanged.AddListener(delegate { currentRotate.axis = (Axis) axis.value; });

        List<Dropdown.OptionData> meshOptions = new List<Dropdown.OptionData>();
        meshOptions.Add(new Dropdown.OptionData("Unity Primitive Sphere"));
        meshOptions.Add(new Dropdown.OptionData("Generic Sphere (Low)"));
        meshOptions.Add(new Dropdown.OptionData("Generic Sphere (High)"));
        meshOptions.Add(new Dropdown.OptionData("Icosphere (Low)"));
        meshOptions.Add(new Dropdown.OptionData("Icosphere (High)"));

        meshes.ClearOptions();
        meshes.AddOptions(meshOptions);
        meshes.value = 0;
        meshes.onValueChanged.AddListener(delegate { ChangeMesh(); });

        List<Dropdown.OptionData> materialOptions = new List<Dropdown.OptionData>();
        materialOptions.Add(new Dropdown.OptionData("Checkered"));
        materialOptions.Add(new Dropdown.OptionData("Jelly"));
        materialOptions.Add(new Dropdown.OptionData("Colorchange"));

        materials.ClearOptions();
        materials.AddOptions(materialOptions);
        materials.value = 0;
        materials.onValueChanged.AddListener(delegate { 
            currentJellyGameObject.GetComponent<MeshRenderer>().material = materialPrefabs[materials.value]; });

        pressure.minValue = 0.01f;
        pressure.maxValue = 20f;
        pressure.value = Camera.main.GetComponent<MouseInput>().pressureForce;
        pressure.onValueChanged.AddListener(delegate {
            Camera.main.GetComponent<MouseInput>().pressureForce = pressure.value;
        });

        stiffness.minValue = .1f;
        stiffness.maxValue = 10f;
        stiffness.value = currentJellyfier.stiffness;
        stiffness.onValueChanged.AddListener(delegate
        {
            currentJellyfier.stiffness = stiffness.value;
        });

        bounceSpeed.minValue = 5f;
        bounceSpeed.maxValue = 100f;
        bounceSpeed.value = currentJellyfier.bounceSpeed;
        bounceSpeed.onValueChanged.AddListener(delegate
        {
            currentJellyfier.bounceSpeed = bounceSpeed.value;
        });
    }

    private void ChangeMesh()
    {
        Destroy(currentJellyGameObject);
        currentJellyGameObject = Instantiate(prefabs[meshes.value]);
        currentJellyfier = currentJellyGameObject.GetComponent<Jellyfier>();
        currentRotate = currentJellyGameObject.GetComponent<Rotate>();
        materials.value = 0;
        pressure.value = meshes.value == 0 ? 2f : .04f;
        pressure.minValue = meshes.value == 0 ? 1f : .001f;
        pressure.maxValue = meshes.value == 0 ? 10f : .5f;

        Camera.main.GetComponent<MouseInput>().pressureForce = pressure.value;
        axis.value = (int)currentRotate.axis;
        rotationSpeed.value = currentRotate.speed;

    }

    
}
