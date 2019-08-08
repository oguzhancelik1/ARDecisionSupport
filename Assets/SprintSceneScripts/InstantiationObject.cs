using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
//using System.Numerics; ****cant use it because it causes problem when it is used with UnityEngine

public class InstantiationObject : MonoBehaviour
{
    public GameObject ExistingPrefab;
    public int step = 0;
    public float PositionShift;
    public GameObject ImageTarget;
    public static IList Instances = new ArrayList();
    public int instance_id;
    
    public Camera cam;
    public GameObject prefabInstance;
    public Transform target;
    
    public Vector3 temp;

    public static int x;
    public string instance_id_String;
    


    public void OnClick()
    {
        try
        {
            // Make the function callable every time the button is pressed without a limitation on how many times you can press the button
            step++;
            switch (step)
            {
                default:
                    prefabInstance = Instantiate(ExistingPrefab);
                    prefabInstance.transform.parent = ImageTarget.transform;
                    prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                    prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    PositionShift = PositionShift + 1f;
                    //name the instance with its id number 
                    prefabInstance.name = prefabInstance.GetInstanceID().ToString();
                    //get the id of each prefab instance
                    instance_id = prefabInstance.GetInstanceID();
                    instance_id_String = instance_id.ToString();
                    temp = (GameObject.Find(instance_id_String)).transform.localPosition;// MIGHT NEED TO DRAG IT UPWARDS
                    //Debug.Log("prefab instance is"+instance_id);
                    // add ids to the Instances array
                    //Instances.Insert(Instances_Array_index, instance_id);
                    // set the layer 8, for each instance so that they are collidible
                    prefabInstance.layer = 9;                       
                    break;
            }
            //Instances_Array_index++;
            x = Instances.Count;
        }
        catch (Exception ex) { }
       // saveStuff.FindGameObject(instance_id_String);// why do i need an instance of a class when gamecontroller doesnt need it to implement savedata class functions?
    }
    public string InstanceIdGetter()
    {
        string ad = instance_id_String;

        return ad;
    }
    public Vector3 SetPosition()
    {
        Vector3 temposition;
        temposition = temp;
        

        return temposition;
    }
    public Transform SetTransform()
    {
        Transform tr = ImageTarget.transform;
        return tr;
        
    }
    /*
    public static int GetCount(IList arrayList )
    {
        int num = arrayList.Count;
        return num;
    }*/
    
    
       
   

}

