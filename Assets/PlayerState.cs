using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour {
    private enum SpaceShipState {Shoot, Normal , Fire, Arming, Launch}

    public float MovementSpeed = 8f;
    public float SteeringSpeed = 1.3f;

    public GameObject Bullet;
    public Vector3[] BulletRelativePosition = { new Vector3(-0.396f, -0.066f, 1.233f), new Vector3(0.396f, -0.066f, 1.233f) };
    public float BulletRate = 0.1f;
    public float BulletForce = 1000;
    public float BulletLifeTime = 2f;

    public GameObject Missile;
    public Vector3[] MissileRelativePosition = { new Vector3(0, -0.2f, 1.83f) };
    public float MissileLauchingDistance = 2f;
    public float MissileLauchingForce = 1;
    public float MissileTargetingTime = 1f;
    public float MissileFiringForce = 50;
    public float MissileSteerFactor = 0.1f;
    public float MissileLifetime = 5;

    float TimeStickFire = 0;
    SpaceShipState spaceShipState = SpaceShipState.Normal;
    SpaceShipState spaceShipPreState = SpaceShipState.Normal;

    private void Start()
    {
        spaceShipState = SpaceShipState.Normal;
        spaceShipPreState = SpaceShipState.Normal;
    }
    

    void Update()
    {
        switch(PlayerClickHandle.ClickEvent)
        {
            case PlayerClickHandle.ClickState.Click:
                if (spaceShipState == SpaceShipState.Normal)
                {
                    spaceShipState = SpaceShipState.Shoot;
                }
                break;
            case PlayerClickHandle.ClickState.LongPress:
                spaceShipState = SpaceShipState.Arming;
                break;
            case PlayerClickHandle.ClickState.DoubleClick:
                if (spaceShipState == SpaceShipState.Fire) 
                {
                    spaceShipPreState = spaceShipState = SpaceShipState.Normal;
                }
                else if (spaceShipState == SpaceShipState.Normal) 
                {
                    spaceShipPreState = spaceShipState = SpaceShipState.Fire;
                }
                break;
            default:
                if (spaceShipState == SpaceShipState.Arming)
                {
                    spaceShipState = SpaceShipState.Launch;
                }
                else if (spaceShipState == SpaceShipState.Launch)
                {
                    spaceShipState = spaceShipPreState;
                }
                else if (spaceShipState == SpaceShipState.Shoot)
                {
                    spaceShipState = SpaceShipState.Normal;
                }
                break;
        }
        
        SpaceShipHandle();
    }

    private void SpaceShipMove(bool FollowCamera)
    {
        //Follow Camera
        if (FollowCamera)
        {
            if (!transform.rotation.Equals(Camera.main.transform.rotation))
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Camera.main.transform.rotation, Time.deltaTime * SteeringSpeed);
            }
        }
        else
        {
        }
        transform.Translate(transform.forward.normalized * MovementSpeed * Time.deltaTime, Space.World);
        //transform.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * MovementSpeed * Time.deltaTime);
    }

    private void SpaceShipFire()
    {
        if (TimeStickFire > BulletRate)
        {
            TimeStickFire = 0;
            foreach (Vector3 item in BulletRelativePosition)
            {
                GameObject Bullett = Instantiate(Bullet, transform.TransformPoint(item), transform.rotation);
                Bullett.tag = "BULLET";
                Bullett.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * BulletForce);
                Destroy(Bullett, BulletLifeTime);
            }
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
                this.SpaceShipMove(true);
                break;
            case SpaceShipState.Shoot: //move and Fire
            case SpaceShipState.Fire: //move and Fire
                this.SpaceShipMove(true);
                this.SpaceShipFire();
                break;
            case SpaceShipState.Arming:  //do something until Launch signal
                this.SpaceShipMove(false);
                if (spaceShipPreState == SpaceShipState.Fire)
                {
                    this.SpaceShipFire();
                }

                break;
            case SpaceShipState.Launch:
                this.SpaceShipMove(true);
                foreach (Vector3 item in MissileRelativePosition)
                {
                    Missile missile = Instantiate(Missile, transform.TransformPoint(item), transform.rotation, this.gameObject.transform).GetComponent<Missile>();
                    missile.LauchingForce = MissileLauchingForce;
                    missile.LauchingDistance = MissileLauchingDistance;
                    missile.FiringForce = MissileFiringForce;
                    missile.TargetingTime = MissileTargetingTime;
                    missile.Lifetime = MissileLifetime;
                    missile.SteerFactor = MissileSteerFactor;
                    missile.Launch(this.gameObject, PlayerTarget.Target);
                }
                break;
            default:
                break;

        }
    }
}
