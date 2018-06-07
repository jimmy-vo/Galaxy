using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpaceship : MonoBehaviour {

    public bool Enable = false;
    public bool Respawn = true;
    public float StateRateMax = 5f;
    public float StateRateMin = 3f;
    public float RateFire = 0.3f;
    public float RateLaunch = 0.01f;
    public float RateFireCenter = 0.3f;
    public float RateLaunchCenter = 0.3f;



    public GameObject[] SpaceShip;
    public float Count = 20;
    public float OffsetDestroy = 270;
    public float OffsetGenerate = 250;
    public float BurningTime = 2f;
    public float MovementSpeedMax = 5;
    public float MovementSpeedMin = 8;
    public float SteeringSpeed = .5f;

    public GameObject Bullet;
    public float FireRate = 0.3f;
    public float FireForce = 600;
    public float BulletLifeTime = 3f;
    Vector3 BulletRelativePosition = new Vector3(0, 0, 3f);

    public GameObject Missile;
    public float MissileLifeTime = 3f;
    public GameObject ExplosionSpaceship;
    public GameObject BurningSpaceship;



    enum SpaceShipState { Normal, Fire , Launch, Burning }
    float RandomControlTick = 0;
    float StateRate = 0;


    Vector3[] BulletRelativePositionData =
    {
        new Vector3(0,0,2),
        new Vector3(0,0,2.15f),
        new Vector3(0,0,2.181f),
        new Vector3(0,0, 2.114f),
        new Vector3(0,0, 2.1589f),
        new Vector3(0,0, 2.707f),
        new Vector3(0,0, 1.993f),
        new Vector3(0,0, 1.509f)
    };

    float TimeStickFire = 0;
    float TimeStickDestroy = 0;
    SpaceShipState spaceShipState = SpaceShipState.Normal;
    SpaceShipState spaceShipPreState = SpaceShipState.Normal;
    GameObject Target;

    // Use this for initialization
    void Start () {
        if (Enable)
        {
            //remove the script from ParentObject
            this.enabled = false;

            //Create Parent Object 
            GameObject ParentObject = new GameObject("SpaceShip");
            ParentObject.transform.parent = this.transform;

            for (int i = 0; i < Count; i++)
                CreateSpaceship(ParentObject.transform, OffsetDestroy * Random.insideUnitSphere);
        }
    }

    // Update is called once per frame
    void Update () {
        SpaceShipHandle();

        if (spaceShipState != SpaceShipState.Burning)
        {
            if ((transform.position - Camera.main.transform.position).magnitude > OffsetDestroy)
            {
                Destroy(this.gameObject);
                RespawnSpaceship(Respawn);
            }
            else if (spaceShipPreState == SpaceShipState.Fire)
            {
                spaceShipPreState = spaceShipState = SpaceShipState.Normal;
            }
            else if (spaceShipPreState == SpaceShipState.Normal)
            {
                spaceShipPreState = spaceShipState = SpaceShipState.Fire;
            }

            if (spaceShipState == SpaceShipState.Launch)
            {
                spaceShipState = SpaceShipState.Normal;
            }
            SpaceShipRandomControl();
        }
    }

    private void RespawnSpaceship(bool respawn)
    {
        if (respawn)
        {
            Vector3 random = OffsetGenerate * Random.onUnitSphere;
            random.z = Mathf.Abs(random.z);

            CreateSpaceship(transform.parent, random);
        }
    }

    private void CreateSpaceship(Transform parrent, Vector3 RelativePosition)
    {
        int index = (int)Random.Range(0, SpaceShip.Length - 1);
        GameObject Spaceship = Instantiate(
            SpaceShip[index],
            Camera.main.transform.TransformPoint(RelativePosition),
            transform.rotation, parrent);

        // common configuration
        Spaceship.tag = "ENEMY_SPACESHIP";

        // Script parameters
        RoomSpaceship script = Spaceship.AddComponent<RoomSpaceship>();
        script.Bullet = Bullet;
        script.Missile = Missile;
        script.ExplosionSpaceship = ExplosionSpaceship;
        script.MovementSpeedMax = MovementSpeedMax;
        script.MovementSpeedMin = MovementSpeedMin;
        script.SteeringSpeed = SteeringSpeed;
        script.BurningSpaceship = BurningSpaceship;
        script.BulletRelativePosition = BulletRelativePositionData[index];
        script.BulletRelativePositionData = BulletRelativePositionData;
        script.BurningTime = BurningTime;
        script.SpaceShip = SpaceShip;
        script.StateRateMax = StateRateMax;
        script.StateRateMin = StateRateMin;
        script.RateFire = RateFire;
        script.RateLaunch = RateLaunch;
        script.RateFireCenter = RateFireCenter;
        script.RateLaunchCenter = RateLaunchCenter;
        script.MovementSpeedMax = MovementSpeedMax;
        script.MovementSpeedMin = MovementSpeedMin;
        script.Respawn = Respawn;
        script.Enable = false;
    }


    private void SpaceShipRandomControl()
    {
        RandomControlTick += Time.deltaTime;

        if ((Target == null) || ((Target.transform.position - transform.position).magnitude < 6f))
        {
            StateRate = RandomControlTick;
        }

        if (RandomControlTick>= StateRate)
        {
            StateRate = Random.Range(StateRateMin, StateRateMax);
            RandomControlTick = 0;
            float random = Random.Range(0f, 1f);
            if (random < RateLaunch)
            {
                spaceShipState = SpaceShipState.Launch;
                GameObject[] list = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
                Target = list[Random.Range(0, list.Length - 1)];
            }
            else if (random < RateFire)
            {
                spaceShipState = SpaceShipState.Fire;
                GameObject[] list = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
                Target = list[Random.Range(0, list.Length - 1)];
            }
            else
            {
                spaceShipState = SpaceShipState.Normal;
                GameObject[] list = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
                Target = list[Random.Range(0, list.Length - 1)];
            }
            //Debug.Log(spaceShipState + " " + Target);
        }

    }

    private void SpaceShipMove()
    {
        if (Target != null)
        {
            // update direction each frame:
            Vector3 dir = Target.transform.position - transform.position;
            // calculate desired rotation:
            Quaternion rot = Quaternion.LookRotation(dir);
            // Slerp to it over time:
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, SteeringSpeed * Time.deltaTime);
        }
        // move in the current forward direction at specified speed:
        transform.Translate(transform.forward.normalized * Random.Range(MovementSpeedMin, MovementSpeedMax) * Time.deltaTime, Space.World);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
    }

    private void SpaceShipFire()
    {
        if (TimeStickFire > FireRate)
        {
            TimeStickFire = 0;
            GameObject Bullett = Instantiate(Bullet, transform.TransformPoint(BulletRelativePosition), transform.rotation);
            Bullett.tag = "BULLET";
            Bullett.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * FireForce);
            Destroy(Bullett, BulletLifeTime);
        }
        else
        {
            TimeStickFire += Time.deltaTime;
        }
    }

    private void SpaceShipHandle()
    {
        switch (spaceShipState)
        {
            case SpaceShipState.Normal:
                SpaceShipMove();
                break;
            case SpaceShipState.Fire: //move and Fire
                SpaceShipMove();
                GameObject Bullett = Instantiate(Bullet, transform.TransformPoint(BulletRelativePosition), transform.rotation);
                Bullett.tag = "BULLET";
                Bullett.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * FireForce);
                Destroy(Bullett, BulletLifeTime);
                break;
            case SpaceShipState.Launch:
                SpaceShipMove();
                Instantiate(Missile, transform.TransformPoint(BulletRelativePosition), transform.rotation, this.gameObject.transform).GetComponent<Missile>().Launch(this.gameObject, Target);
                break;
            case SpaceShipState.Burning:
                SpaceShipMove();
                if (TimeStickDestroy == BurningTime)
                {
                    Instantiate(BurningSpaceship, transform.position, transform.rotation, transform);
                }

                if (TimeStickDestroy > 0)
                {
                    TimeStickDestroy -= Time.deltaTime;
                }
                else
                {
                    Destroy(gameObject);
                    Instantiate(ExplosionSpaceship, transform.position, transform.rotation);
                    RespawnSpaceship(Respawn);
                }
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (spaceShipState != SpaceShipState.Burning)
        {
            if (col.gameObject.tag == "BULLET")
            {
                Destroy(gameObject, BurningTime + 0.5f);
                Destroy(col.gameObject);
                spaceShipState = SpaceShipState.Burning;
                TimeStickDestroy = BurningTime;
            }
            else if (col.gameObject.tag == "MISSILE")
            {

                Destroy(gameObject, BurningTime + 0.5f);
                Destroy(col.gameObject);
                spaceShipState = SpaceShipState.Burning;
                TimeStickDestroy = BurningTime;
            }
        }
    }
}
