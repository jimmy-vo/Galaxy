using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManage:MonoBehaviour
{
    public static GameObject ExplosionBulletAsteroid;
    public static GameObject ExplosionMissileAsteroid;
    public static GameObject ExplosionSpaceship;
    public static GameObject BurningSpaceship;


    private void Start()
    {
        
    }

    public static void OnAsteroid(GameObject Asteroid, GameObject Other)
    {
        if (Other.tag == "BULLET")
        {
            Asteroid.transform.GetComponent<Rigidbody>().isKinematic = true;
            Instantiate(ExplosionBulletAsteroid, Asteroid.transform.position, Asteroid.transform.rotation);

            Destroy(Asteroid);
            Destroy(Other.gameObject);

            //CreateAstoroid(transform.parent);
        }
        //else if (col.gameObject.tag == "MISSILE")
        //{
        //    transform.GetComponent<Rigidbody>().isKinematic = true;
        //    Instantiate(ExplosionMissileAsteroid, transform.position, transform.rotation);

        //    Destroy(gameObject);
        //    Destroy(col.gameObject);

        //    CreateAstoroid(transform.parent);
        //}
    }
}
