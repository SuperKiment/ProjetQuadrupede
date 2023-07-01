using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerToutesPattes : MonoBehaviour
{
    public string patteAvantDroiteName;
    public string patteArriereDroiteName;
    public string patteAvantGaucheName;
    public string patteArriereGaucheName;

    public ControllerPatteSeule AvantDroite;
    public ControllerPatteSeule ArriereDroite;
    public ControllerPatteSeule AvantGauche;
    public ControllerPatteSeule ArriereGauche;

    public bool makeAll = false;
    public float positionX;
    public float positionY;
    public float maxSpeed;
    public float torque;
    public float toleranceAngle;

    Dictionary<string, ControllerPatteSeule> dico;

    void Start()
    {
        positionX = -1;
        positionY = -5;
        maxSpeed = 40;
        torque = 50;
        toleranceAngle = 10;

        dico = new Dictionary<string, ControllerPatteSeule>
        {
            [patteAvantDroiteName] = AvantDroite,
            [patteArriereDroiteName] = ArriereDroite,
            [patteAvantGaucheName] = AvantGauche,
            [patteArriereGaucheName] = ArriereGauche
        };

        foreach (ControllerPatteSeule controller in transform.GetComponents<ControllerPatteSeule>())
        {
            dico[controller.patteName] = controller;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (makeAll)
        {
            foreach (ControllerPatteSeule controller in dico.Values)
            {
                controller.positionCibleX = positionX;
                controller.positionCibleY = positionY;
                controller.maxSpeed = maxSpeed;
                controller.torque = torque;
                controller.toleranceAngle = toleranceAngle;


            }
        }
    }
}
