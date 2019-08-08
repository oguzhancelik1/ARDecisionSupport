using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{




    int counter = 0;
    public void ButtonClick()
    {
        
        GameObject MyArrow_1 = GameObject.Find("MyArrow_1");
        GameObject MyArrow_2 = GameObject.Find("MyArrow_2");
        GameObject MyArrow_3 = GameObject.Find("MyArrow_3");
        GameObject Instruction_1 = GameObject.Find("Instruction_1");
        GameObject Instruction_2 = GameObject.Find("Instruction_2");
        GameObject DoneMessage = GameObject.Find("DoneMessage");
        GameObject FirstMessage = GameObject.Find("FirstMessage");
        GameObject Button = GameObject.Find("Button");


        //MyArrow_1.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
        

        counter++;
        switch (counter)
        {
           /* case 0:
                FirstMessage.SetActive(true);
                break;*/
            case 1:
                
                
                //Instruction_1.GetComponent<MeshRenderer>().enabled = false;
                Instruction_1.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                Instruction_2.transform.localScale = new Vector3(1f, 1f, 1f);
                //Instruction_2.GetComponent<MeshRenderer>().enabled = true;
                MyArrow_1.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                MyArrow_2.transform.localScale = new Vector3(0.320978f, 0.5368357f, 0.320978f);
                MyArrow_3.transform.localScale = new Vector3(0.320978f, 0.5368357f, 0.320978f);

                //MyArrow_2.GetComponent<MeshRenderer>().enabled = false;// Should happen at the beginning
                //MyArrow_3.GetComponent<MeshRenderer>().enabled = false;//




                break;

            case 2:
                //Instruction_2.GetComponent<MeshRenderer>().enabled = false;
                //Instruction_1.GetComponent<MeshRenderer>().enabled = false;
                Instruction_1.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                Instruction_2.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                DoneMessage.transform.localScale = new Vector3(1f, 1f, 1f);
                //DoneMessage.GetComponent<MeshRenderer>().enabled = true;
                MyArrow_2.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                MyArrow_3.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                FirstMessage.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);

                break;
            /*case 3:
                break;*/
            default:

                Instruction_1.transform.localScale = new Vector3(1f, 1f, 1f);
                Instruction_2.transform.localScale = new Vector3(0.0000001f, 0.00000001f, 0.0000001f);
                DoneMessage.transform.localScale = new Vector3(0.0000001f, 0.0000001f, 0.0000001f);
                MyArrow_1.transform.localScale = new Vector3(0.320978f, 0.5368357f, 0.320978f);
                FirstMessage.transform.localScale = new Vector3(0.9985631f, 0.9990476f,0.9985631f);

                //DoneMessage.GetComponent<MeshRenderer>().enabled = false;
                //Instruction_2.GetComponent<MeshRenderer>().enabled = false;
                //Instruction_1.GetComponent<MeshRenderer>().enabled = true;
                //MyArrow_1.transform.localScale = new Vector3(0.320978f, 0.5368357f, 0.320978f);



                counter = 0;
               
                //FirstMessage.SetActive(false);
                break;
        }

        
    }
}
