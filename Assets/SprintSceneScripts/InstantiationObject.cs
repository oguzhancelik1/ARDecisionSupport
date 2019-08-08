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
    
    public Vector3 SavedTransformPosition;

    public static int x;
    public string instance_id_String;
    


    public void CreateNewObjectButton()
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
                    prefabInstance.layer = 9;
                    /*
                    //name the instance with its id number 
                    prefabInstance.name = prefabInstance.GetInstanceID().ToString();
                    
                    //get the id of each prefab instance
                    instance_id = prefabInstance.GetInstanceID();
                    instance_id_String = instance_id.ToString();

                    //Saving the transform local position
                    SavedTransformPosition = (GameObject.Find(instance_id_String)).transform.localPosition;

                    //Save the new ID

                    //Save the new transform position
                    PlayerPrefs.SetFloat("TransformPosX", SavedTransformPosition.x);
                    PlayerPrefs.SetFloat("TransformPosY", SavedTransformPosition.y);
                    PlayerPrefs.SetFloat("TransformPosZ", SavedTransformPosition.z);
                    */
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
        temposition = SavedTransformPosition;
        

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

