using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public GameObject MyArrow;
    public GameObject TopLeft;
    public GameObject TopRight;
    public GameObject ButtomLeft;
    public GameObject ButtomRight;

    int StepCounter = 0;

    public void NextPosition()
    {
        StepCounter++;

        switch (StepCounter)
        {
            case 0:
                MyArrow.transform.position = new Vector3(0.305f,0,0);
                break;
            case 1:
                MyArrow.transform.position = TopLeft.transform.position;
                MyArrow.transform.rotation = TopLeft.transform.rotation;
                break;
            case 2:
                MyArrow.transform.position = TopRight.transform.position;
                MyArrow.transform.rotation = TopRight.transform.rotation;
                break;
            case 3:
                MyArrow.transform.position = ButtomLeft.transform.position;
                MyArrow.transform.rotation = ButtomLeft.transform.rotation;
                break;
            case 4:
                MyArrow.transform.position = ButtomRight.transform.position;
                MyArrow.transform.rotation = ButtomRight.transform.rotation;
                break;
            default:
                StepCounter = 0;
                break;
        }
    }
}
