using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Text;
using System.IO;


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

    //Declaration of dictionary and the string that will hold the sphere object's information
    public Dictionary<string, float> sphereObjectsDictionary = new Dictionary<string, float>();
    public string string_that_holds_sphere_info;
    
    

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
    public /*Dictionary<string, float>*/void FillSphereDictionary(Dictionary<string, float> dict,string key,float info)//,float posy,float posz,float rotx,float roty,float rotz,float sclx,float scly,float sclz )
    {
        dict.Add(key, info);

        
    }



    /// <summary>
    /// ConvertDictToString converts a given string, float dictionary to a string using the GetLine method.
 
    public string ConvertDictToString(Dictionary<string, float> dict,string objectTypeString)
    {
        // Convert dictionary to string and save
        objectTypeString = GetLine(dict);
        //File.WriteAllText("dict.txt", s);
        // Get dictionary from that file
        //Dictionary<string, int> d = GetDict("dict.txt");
        return objectTypeString;
    }

    string GetLine(Dictionary<string, float> d)
    {
        // Build up each line one-by-one and then trim the end
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<string, float> pair in d)
        {
            builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
        }
        string result = builder.ToString();
        // Remove the final delimiter
        result = result.TrimEnd(',');
        return result;
    }
    /// <summary>
  





    //Takes a string returns a dictionary of type <string, float>
    public Dictionary<string, float> ConvertStringToDict(string f)
    {
        Dictionary<string, float> d = new Dictionary<string, float>();
        string s = f; //= File.ReadAllText(f);
        // Divide all pairs (remove empty strings)
        string[] tokens = s.Split(new char[] { ':', ',' });//, StringSplitOptions.RemoveEmptyEntries);

        // Walk through each item
        for (int i = 0; i < tokens.Length; i += 2)
        {
            string name = tokens[i];
            string freq = tokens[i + 1];

            // Parse the int (this can throw)
            int count = int.Parse(freq);
            // Fill the value in the sorted dictionary
            if (d.ContainsKey(name))
            {
                d[name] += count;
            }
            else
            {
                d.Add(name, count);
            }
        }
        return d;
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

                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosX", prefabInstance.transform.localPosition.x);
                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosY", prefabInstance.transform.localPosition.y);
                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosZ", prefabInstance.transform.localPosition.z);
                        string_that_holds_sphere_info = ConvertDictToString(sphereObjectsDictionary, string_that_holds_sphere_info);
                        Debug.Log(string_that_holds_sphere_info);
                        PlayerPrefs.SetString("Sphere", string_that_holds_sphere_info);



                        /*
                        
                        //Add the local position information into the sphereObject dictionary
                        sphereObject.Add(prefabInstance.name + "PosX", prefabInstance.transform.localPosition.x);
                        sphereObject.Add(prefabInstance.name + "PosY", prefabInstance.transform.localPosition.y);
                        sphereObject.Add(prefabInstance.name + "PosZ", prefabInstance.transform.localPosition.z);
                        //Add the local rotaion information into the sphereObject dictionary
                        sphereObject.Add(prefabInstance.name + "RotX", prefabInstance.transform.localRotation.x);
                        sphereObject.Add(prefabInstance.name + "RotY", prefabInstance.transform.localRotation.y);
                        sphereObject.Add(prefabInstance.name + "RotZ", prefabInstance.transform.localRotation.z);
                        //Add the local scale information into the sphereObject dictionary
                        sphereObject.Add(prefabInstance.name + "SclX", prefabInstance.transform.localScale.x);
                        sphereObject.Add(prefabInstance.name + "SclY", prefabInstance.transform.localScale.y);
                        sphereObject.Add(prefabInstance.name + "SclZ", prefabInstance.transform.localScale.z);
                        */




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

