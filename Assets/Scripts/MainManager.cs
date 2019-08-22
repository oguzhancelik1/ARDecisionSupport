﻿using System.Collections;
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
    //Movement Buttons 
    public GameObject MovementButtons;
    //Array of Objects Prefabs
    public GameObject[] ARObjectsArray;
    //Declaration of the generative objects
    public GameObject generativeCube;
    public GameObject generativeSphere;
    public GameObject generativeArrow;

    //UI text to indicate the current Step
    public Text StepUIText;

    //camera to be used for the raycasting
    public Camera cam;
    #endregion

    #region Private Variables

    //holds whichever object is added from the generative objects
    GameObject currentObject;

    //Currently Selected GameObject
    private string SelectedGameObject;

    //Small shift value for each created gameobject to avoid multiple objects created on each other
    private float NewGOPositionShift = 0f;

    //Image target gameobject in the scene
    private GameObject ImageTarget;

    //Current Step (Starts from 1 always)
    public static int CurrentStep;

    public int GetCurrentStep()
    {
        return CurrentStep;
    }
    //Total number of created objects
    private int NumberOfObjects;
    //Position shift for new objects
    private float instantiationPositionShift = 0;

    //Layer that holds the created objects which will collide with the raycast
    int layerMask = 0xffffff;

    //Step for Moving on X, Y and Z axis
    private readonly float MOVING_STEP = 0.1f;
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

    /// ConvertDictToString converts a given string, float dictionary to a string using the GetLine method.
    public string ConvertDictToString(Dictionary<string, float> dict)
    {
        //Convert dictionary to string and save
        string objectTypeString = GetLine(dict);
        return objectTypeString;
    }

    //Takes a string returns a dictionary of type <string, float>
    public Dictionary<string, float> ConvertStringToDict(string f)
    {
        //Debug.Log("String to be converted to Dict is: " + f);
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

    #region Select and Create Object Buttons
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
        /********************************************************************************
         * Create New Object Algorithm
         ********************************************************************************
         * 
        /*  Create new dictionary
         *  Create new gameobject
         *  Increment the numberofObjects
         *  Save the NumberOfObjects to playerprefs
         *  Set the new object name and ID to the new numberofObjects
         *  Set the stepvalue of the new object to the current step
         *  Check the type of the currently selected object to be created in the menu
         *  Instantiate the new GameObject in the scene
         *  Save the type attribute to dictionary
         *  Set the imagetarget to be the parent
         *  Set the position of the new gameobject with position shift
         *  Save the position data to dictionary
         *  Increment the position shift
         *  Set the layer of the gameobject for raycasting
         *  Convert dictionary to string
         *  Save the string to playerprefs by the name of the new object ID
         *  ******************************************************************************
        */

        //Create a new dictionary
        Dictionary<string, float> NewDictionary = new Dictionary<string, float>();

        //Create new GameObject
        GameObject NewGameObject = new GameObject();

        //Increment Number of Saved Objects and save it
        NumberOfObjects++;
        PlayerPrefs.SetInt("NumberOfObjects", NumberOfObjects);

        //FIND THE SELECTED TYPE TO BE CREATED
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

        //INSTANTIATE NEW OBJECT and save the type attribute
        NewGameObject = Instantiate(ARObjectsArray[IndexOfObjectToBeCreated]);
        FillDictionary(NewDictionary, "Type", IndexOfObjectToBeCreated);

        //Set object name to the new number of objects
        string NewObjectID = NumberOfObjects.ToString();
        NewGameObject.name = NewObjectID;
        FillDictionary(NewDictionary, "ID", float.Parse(NewObjectID));

        //Fill the step value of the object with the step it is created in
        FillDictionary(NewDictionary, "StepValue", CurrentStep);

        //Set the image target as parent
        NewGameObject.transform.parent = ImageTarget.transform;

        //Set the local position and save it
        NewGameObject.transform.localPosition = new Vector3(NewGOPositionShift, 0f, 0f);
        FillDictionary(NewDictionary, "PosX", NewGameObject.transform.localPosition.x);
        FillDictionary(NewDictionary, "PosY", NewGameObject.transform.localPosition.y);
        FillDictionary(NewDictionary, "PosZ", NewGameObject.transform.localPosition.z);

        //Increment the PositionShift to avoid multiple objects on the same spot when creating another object
        NewGOPositionShift = NewGOPositionShift + MOVING_STEP;

        //set every NewGameObject in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
        NewGameObject.layer = 9;

        //Convert dictionary to string
        String StringOfObjectData = ConvertDictToString(NewDictionary);

        //Save the stringified dictionary by the its ID
        PlayerPrefs.SetString(NewObjectID, StringOfObjectData);
    }
    #endregion

    #region Moving on X,Y,Z axis buttons
    //move the last selected object +x direction with the click on button Move_Object_on_X_Positive
    public void Move_Object_on_X_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);
        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string a = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(a);

        //change the location on +x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x + MOVING_STEP, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosX"] = currentObject.transform.localPosition.x;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        a = ConvertDictToString(dict);
        Debug.Log(a);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, a);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -x direction with the click on button Move_Object_on_X_Negative
    public void Move_Object_on_X_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string b = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(b);

        //change the location on -x
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x - MOVING_STEP, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosX"] = currentObject.transform.localPosition.x;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        b = ConvertDictToString(dict);
        Debug.Log(b);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, b);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +y direction with the click on button Move_Object_on_Y_Positive
    public void Move_Object_on_Y_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);

        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string c = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(c);

        //change the location on +y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y + MOVING_STEP, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosY"] = currentObject.transform.localPosition.y;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        c = ConvertDictToString(dict);
        Debug.Log(c);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, c);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -y direction with the click on button Move_Object_on_Y_Negative
    public void Move_Object_on_Y_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string d = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(d);

        //change the location on -y
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y - MOVING_STEP, currentObject.transform.localPosition.z);

        //Update the dictionary
        dict["PosY"] = currentObject.transform.localPosition.y;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        d = ConvertDictToString(dict);
        Debug.Log(d);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, d);
        Debug.Log("New position data stored.");
    }
    //move the last selected object +z direction with the click on button Move_Object_on_Z_Positive
    public void Move_Object_on_Z_Positive()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string e = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(e);

        //change the location on -z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z + MOVING_STEP);

        //Update the dictionary
        dict["PosZ"] = currentObject.transform.localPosition.z;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        e = ConvertDictToString(dict);
        Debug.Log(e);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, e);
        Debug.Log("New position data stored.");
    }
    //move the last selected object -z direction with the click on button Move_Object_on_Z_Negative
    public void Move_Object_on_Z_Negative()
    {
        //get the last touched object
        currentObject = GameObject.Find(SelectedGameObject);


        Dictionary<string, float> dict = new Dictionary<string, float>();

        //get the latest version of the string value that holds the information on the object
        string f = PlayerPrefs.GetString(SelectedGameObject);

        //Convert the string back to dictionary for manipulation
        dict = ConvertStringToDict(f);

        //change the location on -z
        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, currentObject.transform.localPosition.z - MOVING_STEP);

        //Update the dictionary
        dict["PosZ"] = currentObject.transform.localPosition.z;
        //FillDictionary(dict,"PosZ", currentObject.transform.localPosition.z);

        //Convert the dictionary back to the string to set the playerprefs
        f = ConvertDictToString(dict);
        Debug.Log(f);
        //save the new string for the object
        PlayerPrefs.SetString(SelectedGameObject, f);
        Debug.Log("New position data stored.");
    }
    #endregion

    #region Next Step and Previous Step Buttons
    //This function renders objects based on the step number
    public void UpdateRenderedObjects()
    {
        //Reset the position shift of newly created objects with each step
        NewGOPositionShift = 0;

        //for loop will iterate for all objects starting from the first until it finishes with the last object created.
        for (int counter = 1; counter <= PlayerPrefs.GetInt("NumberOfObjects"); counter++)
        {
            if (PlayerPrefs.HasKey(counter.ToString()))
            {
                //Get the Dictionary of this Object
                Dictionary<string, float> ObjectDictionary = ConvertStringToDict(PlayerPrefs.GetString(counter.ToString()));

                //Gameobject variable to access the gameobjects
                GameObject go;
                go = GameObject.Find(counter.ToString());

                if (go != null)
                {
                    //Compare current step and the object's step value
                    if (ObjectDictionary["StepValue"] == CurrentStep)
                    {
                        //Enable renderer and collider components of the object
                        go.GetComponent<MeshRenderer>().enabled = true;
                        go.GetComponent<Collider>().enabled = true;
                    }
                    else
                    {
                        //Disable renderer and collider components of the object
                        go.GetComponent<MeshRenderer>().enabled = false;
                        go.GetComponent<Collider>().enabled = false;
                    }
                }
            }
        }
    }

    //Function to be called when the next button is clicked to change the step
    public void OnClickNext()
    {   
        //Increment the current step 
        CurrentStep++;
        //Update the step text
        StepUIText.text = "Step " + CurrentStep.ToString();

        //Update Rendered based on current step
        UpdateRenderedObjects();
    }

    //Function to be called when the previous button is clicked to change the step
    public void OnClickPrevious()
    {
        //Decrement the current step 
        CurrentStep--;
        if (CurrentStep < 1) CurrentStep = 1;

        //Update the step text
        StepUIText.text = "Step " + CurrentStep.ToString();

        //Update Rendered based on current step
        UpdateRenderedObjects();
    }

    //Function to change the color of the selected object, it will also set the color of 'unchosen' objects to default
    public void HighlightSelectedObject(string id)
    {
        int numberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");
        for (int i = 1; i <= numberOfObjects; i++)
        {
            //Check if the key exists or this is the first created object and its not saved yet
            if (PlayerPrefs.HasKey(i.ToString()) || NumberOfObjects ==1)
            {
                //Find the gameobject
                GameObject go = GameObject.Find(i.ToString());
                
                //Create a reference to the renderer component of the object
                Renderer renderer = go.GetComponent<Renderer>();
                //Among all the objects, only selected object will have the color cyan
                if (go.name == id)
                {
                    //change color to cyan
                    renderer.material.color = new Color(0, 1, 1, 1);
                }
                else
                {
                    //set the colour default for all other 'unchosen' objects
                    renderer.material.color = new Color(1, 1, 1, 1);
                }

                //Display the move buttons
                MovementButtons.SetActive(true);
            }
        }
    }

    //Function to disable all move functions for the objects when next or previous buttons are clicked.
    //When one of the next or previous buttons are clicked value of the tag variable(just a string variable to hold the name)  
    //should be set to something that a gameobject name can never be.
    public void DeselectObject()
    {
        SelectedGameObject = "ImpossibleName";
        int numberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");
        for (int i = 1; i <= numberOfObjects; i++)
        {
            //Check if the key exists or this is the first created object and its not saved yet
            if (PlayerPrefs.HasKey(i.ToString()) || NumberOfObjects == 1)
            {
                //Find the gameobject
                GameObject go = GameObject.Find(i.ToString());
                if(go!=null)
                {
                    //Create a reference to the renderer component of the object
                    Renderer renderer = go.GetComponent<Renderer>();

                    //set the colour default for all other 'unchosen' objects
                    renderer.material.color = new Color(1, 1, 1, 1);
                }                
            }
        }
            //Hide the move buttons
            MovementButtons.SetActive(false);
    }

    public void DeleteObject()
    {
        GameObject go = GameObject.Find(SelectedGameObject);
        if(go!= null)
        {
            Destroy(go);
            PlayerPrefs.DeleteKey(SelectedGameObject);
        }
        DeselectObject();
    }
    #endregion

    #region Unity Functions
    void Start()
    {
       
        //Hide the move buttons
        MovementButtons.SetActive(false);

        CurrentStep = 1;
        //Get the Main Camera GameObject
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //Find the ImageTarget gameobject
        ImageTarget = GameObject.Find("ImageTarget");
        DefaultTrackableEventHandler DefaultTrackableEventHandler_ = ImageTarget.GetComponent<DefaultTrackableEventHandler>();
        

        //Restore the total number of objects
        NumberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");
        
        if(NumberOfObjects>0)
        {
            //Update rendering based on current step
            UpdateRenderedObjects();
        }
    }

    void Update()
    {
        //HANDLING OF SELECTING OBJECTS
        if (Input.GetMouseButtonDown(0))
        {
            //create ray and RaycastHit objects
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //check if the ray collides with the objects 
            if (Physics.Raycast(ray, out hit, layerMask))
            {
                if(hit.transform.gameObject.layer == 9)
                {
                    //Get GameObject and log its position
                    GameObject HitObjectGO = hit.transform.gameObject;

                    //Get the distinctive id and hold it in a string variable for later use when accessing the touched gameobject
                    SelectedGameObject = HitObjectGO.name;

                    //Change the color of the selected object while setting the color for all other objects to default
                    HighlightSelectedObject(SelectedGameObject);
                }
                else //Raycast doesn't collide with any objects
                {
                    DeselectObject();
                }
            }
        }
    }
    #endregion
}