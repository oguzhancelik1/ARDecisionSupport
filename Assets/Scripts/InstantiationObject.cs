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
    //public GameObject ExistingPrefabCube;
    //public GameObject ExistingPrefabSphere;
    //public GameObject ExistingPrefabArrow;

    public GameObject[] ARObjectsArray;

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

 
    public string StringOfObjectData;
    
    

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
            generativeArrow.SetActive(true);
            generativeCube.SetActive(false);
            generativeSphere.SetActive(false);
        }
        else if (generativeArrow.activeSelf)
        {
            generativeSphere.SetActive(true);
            generativeCube.SetActive(false);
            generativeArrow.SetActive(false);
        }
        else if (generativeSphere.activeSelf)
        {
            generativeCube.SetActive(true);
            generativeArrow.SetActive(false);
            generativeSphere.SetActive(false);
        }
    }
    //function to swipe left through the generative objects
    public void SwipeLeft()
    {
        if (generativeCube.activeSelf)
        {
            generativeSphere.SetActive(true);
            generativeArrow.SetActive(false);
            generativeCube.SetActive(false);
        }
        else if (generativeArrow.activeSelf)
        {
            generativeCube.SetActive(true);
            generativeArrow.SetActive(false);
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
        currentStepNumber = PlayerPrefs.GetFloat("Step");

        //The index of the object to be created, 
        //0: Sphere
        //1: Arrow
        //2: Cube
        int IndexOfObjectToBeCreated = 0;
        if (generativeSphere.activeSelf)
        {
            IndexOfObjectToBeCreated = 0;
        }
        else if (generativeArrow.activeSelf)
        {
            IndexOfObjectToBeCreated = 1;
        }
        else if (generativeCube.activeSelf)
        {
            IndexOfObjectToBeCreated = 2;
        }

        //check previously incremented distinctive numbers to see what is the last distinctive number in the key if any exists
        if (PlayerPrefs.HasKey("DistinctiveNumber"))
        {
            //Get the last distinctive number(it also represents the number of spehere objects created prior createing new sphere object)
            distinctiveNumber = PlayerPrefs.GetInt("DistinctiveNumber");
            //continue with the next distinct number when the application is launched multiple times
            distinctiveNumber++;
            //instantiate prefab instance 
            prefabInstance = Instantiate(ARObjectsArray[IndexOfObjectToBeCreated]);
            //set the image target as parent of prefab instance
            prefabInstance.transform.parent = ImageTarget.transform;
            //make every new instance further from the previously created prefab instance
            prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
            prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
            PositionShift = PositionShift + instantiationPositionShift;
            //Assign the objects distinctive id using distinctive number 
            DistinctiveObjectData distinctiveObjectData = prefabInstance.GetComponent<DistinctiveObjectData>();
            distinctiveObjectData.id = distinctiveNumber;
            prefabInstance.name = distinctiveObjectData.id.ToString();
            distinctiveObjectData.type = IndexOfObjectToBeCreated;

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
            FillDictionary(dict, "Type", distinctiveObjectData.type);
            //Fill the step value of the object with the step it is created
            FillDictionary(dict, "StepValue", currentStepNumber);
            //Convert dictionary to string then assign to a variable
            StringOfObjectData = ConvertDictToString(dict, StringOfObjectData);
            Debug.Log(StringOfObjectData);


            //Save the stringified dictionary
            PlayerPrefs.SetString(distinctDictKeyName, StringOfObjectData);
            //Save the number of sphere objects that were created
            PlayerPrefs.SetInt("DistinctiveNumber", distinctiveNumber);


            Debug.Log("Instantiation number of instances :" + distinctiveNumber);

        }
        //If there is no DistinctiveNumber key, create the first sphere object and assign its tag distinctiveNumber which should be 1 at this moment
        else
        {
            //instantiate prefab instance 
            prefabInstance = Instantiate(ARObjectsArray[IndexOfObjectToBeCreated]);
            //set the image target as parent of prefab instance
            prefabInstance.transform.parent = ImageTarget.transform;
            //make every new instance further from the previously created prefab instance
            prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
            prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
            PositionShift = PositionShift + 1f;
            //Assign the objects distinctive id using distinctive number
            DistinctiveObjectData distinctiveObjectData = prefabInstance.GetComponent<DistinctiveObjectData>();
            distinctiveObjectData.id = distinctiveNumber;
            distinctiveObjectData.type = IndexOfObjectToBeCreated;
            prefabInstance.name = distinctiveObjectData.id.ToString();

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
            FillDictionary(dict, "Type", distinctiveObjectData.type);
            //Fill the step value of the object with the step it is created
            FillDictionary(dict, "StepValue", currentStepNumber);
            //Convert dictionary to string then assign to a variable
            StringOfObjectData = ConvertDictToString(dict, StringOfObjectData);
            Debug.Log(StringOfObjectData);


            //Save the stringified dictionary
            PlayerPrefs.SetString(distinctDictKeyName, StringOfObjectData);
            //Save the number of sphere objects that were created
            PlayerPrefs.SetInt("DistinctiveNumber", distinctiveNumber);


            Debug.Log("Instantiation number of instances :" + distinctiveNumber);
        }

#if false
        if (generativeCube.activeSelf)
        {
            //instantiate prefab instance 
            prefabInstance = Instantiate(ARObjectsArray[2]);
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
                prefabInstance = Instantiate(ARObjectsArray[0]);
                //set the image target as parent of prefab instance
                prefabInstance.transform.parent = ImageTarget.transform;
                //make every new instance further from the previously created prefab instance
                prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                PositionShift = PositionShift + instantiationPositionShift;
                //Assign the objects distinctive id using distinctive number 
                distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                distinctiveSphereData.id = distinctiveNumber;
                prefabInstance.name = distinctiveSphereData.id.ToString();


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
                FillDictionary(dict, "Type", distinctiveSphereData.type);
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
                prefabInstance = Instantiate(ARObjectsArray[0]);
                //set the image target as parent of prefab instance
                prefabInstance.transform.parent = ImageTarget.transform;
                //make every new instance further from the previously created prefab instance
                prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                PositionShift = PositionShift + 1f;
                //Assign the objects distinctive id using distinctive number
                distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                distinctiveSphereData.id = distinctiveNumber;
                distinctiveSphereData.type = 1;
                prefabInstance.name = distinctiveSphereData.id.ToString();

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
                FillDictionary(dict, "Type", distinctiveSphereData.type);
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
            //check previously incremented distinctive numbers to see what is the last distinctive number in the key if any exists
            if (PlayerPrefs.HasKey("DistinctiveNumber"))
            {


                //Get the last distinctive number(it also represents the number of spehere objects created prior createing new sphere object)
                distinctiveNumber = PlayerPrefs.GetInt("DistinctiveNumber");
                //continue with the next distinct number when the application is launched multiple times
                distinctiveNumber++;
                //instantiate prefab instance 
                prefabInstance = Instantiate(ARObjectsArray[1]);
                //set the image target as parent of prefab instance
                prefabInstance.transform.parent = ImageTarget.transform;
                //make every new instance further from the previously created prefab instance
                prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                PositionShift = PositionShift + instantiationPositionShift;
                //Assign the objects distinctive id using distinctive number 
                distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                distinctiveSphereData.id = distinctiveNumber;
                distinctiveSphereData.type = 2;
                prefabInstance.name = distinctiveSphereData.id.ToString();


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
                FillDictionary(dict, "Type", distinctiveSphereData.type);

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
            //If there is no DistinctiveNumber key, create the first sphere object and assign its tag distinctiveNumber which should be 1 at this moment
            else
            {
                //instantiate prefab instance 
                prefabInstance = Instantiate(ARObjectsArray[1]);
                //set the image target as parent of prefab instance
                prefabInstance.transform.parent = ImageTarget.transform;
                //make every new instance further from the previously created prefab instance
                prefabInstance.transform.localPosition = new Vector3(0f + PositionShift, 0f, 0f);
                prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                PositionShift = PositionShift + 1f;
                //Assign the objects distinctive id using distinctive number
                distinctiveSphereData = prefabInstance.GetComponent<DistinctiveSphereData>();
                distinctiveSphereData.type = 2;
                distinctiveSphereData.id = distinctiveNumber;
                prefabInstance.name = distinctiveSphereData.id.ToString();

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
                FillDictionary(dict, "Type", distinctiveSphereData.type);

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
#endif     
    }
  
   
}

