using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballpath : MonoBehaviour
{

    public float Velocity_Magnitude = 90;
    public float Degree = 25;
    public bool Degree_Debug = false;


    public bool Velocity_Magnitude_Amplify = true;
    public bool Velocity_Magnitude_Amplify_Debug = false;

    public float Air_Resistant = 10.0f;
    public float g = 98.1f;

    private Vector3 InitialPosition;
    private Vector3 Velocity;
    private Vector2 Velocity_Init;

    public void Destroy()
    {
        Object.Destroy(this.gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        Onfire();
    }

   

    void Onfire()
    {
        transform.Translate(-Camera.main.transform.position);
        InitialPosition = transform.position;

        Vector3 cameraView_Pre = Camera.main.transform.forward - Camera.main.transform.position;
        Vector3 cameraView_Pre_xz = new Vector3(cameraView_Pre.x, 0, cameraView_Pre.z);
        Vector3 axisToRotateAround = Vector3.Cross(cameraView_Pre_xz, cameraView_Pre);
        Vector3 cameraView_Post = Quaternion.AngleAxis((cameraView_Pre.y < 0) ? -Degree : Degree, axisToRotateAround) * cameraView_Pre;

        if (Degree_Debug)
        {
            Vector3 angle_pre = new Vector3();
            angle_pre.x = Vector3.Angle(new Vector3(1, 0, 0), cameraView_Pre);
            angle_pre.y = Vector3.Angle(new Vector3(0, 1, 0), cameraView_Pre);
            angle_pre.z = Vector3.Angle(new Vector3(0, 0, 1), cameraView_Pre);

            Vector3 angle_post = new Vector3();
            angle_post.x = Vector3.Angle(new Vector3(1, 0, 0), cameraView_Post);
            angle_post.y = Vector3.Angle(new Vector3(0, 1, 0), cameraView_Post);
            angle_post.z = Vector3.Angle(new Vector3(0, 0, 1), cameraView_Post);

            Debug.Log("Camera's angles:" + angle_pre + " Throw's angles:" + angle_post + "\nDifference:" + (angle_post - angle_pre));
        }

        Velocity_Init = new Vector2(cameraView_Post.x, cameraView_Post.z);
        Velocity = cameraView_Post * Velocity_Magnitude;

        if (Velocity_Magnitude_Amplify)
        {
            float amplified_angle = Vector2.Angle(new Vector2(0, 1), Velocity_Init);
            amplified_angle = (amplified_angle > 180) ? amplified_angle - 180 : amplified_angle;
            amplified_angle = (amplified_angle > 90) ? 180 - amplified_angle : amplified_angle;
            amplified_angle *= Mathf.PI / 180;

            float amplified_magnitude =
               (amplified_angle < Mathf.PI / 4) ? Mathf.Sin(amplified_angle) : Mathf.Cos(amplified_angle);
            amplified_magnitude *= amplified_magnitude;
            amplified_magnitude = Mathf.Sqrt(amplified_magnitude * amplified_magnitude + 1);
            if (Velocity_Magnitude_Amplify_Debug)
            {
                Debug.Log(amplified_magnitude);
            }

            Velocity *= amplified_magnitude;
        }
    }

    // Update is called once per frame
    void Update () {

        //if (Input.GetButtonDown("Tap"))
        //{
        //    Onfire();
        //}


        //Newton's law
        float k = Time.deltaTime * Air_Resistant;
        Velocity.x = (Mathf.Abs(Velocity.x) - k > 0) ? Velocity.x - ((Velocity.x > 0) ? k : -k) : 0;
        Velocity.y -= g * Time.deltaTime - ((Velocity.y > 0) ? k : -k);
        Velocity.y = (Mathf.Abs(Velocity.y) - k > 0) ? Velocity.y - ((Velocity.y > 0) ? k : -k) : 0;

        //displacement
        Vector3 translateVector = Velocity * Time.deltaTime / 2;
        float distanceFromOrigin = Mathf.Abs(transform.position.y);
        float dotProduct = Vector2.Dot(Velocity_Init.normalized, new Vector2(Velocity.x, Velocity.z).normalized);
        //Debug.Log(dotProduct);
        if (distanceFromOrigin < 50)
        {
            
            transform.Translate(translateVector);
            //Debug.Log(transform.position.x + " " + transform.position.y + " " + t);
        }
        else
        {
            Destroy();
        }
    }
}
