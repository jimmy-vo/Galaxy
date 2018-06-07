using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAsteroid : MonoBehaviour {

    public GameObject[] Astoroids;
    public GameObject ExplosionBulletAsteroid;
    public GameObject ExplosionMissileAsteroid;

    public bool Enable = false;

    public float Count = 250;
    public float OffsetDestroy = 270;
    public float OffsetGenerate = 250;
    public float MaxScale = 2;
    public float Torque = 90;


    void Start ()
    {
        if (Enable)
        {
            //remove the script from ParentObject
            this.enabled = false;

            //Create Parent Object 
            GameObject ParentObject = new GameObject("Asteroid");
            ParentObject.transform.parent = this.transform;

            for (int i = 0; i < Count; i++)
                CreateAstoroid(ParentObject.transform, OffsetDestroy * Random.insideUnitSphere);
        }
    }
	
	void Update ()
    {
        if ((transform.position - Camera.main.transform.position).magnitude > OffsetDestroy)
        {
            Destroy(this.gameObject);

            Vector3 random = OffsetGenerate * Random.onUnitSphere;
            random.z = Mathf.Abs(random.z);

            CreateAstoroid(transform.parent, random);
        }
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "BULLET")
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
            Instantiate(ExplosionBulletAsteroid, transform.position, transform.rotation);



            Vector3 random = OffsetGenerate * Random.onUnitSphere;
            random.z = Mathf.Abs(random.z);

            CreateAstoroid(transform.parent, random);
        }
        else if (col.gameObject.tag == "MISSILE")
        {
            Destroy(gameObject);
            Destroy(col.gameObject);
            Instantiate(ExplosionMissileAsteroid, transform.position, transform.rotation);


            Vector3 random = OffsetGenerate * Random.onUnitSphere;
            random.z = Mathf.Abs(random.z);

            CreateAstoroid(transform.parent, random);
        }
    }

    private void CreateAstoroid(Transform parrent, Vector3 RelativePosition)
    {        
        GameObject Astoroid = Instantiate(
            Astoroids[(int)Random.Range(0, Astoroids.Length - 1)],
            Camera.main.transform.TransformPoint(RelativePosition),
            transform.rotation, 
            parrent);

        // common configuration
        Astoroid.tag = "ASTOROID";
        Astoroid.transform.localScale *= Random.Range(1, MaxScale);

        // Script parameters
        RoomAsteroid script = Astoroid.AddComponent<RoomAsteroid>();
        script.ExplosionBulletAsteroid = ExplosionBulletAsteroid;
        script.ExplosionMissileAsteroid = ExplosionMissileAsteroid;
        script.OffsetDestroy = OffsetDestroy;
        script.OffsetGenerate = OffsetGenerate;
        script.Astoroids = Astoroids;
        script.Count = Count;
        script.Torque = Torque;
        script.MaxScale = MaxScale;
        script.Enable = false;

        Astoroid.GetComponent<Rigidbody>().AddTorque(Torque * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
    }
}
