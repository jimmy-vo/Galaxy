using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    private enum MissileState  {Launching, Targeting, Following};
    public Rigidbody rb;

    public float LauchingForce = 1;
    public float LauchingDistance = 2f;

    public float FiringForce = 50;
    public float TargetingTime = 1f;

    public float Lifetime = 5;
    public float SteerFactor = .1f;

    public GameObject ExplosionMissile;


    MissileState missileState = MissileState.Launching;
    GameObject LaunchBase;
    GameObject Target;
    float timeStick = 0;

    // Use this for initialization
    void Start () {
        this.gameObject.tag = "MISSILE";
        rb = GetComponent<Rigidbody>();
    }    
    
    public void Launch(GameObject lastObject, GameObject forward)
    {
        LaunchBase = lastObject;
        Target = forward;
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        missileState = MissileState.Launching;
    }

    // Update is called once per frame
    void Update ()
    {
        switch (missileState)
        {
            case MissileState.Launching:
                if (LaunchBase.transform.InverseTransformPoint(transform.position).z < LauchingDistance)
                {
                    transform.Translate(transform.forward.normalized * LauchingForce * Time.deltaTime, Space.World);
                }
                else
                {
                    this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    this.gameObject.GetComponent<SphereCollider>().enabled = true;
                    timeStick = 0;
                    missileState = MissileState.Targeting;
                    this.transform.parent = null;
                    GetComponent<Light>().enabled = true;
                }
                break;
            case MissileState.Targeting:
                timeStick += Time.deltaTime;
                if (timeStick< TargetingTime)
                {
                    // y = (s2-s1)/t * x + s1
                    transform.Translate(transform.forward.normalized * Time.deltaTime *
                        ((FiringForce - LauchingForce) / TargetingTime * timeStick + LauchingForce)
                        , Space.World);

                    if (Target != null)
                    {
                        Vector3 dir = Target.transform.position - transform.position;

                        Quaternion rot = Quaternion.LookRotation(dir);

                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, SteerFactor*timeStick / TargetingTime); 
                    }
                }
                else
                {
                    missileState = MissileState.Following;
                    timeStick = 0;
                }
                break;
            case MissileState.Following:
                timeStick += Time.deltaTime;
                if (timeStick < Lifetime)
                {
                    // move in the current forward direction at specified speed:
                    transform.Translate(transform.forward.normalized * FiringForce * Time.deltaTime, Space.World);

                    if (Target != null)
                    {
                        Vector3 dir = Target.transform.position - transform.position;

                        Quaternion rot = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
                    }

                }
                else
                {
                    Instantiate(ExplosionMissile).transform.position = transform.position;
                    Destroy(this.gameObject);
                }
                break;
            default:
                break;
        }
    }

}
