using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPatteSeule : MonoBehaviour
{
    public string patteName;

    public float maxSpeed;
    public float toleranceAngle;
    public float torque;
    public float positionCibleX;
    public float positionCibleY;

    public GameObject patte;

    private HingeJoint arriereCorps;
    private HingeJoint avantArriere;
    private HingeJoint avantPatte;

    private float upperArmLength = 3.5f;
    private float lowerArmLength = 3.5f;

    void Start()
    {
        patte = GameObject.Find(patteName);
        arriereCorps = patte.transform.Find("Arrière Patte").GetComponent<HingeJoint>();
        avantArriere = patte.transform.Find("Avant Patte").GetComponent<HingeJoint>();
        avantPatte = patte.transform.Find("Pied").GetComponent<HingeJoint>();

        arriereCorps.GetComponent<HingeJoint>().connectedBody = transform.GetComponent<Rigidbody>();

        InitMotor(arriereCorps);
        InitMotor(avantArriere);
        InitMotor(avantPatte);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = CalculateLegAngles(upperArmLength, lowerArmLength, positionCibleX, positionCibleY);
        //Debug.Log(angles);

        setAngle(arriereCorps, angles.x, 1);
        setAngle(avantArriere, angles.y, 2);
        setAngle(avantPatte, angles.z, 3);
    }

    private void setTargetSpeed(float speed, HingeJoint hinge, int placement)
    {
        var motor = hinge.motor;
        motor.targetVelocity = speed*placement;
        hinge.motor = motor;
    }

    private void setAngle(HingeJoint hinge, float angle, int placement)
    {
        if (Mathf.Abs(hinge.angle - angle) > toleranceAngle)
        {
            hinge.useMotor = true;

            if (hinge.angle < angle)
                setTargetSpeed(maxSpeed*(Mathf.Abs(hinge.angle - angle)/90), hinge, placement);
            else setTargetSpeed(-maxSpeed * (Mathf.Abs(hinge.angle - angle) / 90), hinge, placement);
        }
        else
        {
            hinge.useMotor = false;
        }
    }

    private void InitMotor(HingeJoint hinge)
    {
        var motor3 = hinge.motor;
        motor3.force = torque;
        motor3.targetVelocity = 0;
        motor3.freeSpin = false;

        hinge.motor = motor3;
        hinge.useMotor = true;

        var spring = hinge.spring;
        spring.spring = 50;
        spring.damper = 50;
        hinge.spring = spring;

        //hinge.useSpring = true;
    }

    private Vector3 CalculateLegAngles(float femurLength, float tibiaLength, float ankleX, float ankleY)
    {
        // Convert ankle coordinates to local coordinates (relative to the hip)
        Vector3 anklePosition = new Vector3(ankleX, ankleY, 0);

        // Calculate the distance between the hip and ankle
        float hipAnkleDistance = anklePosition.magnitude;

        // Check if the desired position is within reach
        float maxLegLength = femurLength + tibiaLength;
        if (hipAnkleDistance > maxLegLength)
        {
            Debug.LogWarning("Desired ankle position is out of reach!");
            anklePosition = anklePosition.normalized * maxLegLength; // Set ankle to maximum reachable distance
            hipAnkleDistance = maxLegLength;
        }

        // Calculate the angles
        float hipAngle = Mathf.Rad2Deg * Mathf.Acos(Mathf.Deg2Rad * (
            Mathf.Pow(femurLength, 2) + Mathf.Pow(hipAnkleDistance, 2) - Mathf.Pow(tibiaLength, 2) / 2 * femurLength * ankleY
            )/Mathf.PI);


        float kneeAngle = -2*hipAngle;

        hipAngle += Mathf.Rad2Deg * Mathf.Acos(-ankleX / ankleY) - 90;

        float ankleAngle = -(hipAngle + kneeAngle);

        // Return the angles as a Vector3
        return new Vector3(hipAngle, kneeAngle, ankleAngle);
    }

    public void setPosition(float x, float y)
    {
        positionCibleX = x;
        positionCibleY = y;
    }
}
