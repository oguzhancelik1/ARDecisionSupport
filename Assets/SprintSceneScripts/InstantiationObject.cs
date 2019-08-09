using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;


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
                    //instantiate prefab instance 
                    prefabInstance = Instantiate(ExistingPrefab);
                    //set the image target as parent of prefab instance
                    prefabInstance.transform.parent = ImageTarget.transform;
                    //make every new instance further from the previously created prefab instance
                    prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                    prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    PositionShift = PositionShift + 1f;
                    //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                    prefabInstance.layer = 9;
                    /*
                    //name the instance with its id number 
                    prefabInstance.name = prefabInstance.GetInstanceID().ToString();
                    
                    //get the id of each prefab instance
                    instance_id = prefabInstance.GetInstanceID();
                    instance_id_String = instance_id.ToString();

                    //Saving the transform local position
                    SavedTransformPosition = (GameObject.Find(instance_id_String)).transform.localPosition;
                    */
                    //Save the new ID

                    //Save the new transform position
                    /*
                    PlayerPrefs.SetFloat("TransformPosX", SavedTransformPosition.x);
                    PlayerPrefs.SetFloat("TransformPosY", SavedTransformPosition.y);
                    PlayerPrefs.SetFloat("TransformPosZ", SavedTransformPosition.z);
                    */
                    break;
            }
            
            x = Instances.Count;
        }
        catch (Exception ex) { }
       
    }
  
   
}

