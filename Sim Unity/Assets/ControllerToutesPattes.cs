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
    public bool avancer = true;
    public float positionX;
    public float positionY;
    public float maxSpeed;
    public float torque;
    public float toleranceAngle;
    public float speedMarche;
    public float amplitudeX;
    public float amplitudeY;
    public float hauteurY;

    Dictionary<string, ControllerPatteSeule> dico;

    void Start()
    {

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

        if (avancer)
        {
            float cosX = getCos(0);

            float cosY = getCos(Mathf.PI * 3 / 2);
            if (cosY < 0) cosY = 0;

            dico[patteAvantDroiteName].setPosition(cosX * amplitudeX, cosY * amplitudeY - hauteurY);
            dico[patteArriereGaucheName].setPosition(cosX * amplitudeX, cosY * amplitudeY - hauteurY);

            float cosX2 = getCos(Mathf.PI);

            float cosY2 = getCos(Mathf.PI / 2);
            if (cosY2 < 0) cosY2 = 0;

            dico[patteArriereDroiteName].setPosition(cosX2 * amplitudeX, cosY2 * amplitudeY - hauteurY);
            dico[patteAvantGaucheName].setPosition(cosX2 * amplitudeX, cosY2 * amplitudeY - hauteurY);
        }
    }

    private float getCos(float delay)
    {
        return Mathf.Cos(Time.time / speedMarche - delay);

    }
}
