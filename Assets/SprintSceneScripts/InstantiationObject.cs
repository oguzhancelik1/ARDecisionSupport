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

 
    public string string_that_holds_sphere_info;
    
    

    // Declaration of the generative objects
    public GameObject generativeCube;
    public GameObject generativeSphere;
    public GameObject generativeArrow;

    //To be assigned to each object created
    public static int distinctiveNumber = 1;
    //By default the application will be in step 1

    //Position shift for new objects
    private float instantiationPositionShift = 0;

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
    public /*Dictionary<string, float>*/void FillDictionary(Dictionary<string, float> dict, string key, float info)//,float posy,float posz,float rotx,float roty,float rotz,float sclx,float scly,float sclz )
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
        float currentStepNumber;
        DistinctiveSphereData distinctiveSphereData = new DistinctiveSphereData();
        currentStepNumber = PlayerPrefs.GetFloat("Step");
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
                        //check previously incremented distinctive numbers to see what is the last distinctive number in the key if any exists
                        if (PlayerPrefs.HasKey("DistinctiveNumber"))
                        {
                            
                           
                            //Get the last distinctive number(it also represents the number of spehere objects created prior createing new sphere object)
                            distinctiveNumber = PlayerPrefs.GetInt("DistinctiveNumber");
                            //continue with the next distinct number when the application is launched multiple times
                            distinctiveNumber++;
                            //instantiate prefab instance 
                            prefabInstance = Instantiate(ExistingPrefabSphere);
                            //set the image target as parent of prefab instance
                            prefabInstance.transform.parent = ImageTarget.transform;
                            //make every new instance further from the previously created prefab instance
                            prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                            prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            PositionShift = PositionShift + instantiationPositionShift;
                            //Assign the objects distinctive id using distinctive number 
                            distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                            distinctiveSphereData.id = distinctiveNumber;

                            
                            //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                            prefabInstance.layer = 9;

                      
                            //Set the key for the SetString function by stringifying the distinctive number
                            string distinctDictKeyName = distinctiveNumber.ToString();
                            //Create a dicctionary object to fill in the data
                            Dictionary<string, float> dict = new Dictionary<string, float>();
                            //Fill the position data to the dictionary
                            FillDictionary(dict, "PosX", prefabInstance.transform.localPosition.x);
                            FillDictionary(dict, "PosY", prefabInstance.transform.localPosition.y);
                            FillDictionary(dict, "PosZ", prefabInstance.transform.localPosition.z);
                            //Fill the step value of the object with the step it is created
                            FillDictionary(dict,"StepValue", currentStepNumber);
                            //Convert dictionary to string then assign to a variable
                            string_that_holds_sphere_info = ConvertDictToString(dict, string_that_holds_sphere_info);
                            Debug.Log(string_that_holds_sphere_info);


                            //Save the stringified dictionary
                            PlayerPrefs.SetString(distinctDictKeyName, string_that_holds_sphere_info);
                            //Save the number of sphere objects that were created
                            PlayerPrefs.SetInt("DistinctiveNumber", distinctiveNumber);

                            
                            Debug.Log("Instantiation number of instances :" + distinctiveNumber);

                        }
                        //If there is no DistinctiveNumber key, create the first sphere object and assign its tag distinctiveNumber which should be 1 at this moment
                        else
                        {
                            //instantiate prefab instance 
                            prefabInstance = Instantiate(ExistingPrefabSphere);
                            //set the image target as parent of prefab instance
                            prefabInstance.transform.parent = ImageTarget.transform;
                            //make every new instance further from the previously created prefab instance
                            prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                            prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            PositionShift = PositionShift + 1f;
                            //Assign the objects distinctive id using distinctive number
                            distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                            distinctiveSphereData.id = distinctiveNumber;
                            
                            //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                            prefabInstance.layer = 9;

                            //Assign instance tag the distinctive number
                            //prefabInstance.tag = distinctiveNumber.ToString();
                            //Set the key for the SetString function by stringifying the distinctive number
                            string distinctDictKeyName = distinctiveNumber.ToString();
                            //Create a dicctionary object to fill in the data
                            Dictionary<string, float> dict = new Dictionary<string, float>();
                            //Fill the position data to the dictionary
                            FillDictionary(dict, "PosX", prefabInstance.transform.localPosition.x);
                            FillDictionary(dict, "PosY", prefabInstance.transform.localPosition.y);
                            FillDictionary(dict, "PosZ", prefabInstance.transform.localPosition.z);
                            //Fill the step value of the object with the step it is created
                            FillDictionary(dict, "StepValue", currentStepNumber);
                            //Convert dictionary to string then assign to a variable
                            string_that_holds_sphere_info = ConvertDictToString(dict, string_that_holds_sphere_info);
                            Debug.Log(string_that_holds_sphere_info);


                            //Save the stringified dictionary
                            PlayerPrefs.SetString(distinctDictKeyName, string_that_holds_sphere_info);
                            //Save the number of sphere objects that were created
                            PlayerPrefs.SetInt("DistinctiveNumber", distinctiveNumber);


                            Debug.Log("Instantiation number of instances :" + distinctiveNumber);

                            
                        }
                        
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

