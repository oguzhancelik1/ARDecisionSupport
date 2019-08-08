
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class SelectObject : InstantiationObject
{
   // public GameObject Button_move;
    public int counter;
    private int id_to_move;
    int layerMask = 1 << 9;
    GameObject temporary;
    public int countStep = 0;
    public int Instances_Array_index = 0;
    public static string hit_instance_id;

    void Start()
    {
     
            
    }

    public void GetObjectToMove()
    {
        /* int id_to_move = 0;
         if (!Int32.TryParse(hit_instance_id, out id_to_move))
         {
             id_to_move = -1;
         }
         return id_to_move;*/
         
       
    }
    //set color to default for the object deselected
    public void ObjectDeselected()
    {



    }

    //change color of the object selected
    public void ObjectSelected()
    {
        temporary = GameObject.Find(hit_instance_id);
        Renderer rend = temporary.GetComponentInChildren<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.green);
    }

   
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            //create ray and RaycastHit objects
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //check if the ray collides with the objects 
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                //get the collided objects name(its instance id)
                hit_instance_id = hit.transform.name;

                Debug.Log("hit_instance_id is : " + hit_instance_id);


                /*ObjectSelected();
                Button_move.SetActive(true);
                GetObjectToMove();*/



                /*int num = InstantiationObject.GetCount(Instances);
                for (counter = 0; counter < num; counter++)
                {
                    Debug.Log("elements in Instances are : " + Instances[counter]);
                }*/

               /* switch(countStep)
                    {
                    default:
                Instances.Insert(Instances_Array_index, hit_instance_id);
                        break;
                    }
                //rotate the object with every click
                temp = GameObject.Find(hit_instance_id);
                temp.transform.Rotate(0, 4, 3);


                /*if (Instances.Contains(hit_instance_id))  
                {
                    Debug.Log("Object hit was : "+hit_instance_id);
                    temp = GameObject.Find(hit_instance_id.ToString());
                    temp.transform.Rotate(0, 4, 3);
                }*/
            }
            else
            {
                //No object was selected, disable Button_move
                //Button_move.SetActive(false);
            }
                

            
        }
    }
}