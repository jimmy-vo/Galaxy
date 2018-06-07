using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomSpaceLevel : MonoBehaviour
{
    public GameObject Space;
    public float SpaceScale = 5;

    public GameObject[] Planets;
    public float PlanetCount = 6;
    public float PlanetOffset = 250;
    public float PlanetScale = 6;
    public float PlanetTorque = 40;

    private GameObject spaceLevel;

    void Start()
    {
        //spaceLevel = Instantiate(Space, transform.position, Quaternion.identity, this.transform);
        spaceLevel = new GameObject("Planets");
        spaceLevel.transform.parent = this.transform;
        spaceLevel.transform.localScale = SpaceScale * Vector3.one;

        for (int i = 0; i < PlanetCount; i++)
        {
            Vector3 random = PlanetOffset * Random.onUnitSphere;
            GameObject Planet = Instantiate(Planets[(int)Random.Range(0, Planets.Length - 1)], spaceLevel.transform);
            Planet.transform.localScale = PlanetScale * Vector3.one;
            Planet.transform.position = Camera.main.transform.TransformPoint(random);

            Rigidbody gameObjectsRigidBody = Planet.AddComponent<Rigidbody>(); // Add the rigidbody.
            gameObjectsRigidBody.useGravity = false;
            gameObjectsRigidBody.AddTorque(PlanetTorque * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
        }

    }

    private void Update()
    {
        spaceLevel.transform.position = Camera.main.transform.position;
    }

}
