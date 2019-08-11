
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class SelectObject : InstantiationObject
{
   
    public int counter;
    
    int layerMask = 1 << 9;
    GameObject temporary;
    public int countStep = 0;
    
    public static string hit_instance_name;
    public int hit_instance_id;

    //holds whichever object is added from the generative objects
    public GameObject currentObject;
    

    void Start()
    {
        //Not sure declaration of the type strings should be done here!!!
        PlayerPrefs.SetString("cube", string_holds_cube_objects);
        PlayerPrefs.SetString("sphere", string_holds_sphere_objects);
        PlayerPrefs.SetString("arrow", string_holds_arrow_objects);
    }

    //move the last selected object +x direction with the click on button Move_Object_on_X_Positive
    public void Move_Object_on_X_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on +x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x + 0.1f, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);
        //save the new x location
        PlayerPrefs.SetFloat("TransformPosX", currentObject.transform.localPosition.x);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -x direction with the click on button Move_Object_on_X_Negative
    public void Move_Object_on_X_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on -x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x - 0.1f, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);
        //save the new x location
        PlayerPrefs.SetFloat("TransformPosX", currentObject.transform.localPosition.x);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +y direction with the click on button Move_Object_on_Y_Positive
    public void Move_Object_on_Y_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on +y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y + 0.1f, currentObject.transform.localPosition.z);
        //save the new y location
        PlayerPrefs.SetFloat("TransformPosY", currentObject.transform.localPosition.y);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -y direction with the click on button Move_Object_on_Y_Negative
    public void Move_Object_on_Y_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on -y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x , currentObject.transform.localPosition.y - 0.1f, currentObject.transform.localPosition.z);
        //save the new y location
        PlayerPrefs.SetFloat("TransformPosY", currentObject.transform.localPosition.y);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +z direction with the click on button Move_Object_on_Z_Positive
    public void Move_Object_on_Z_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on +z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z + 0.1f);
        //save the new z location
        PlayerPrefs.SetFloat("TransformPosZ", currentObject.transform.localPosition.z);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -z direction with the click on button Move_Object_on_Z_Negative
    public void Move_Object_on_Z_Negative()
    {

        //get the last touched object
        currentObject = GameObject.Find(hit_instance_name);
        //change the location on -z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z - 0.1f);
        //save the new z location
        PlayerPrefs.SetFloat("TransformPosZ", currentObject.transform.localPosition.z);
        Debug.Log("New position data stored.");
    }
    

    //set color to default for the object deselected
    public void ObjectDeselected()
    {
        
    }

    //return the last touched object
    public void ObjectSelected()
    {
        
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
                //get the collided objects name
                hit_instance_name = hit.transform.name;
                Debug.Log("The object that was hit is : "+ hit_instance_name);
                
            }
        }
    }
}


