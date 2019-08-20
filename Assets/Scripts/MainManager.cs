using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    #region Public Variables
    public GameObject[] ARObjectsArray;
    //Declaration of the generative objects
    public GameObject generativeCube;
    public GameObject generativeSphere;
    public GameObject generativeArrow;

    //To be assigned to each object created
    public static int distinctiveNumber = 1;

    //Declaratıon of instance to be created
    public GameObject prefabInstance;

    public GameObject ImageTarget;

    public float PositionShift;

    public string StringOfObjectData;

    //holds whichever object is added from the generative objects
    public GameObject currentObject;

    public string MyTag;

    public Text StepUIText;

    //camera to be used for the raycasting
    public Camera cam;

    public static string hit_instance_name;
    #endregion

    #region Private Variables
    //Position shift for new objects
    private float instantiationPositionShift = 0;

    //Layer that holds the created objects which will collide with the raycast
    int layerMask = 1 << 9 | 1 << 5;

    //Step for Moving on X, Y and Z axis
    private readonly float MovingStep = 0.1f;
    #endregion

    #region Dictionary functions
    public /*Dictionary<string, float>*/void FillDictionary(Dictionary<string, float> dict, string key, float info)//,float posy,float posz,float rotx,float roty,float rotz,float sclx,float scly,float sclz )
    {
        dict.Add(key, info);
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
    /// ConvertDictToString converts a given string, float dictionary to a string using the GetLine method.
    public string ConvertDictToString(Dictionary<string, float> dict, string objectTypeString)
    {
        // Convert dictionary to string and save
        objectTypeString = GetLine(dict);
        //File.WriteAllText("dict.txt", s);
        // Get dictionary from that file
        //Dictionary<string, int> d = GetDict("dict.txt");
        return objectTypeString;
    }

    //Takes a string returns a dictionary of type <string, float>
    public Dictionary<string, float> ConvertStringToDict(string f)
    {
        Debug.Log("String to be converted to Dict is: " + f);
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
            float count = float.Parse(freq);
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
    #endregion

    #region Select Object Buttons
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
            PositionShift = PositionShift + MovingStep;
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
    }
    #endregion

    #region Moving on X,Y,Z axis buttons
    //move the last selected object +x direction with the click on button Move_Object_on_X_Positive
    public void Move_Object_on_X_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string a = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(a);

        //change the location on +x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x + MovingStep, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosX"] = currentObject.transform.localPosition.x;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        a = ConvertDictToString(dict, a);
        Debug.Log(a);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, a);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -x direction with the click on button Move_Object_on_X_Negative
    public void Move_Object_on_X_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string b = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(b);

        //change the location on -x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x - MovingStep, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosX"] = currentObject.transform.localPosition.x;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        b = ConvertDictToString(dict, b);
        Debug.Log(b);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, b);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +y direction with the click on button Move_Object_on_Y_Positive
    public void Move_Object_on_Y_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string c = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(c);

        //change the location on +y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y + MovingStep, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosY"] = currentObject.transform.localPosition.y;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        c = ConvertDictToString(dict, c);
        Debug.Log(c);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, c);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -y direction with the click on button Move_Object_on_Y_Negative
    public void Move_Object_on_Y_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string d = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(d);

        //change the location on -y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y - MovingStep, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosY"] = currentObject.transform.localPosition.y;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        d = ConvertDictToString(dict, d);
        Debug.Log(d);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, d);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +z direction with the click on button Move_Object_on_Z_Positive
    public void Move_Object_on_Z_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string e = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(e);

        //change the location on -z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z + MovingStep);

        //Update the dictionary
        dict["PosZ"] = currentObject.transform.localPosition.z;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        e = ConvertDictToString(dict, e);
        Debug.Log(e);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, e);
        Debug.Log("New position data stored.");
    }



    //move the last selected object -z direction with the click on button Move_Object_on_Z_Negative
    public void Move_Object_on_Z_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(MyTag);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string f = PlayerPrefs.GetString(MyTag);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(f);

        //change the location on -z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z - MovingStep);

        //Update the dictionary
        dict["PosZ"] = currentObject.transform.localPosition.z;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        f = ConvertDictToString(dict, f);
        Debug.Log(f);
        //save the new string for the object
        PlayerPrefs.SetString(MyTag, f);
        Debug.Log("New position data stored.");
    }
    #endregion

    #region Next Step and Previous Step Buttons
    //Function to be called when the next button is clicked to change the step
    public void OnClickNext()
    {   //restore distinctive number and the current step
        int numberofObjects = PlayerPrefs.GetInt("DistinctiveNumber");
        float currentStep = PlayerPrefs.GetFloat("Step");
        //Increment the current step 
        currentStep++;
        //Update the step text
        StepUIText.text = "Step " + currentStep.ToString();
        //
        PlayerPrefs.SetFloat("Step", currentStep);
        //variable to hold string version of the counter in the for loop
        string counterString;
        //variable to hold the restored object string data
        string keyStringHolder;
        //Variables to hold the gameobject's renderer and collider components which will be enabled or disabled depending on the current step value
        Collider collider;
        Renderer renderer;
        //Dictionary variable is going to be used to compare objects Step value(the step it was created) to the current step value
        Dictionary<string, float> dict = new Dictionary<string, float>();
        //Gameobject variable to access the gameobjects
        GameObject go;

        //for loop will iterate for all objectsstarting from the first until it finishes with the last object created.
        for (int counter = 1; counter <= numberofObjects; counter++)
        {
            if (PlayerPrefs.HasKey(counter.ToString()))
            {

                //Convert counter to string
                counterString = counter.ToString();
                //Get the corresponding objects data
                keyStringHolder = PlayerPrefs.GetString(counterString);
                //Convert the data to dictionary type to access its step value
                dict = ConvertStringToDict(keyStringHolder);
                //Assign 1 to the arrowType variable
                int arrowType = 1;
                //If the object type is arrow
                if (dict["Type"] == arrowType)
                {
                    //Compare current step and the object's step value
                    if (dict["StepValue"] == currentStep)
                    {
                        //Find the arrow instance
                        go = GameObject.Find(counterString);
                        //Get the children of the arrow instance
                        GameObject childArrowObject1 = go.transform.GetChild(0).gameObject;
                        GameObject childArrowObject2 = go.transform.GetChild(1).gameObject;
                        //Create references to the renderers of the children of the arrow prefab
                        Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                        Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                        //Create a reference to the collider component of the arrow prefab
                        collider = go.GetComponent<Collider>();
                        //Enable components
                        rendererChildObject1.enabled = true;
                        rendererChildObject2.enabled = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        //Find the arrow instance
                        go = GameObject.Find(counterString);
                        //Get the children of the arrow instance
                        GameObject childArrowObject1 = go.transform.GetChild(0).gameObject;
                        GameObject childArrowObject2 = go.transform.GetChild(1).gameObject;
                        //Create references to the renderers of the children of the arrow prefab
                        Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                        Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                        //Before turning off the renderers of the children of the arrow prefab, set the color to default since it is not a chosen object anymore
                        rendererChildObject1.material.color = new Color(1, 1, 1, 1);//newly added
                        rendererChildObject2.material.color = new Color(1, 1, 1, 1);
                        //Create a reference to the collider component of the arrow prefab
                        collider = go.GetComponent<Collider>();
                        //Disable components
                        rendererChildObject1.enabled = false;
                        rendererChildObject2.enabled = false;
                        collider.enabled = false;
                    }

                }
                else
                {
                    //Compare current step and the object's step value
                    if (dict["StepValue"] == currentStep)
                    {
                        //Enable renderer and collider components of the object
                        go = GameObject.Find(counterString);
                        renderer = go.GetComponent<MeshRenderer>();
                        collider = go.GetComponent<Collider>();
                        renderer.enabled = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        //Disable renderer and collider components of the object
                        go = GameObject.Find(counterString);
                        renderer = go.GetComponent<MeshRenderer>();
                        //Before turning off the renderer set the color to default since it is not a chosen object anymore
                        renderer.material.color = new Color(1, 1, 1, 1);//newly added
                        collider = go.GetComponent<Collider>();
                        renderer.enabled = false;
                        collider.enabled = false;
                    }



                }


            }
            else
            {


            }





        }

    }
    //Function to be called when the previous button is clicked to change the step
    public void OnClickPrevious()
    {
        //restore distinctive number and the current step
        int numberofObjects = PlayerPrefs.GetInt("DistinctiveNumber");
        float currentStep = PlayerPrefs.GetFloat("Step");
        //Decrement the current step
        currentStep--;
        //Update the step text
        StepUIText.text = "Step " + currentStep.ToString();
        //
        PlayerPrefs.SetFloat("Step", currentStep);
        //variable to hold string version of the counter in the for loop
        string counterString;
        //variable to hold the restored object string data
        string keyStringHolder;
        //Variables to hold the gameobject's renderer and collider components which will be enabled or disabled depending on the current step value
        Collider collider;
        Renderer renderer;
        //Dictionary variable is going to be used to compare objects Step value(the step it was created) to the current step value
        Dictionary<string, float> dict = new Dictionary<string, float>();
        //Gameobject variable to access the gameobjects
        GameObject go;


        //for loop will iterate for all objectsstarting from the first until it finishes with the last object created.
        for (int counter = 1; counter <= numberofObjects; counter++)
        {
            if (PlayerPrefs.HasKey(counter.ToString()))
            {
                //Convert counter to string
                counterString = counter.ToString();
                //Get the corresponding objects data
                keyStringHolder = PlayerPrefs.GetString(counterString);
                //Convert the data to dictionary type to access its step value
                dict = ConvertStringToDict(keyStringHolder);
                //Assign 1 to the arrow type variable
                int arrowType = 1;
                if (dict["Type"] == arrowType)
                {
                    //Compare current step and the object's step value
                    if (dict["StepValue"] == currentStep)
                    {
                        //Find the arrow instance
                        go = GameObject.Find(counterString);
                        //Get the children of the arrow prefab
                        GameObject childArrowObject1 = go.transform.GetChild(0).gameObject;
                        GameObject childArrowObject2 = go.transform.GetChild(1).gameObject;
                        //Create references to the renderers of the children of the arrow prefab
                        Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                        Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                        //Create a reference to the collider component of the arrow prefab
                        collider = go.GetComponent<Collider>();
                        //Enable components
                        rendererChildObject1.enabled = true;
                        rendererChildObject2.enabled = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        //Find the arrow instance
                        go = GameObject.Find(counterString);
                        //Get the children of the arrow prefab
                        GameObject childArrowObject1 = go.transform.GetChild(0).gameObject;
                        GameObject childArrowObject2 = go.transform.GetChild(1).gameObject;
                        //Create references to the renderers of the children of the arrow prefab
                        Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                        Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                        //Before turning off the renderers of the children of the arrow prefab, set the color to default since it is not a chosen object anymore
                        rendererChildObject1.material.color = new Color(1, 1, 1, 1);
                        rendererChildObject2.material.color = new Color(1, 1, 1, 1);
                        //Create a reference to the collider component of the arrow prefab
                        collider = go.GetComponent<Collider>();
                        //Disable components
                        rendererChildObject1.enabled = false;
                        rendererChildObject2.enabled = false;
                        collider.enabled = false;
                    }


                }
                else
                {
                    //Compare current step and the object's step value
                    if (dict["StepValue"] == currentStep)
                    {
                        //Enable renderer and collider components of the object
                        go = GameObject.Find(counterString);
                        renderer = go.GetComponent<MeshRenderer>();
                        collider = go.GetComponent<Collider>();
                        renderer.enabled = true;
                        collider.enabled = true;
                    }
                    else
                    {
                        //Disable renderer and collider components of the object
                        go = GameObject.Find(counterString);
                        renderer = go.GetComponent<MeshRenderer>();
                        //Before turning off the renderer set the color to default since it is not a chosen object anymore
                        renderer.material.color = new Color(1, 1, 1, 1);//newly added
                        collider = go.GetComponent<Collider>();
                        renderer.enabled = false;
                        collider.enabled = false;
                    }



                }



            }
            else
            {


            }



        }



    }
    //Function to change the color of the selected object, it will also set the color of 'unchosen' objects to default
    public void HighlightSelectedObject(int id)
    {

        int numberOfObjects = PlayerPrefs.GetInt("DistinctiveNumber");
        for (int i = 1; i <= numberOfObjects; i++)
        {
            //Check if the key exists
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                //Find the gameobject
                GameObject go = GameObject.Find(i.ToString());
                //Create a reference to DistinctiveObjectData component
                DistinctiveObjectData distinctiveObjectData_ = go.GetComponent<DistinctiveObjectData>();
                //Check if the selected object is an arrow
                if (distinctiveObjectData_.type == 1)
                {
                    //Get the children of the arrow prefab
                    GameObject childArrowObject1 = go.transform.GetChild(0).gameObject;
                    GameObject childArrowObject2 = go.transform.GetChild(1).gameObject;
                    //Create references to the renderers of the children
                    Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                    Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                    //Among all the objects, only selected object will have the color cyan
                    if (go.name == id.ToString())
                    {
                        //Change color to cyan
                        rendererChildObject1.material.color = new Color(0, 1, 1, 1);
                        rendererChildObject2.material.color = new Color(0, 1, 1, 1);
                    }
                    else
                    {
                        //Set the color to default for all 'unchosen' arrows
                        rendererChildObject1.material.color = new Color(1, 1, 1, 1);
                        rendererChildObject2.material.color = new Color(1, 1, 1, 1);
                    }

                }
                //If the object is not an arrow
                else
                {
                    //Create a reference to the renderer component of the object
                    Renderer renderer = go.GetComponent<Renderer>();
                    //Among all the objects, only selected object will have the color cyan
                    if (go.name == id.ToString())
                    {
                        //change color to cyan
                        renderer.material.color = new Color(0, 1, 1, 1);
                    }
                    else
                    {
                        //set the colour default for all other 'unchosen' objects
                        renderer.material.color = new Color(1, 1, 1, 1);

                    }

                }


            }
            else
            {

            }





        }

    }
    //Function to disable all move functions for the objects when next or previous buttons are clicked.
    //When one of the next or previous buttons are clicked value of the tag variable(just a string variable to hold the name)  
    //should be set to something that a gameobject name can never be.
    public void DeselectObject()
    {
        MyTag = "ImpossibleName";

    }

    public void DeleteObject()
    {
        GameObject go = GameObject.Find(MyTag);
        Destroy(go);
        try
        {
            PlayerPrefs.DeleteKey(MyTag);
            Debug.Log("You have deleted the object : " + MyTag);
        }
        catch
        {
            Debug.Log("You need to select an object to delete");

        }




    }
    #endregion

    #region Unity Functions
    void Start()
    {
        PlayerPrefs.SetFloat("Step", 1);
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //create ray and RaycastHit objects
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int currentObjectCount;


            //check if the ray collides with the objects 
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                //Get GameObject and log its position
                GameObject HitObjectGO = hit.transform.gameObject;
                //Debug.Log("Hit GameObject Position X is: " + HitObjectGO.transform.position.x);
                //Debug.Log("Hit GameObject Position Y is: " + HitObjectGO.transform.position.y);
                //Debug.Log("Hit GameObject Position Z is: " + HitObjectGO.transform.position.z);

                //Get GameObject and log its id
                DistinctiveObjectData distinctiveObjectData_ = HitObjectGO.GetComponent<DistinctiveObjectData>();
                //Get the distinctive id and hold it in a string variable for later use when accessing the touched gameobject
                MyTag = distinctiveObjectData_.id.ToString();
                //Change the gameobjects name before calling the HighlightSelectedObject function
                HitObjectGO.name = MyTag;
                //Change the color of the selected object while setting the color for all other objects to default
                HighlightSelectedObject(distinctiveObjectData_.id);

                Debug.Log("Hit GameObject saved ID: " + distinctiveObjectData_.id);
                Debug.Log("Pos X " + HitObjectGO.transform.position.x);
                Debug.Log("Pos Y " + HitObjectGO.transform.position.y);
                Debug.Log("Pos Z " + HitObjectGO.transform.position.z);
                currentObjectCount = PlayerPrefs.GetInt("DistinctiveNumber");

                //get the collided objects name
                hit_instance_name = hit.transform.name;
                Debug.Log("Hit GameObject name" + hit_instance_name);
                Debug.Log("Total number of objects are : " + currentObjectCount);
            }
        }
    }
    #endregion
}
