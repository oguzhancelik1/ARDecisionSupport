using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;


public class InstantiationObject : MonoBehaviour
{   //Declaration of prefabs to be used
    public GameObject ExistingPrefabCube;
    public GameObject ExistingPrefabSphere;
    public GameObject ExistingPrefabArrow;

    public int step = 0;

    public float PositionShift;
    public GameObject ImageTarget;
    public static IList Instances = new ArrayList();
    public int instance_id;
    
    //camera to be used for the raycasting
    public Camera cam;
    //Declaratıon of instance to be created
    public GameObject prefabInstance;
    public Transform target;
    
    public Vector3 SavedTransformPosition;

    public static int x;
    public string instance_id_String;

    //The string that will hold the transform information of all the added cube objects
    public string string_holds_cube_objects;
    //The string that will hold the transform information of all the added sphere objects
    public string string_holds_sphere_objects;
    //The string that will hold the transform information of all the added arrow objects
    public string string_holds_arrow_objects;
    //Declaratıon of the generatıve objects whıch will allow user to create desired object
    public char instance_name;

    // Declaration of the generative objects
    public GameObject generativeCube;
    public GameObject generativeSphere;
    public GameObject generativeArrow;
    
    //Function to swipe right through to generative objects
    public void SwipeRight()
    {
        if (generativeCube.activeSelf)
        {
            generativeCube.SetActive(false);
            generativeSphere.SetActive(false);
            generativeArrow.SetActive(true);
        }
        else if (generativeArrow.activeSelf)
        {
            generativeArrow.SetActive(false);
            generativeCube.SetActive(false);
            generativeSphere.SetActive(true);

        }
        else if (generativeSphere.activeSelf)
        {
            generativeArrow.SetActive(false);
            generativeCube.SetActive(true);
            generativeSphere.SetActive(false);

        }
        

    }
    //function to swipe left through the generative objects
    public void SwipeLeft()
    {
        if (generativeCube.activeSelf)
        {
            generativeCube.SetActive(false);
            generativeSphere.SetActive(true);
            generativeArrow.SetActive(false);
        }
        else if (generativeArrow.activeSelf)
        {
            generativeArrow.SetActive(false);
            generativeCube.SetActive(true);
            generativeSphere.SetActive(false);

        }
        else if (generativeSphere.activeSelf)
        {
            generativeArrow.SetActive(true);
            generativeCube.SetActive(false);
            generativeSphere.SetActive(false);

        }


    }

    //Function to create object on click event
    public void CreateNewObjectButton()
    {

        
        try
        {
            // Make the function callable every time the button is pressed without a limitation on how many times you can press the button
            step++;
            switch (step)
            {
                default:
                    if(generativeCube.activeSelf)
                    {
                        //instantiate prefab instance 
                        prefabInstance = Instantiate(ExistingPrefabCube);
                        //set the image target as parent of prefab instance
                        prefabInstance.transform.parent = ImageTarget.transform;
                        //make every new instance further from the previously created prefab instance
                        prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                        prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        PositionShift = PositionShift + 1f;
                        //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                        prefabInstance.layer = 9;
                        



                        
                        //name the instance with its id number 
                        prefabInstance.name = prefabInstance.GetInstanceID().ToString();

                        //get the id of each prefab instance
                        //instance_id = prefabInstance.GetInstanceID();
                        //instance_id_String = instance_id.ToString();

                        //Saving the transform local position
                        //SavedTransformPosition = (GameObject.Find(instance_id_String)).transform.localPosition;
                        
                        //Save the new ID

                        //Save the new transform position
                        /*
                        PlayerPrefs.SetFloat("TransformPosX", SavedTransformPosition.x);
                        PlayerPrefs.SetFloat("TransformPosY", SavedTransformPosition.y);
                        PlayerPrefs.SetFloat("TransformPosZ", SavedTransformPosition.z);
                        */
                    }
                    else if(generativeSphere.activeSelf)
                    {

                        //instantiate prefab instance 
                        prefabInstance = Instantiate(ExistingPrefabSphere);
                        //set the image target as parent of prefab instance
                        prefabInstance.transform.parent = ImageTarget.transform;
                        //make every new instance further from the previously created prefab instance
                        prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                        prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        PositionShift = PositionShift + 1f;
                        //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                        prefabInstance.layer = 9;
                        
                        //name the instance with its id number 
                        prefabInstance.name = prefabInstance.GetInstanceID().ToString();

                        //get the id of each prefab instance
                        //instance_id = prefabInstance.GetInstanceID();
                        //instance_id_String = instance_id.ToString();

                        //Saving the transform local position
                        //SavedTransformPosition = (GameObject.Find(instance_id_String)).transform.localPosition;
                        
                        //Save the new ID

                        //Save the new transform position
                        /*
                        PlayerPrefs.SetFloat("TransformPosX", SavedTransformPosition.x);
                        PlayerPrefs.SetFloat("TransformPosY", SavedTransformPosition.y);
                        PlayerPrefs.SetFloat("TransformPosZ", SavedTransformPosition.z);
                        */

                    }
                    else if (generativeArrow.activeSelf)
                    {
                        //instantiate prefab instance 
                        prefabInstance = Instantiate(ExistingPrefabArrow);
                        //set the image target as parent of prefab instance
                        prefabInstance.transform.parent = ImageTarget.transform;
                        //make every new instance further from the previously created prefab instance
                        prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                        prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        PositionShift = PositionShift + 1f;
                        //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                        prefabInstance.layer = 9;
                        
                        //name the instance with its id number 
                        prefabInstance.name = prefabInstance.GetInstanceID().ToString();

                        //get the id of each prefab instance
                        //instance_id = prefabInstance.GetInstanceID();
                        //instance_id_String = instance_id.ToString();

                        //Saving the transform local position
                        //SavedTransformPosition = (GameObject.Find(instance_id_String)).transform.localPosition;
                        
                        //Save the new ID

                        //Save the new transform position
                        /*
                        PlayerPrefs.SetFloat("TransformPosX", SavedTransformPosition.x);
                        PlayerPrefs.SetFloat("TransformPosY", SavedTransformPosition.y);
                        PlayerPrefs.SetFloat("TransformPosZ", SavedTransformPosition.z);
                        */

                    }

                    
                    break;
            }
            
            x = Instances.Count;
        }
        catch (Exception ex) { }
       
    }
  
   
}

