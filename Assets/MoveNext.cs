using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNext : MonoBehaviour
{
    // Start is called before the first frame update

    public void MoveObjectNext()
    {   /*t step = speed * Time.deltaTime;
        */

        GameObject Arrow = GameObject.Find("Arrow");        //Arrow and Arrow_1 have the same positions  
        GameObject Target_1 = GameObject.Find("Arrow_1");   //Arrow_1 serve the purpose of knowing where the arrow is 
        GameObject Target_2 = GameObject.Find("Arrow_2");
        GameObject Target_3 = GameObject.Find("Arrow_3");
        GameObject Target_4 = GameObject.Find("Arrow_4");
        GameObject Target_5 = GameObject.Find("Arrow_5");
        GameObject Target_6 = GameObject.Find("Arrow_6");
        GameObject Target_7 = GameObject.Find("Arrow_7");






        //Arrow_1.transform.position = Vector3.MoveTowards(Arrow_1.transform.position, nextPosition.transform.position, step);
        if (Arrow.transform.position == Target_1.transform.position)
        {
            Arrow.transform.position = Target_2.transform.position;
        }
        else if (Arrow.transform.position == Target_2.transform.position)
        {
            Arrow.transform.position = Target_3.transform.position;
        }
        else if (Arrow.transform.position == Target_3.transform.position)
        {
            Arrow.transform.position = Target_4.transform.position;
        }
        else if (Arrow.transform.position == Target_4.transform.position)
        {
            Arrow.transform.position = Target_5.transform.position;
        }
        else if (Arrow.transform.position == Target_5.transform.position)
        {
            Arrow.transform.position = Target_6.transform.position;
        }
        else if (Arrow.transform.position == Target_6.transform.position)
        {
            Arrow.transform.position = Target_7.transform.position;
        }


    }














}
