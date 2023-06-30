using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPatteSeule : MonoBehaviour
{
    public string patteName;

    public float maxSpeed;
    public float toleranceAngle;
    public float torque;
    public float angleCibleArriere;
    public float angleCibleAvant;
    public float angleCiblePatte;

    public GameObject patte;

    private HingeJoint arriereCorps;
    private HingeJoint avantArriere;
    private HingeJoint avantPatte;

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
        setAngle(arriereCorps, angleCibleArriere);
        setAngle(avantArriere, angleCibleAvant);
        setAngle(avantPatte, angleCiblePatte);


    }

    private void setTargetSpeed(float speed, HingeJoint hinge)
    {
        var motor = hinge.motor;
        motor.targetVelocity = speed;
        hinge.motor = motor;
    }

    private void setAngle(HingeJoint hinge, float angle)
    {
        if (Mathf.Abs(hinge.angle - angle) > toleranceAngle)
        {
            hinge.useMotor = true;
            if (hinge.angle < angle)
                setTargetSpeed(maxSpeed, hinge);
            else setTargetSpeed(-maxSpeed, hinge);
        } else
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
    }
}
