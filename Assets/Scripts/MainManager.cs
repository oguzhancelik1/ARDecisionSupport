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
    //Movement Buttons 
    public GameObject MovementButtons;
    //Array of Objects Prefabs
    public GameObject[] ARObjectsArray;
    //Declaration of the generative objects
    public GameObject generativeCube;
    public GameObject generativeSphere;
    public GameObject generativeArrow;

    public GameObject GrabObjectWarning;

    //UI text to indicate the current Step
    public Text StepUIText;

    //camera to be used for the raycasting
    public Camera cam;
    #endregion

    #region Private Variables

    GameObject CreatedObjectGO = null;
    //holds whichever object is added from the generative objects
    GameObject currentObject;

    //Currently Selected GameObject
    private string SelectedGameObject;

    private static GameObject ObjectToRelease;

    //Small shift value for each created gameobject to avoid multiple objects created on each other
    private float NewGOPositionShift = 0f;

    //Image target gameobject in the scene
    private GameObject ImageTarget;

    //Position value of the camera when an object is selected
    private Vector3 ARCameraPositionWhenObjectSelected;

    private Vector3 ARCameraPositionWhenObjectReleased;

    //Position value of the object when it is selected
    private Vector3 objectPositionWhenSelected;

  
    //Current Step (Starts from 1 always)
    public static int CurrentStep;

    public int GetCurrentStep()
    {
        return CurrentStep;
    }
    //Total number of objects created
    

    //Total number of released objects
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

        // Divide all pairs (remove empty strings)
        string[] tokens = f.Split(new char[] { ':', ',' });//, StringSplitOptions.RemoveEmptyEntries);

        // Walk through each item
        for (int i = 0; i < tokens.Length; i += 2)
        {
            string name = tokens[i];
            string freq = tokens[i + 1];

            // Fill the value in the sorted dictionary
            d.Add(name, float.Parse(freq));
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
        //PlayerPrefs.SetInt("NumberOfObjects", NumberOfObjects);

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
        //FillDictionary(NewDictionary, "Type", IndexOfObjectToBeCreated);

        //Set object name to the new number of objects
        string NewObjectID = NumberOfObjects.ToString();
        NewGameObject.name = NewObjectID;
        //FillDictionary(NewDictionary, "ID", float.Parse(NewObjectID));

        //Fill the step value of the object with the step it is created in
        //FillDictionary(NewDictionary, "StepValue", CurrentStep);

        //Set the image target as parent
        //NewGameObject.transform.parent = ImageTarget.transform;
   
        //Set the local position and save it
        NewGameObject.transform.position = new Vector3(NewGOPositionShift + ImageTarget.transform.position.x, ImageTarget.transform.position.y, ImageTarget.transform.position.z);
        //FillDictionary(NewDictionary, "PosX", NewGameObject.transform.localPosition.x);*************
        //FillDictionary(NewDictionary, "PosY", NewGameObject.transform.localPosition.y);************
        //FillDictionary(NewDictionary, "PosZ", NewGameObject.transform.localPosition.z);************

        //Increment the PositionShift to avoid multiple objects on the same spot when creating another object
        NewGOPositionShift = NewGOPositionShift + MOVING_STEP;

        //set every NewGameObject in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
        NewGameObject.layer = 9;
        //////////******************************
        DistinctiveObjectData distinctiveObjectData = NewGameObject.GetComponent<DistinctiveObjectData>();
        
      
        distinctiveObjectData.type = IndexOfObjectToBeCreated;

        CreatedObjectGO = NewGameObject;
        /////////////************************
        ///

        //Convert dictionary to string
        //String StringOfObjectData = ConvertDictToString(NewDictionary);**************

        //Save the stringified dictionary by the its ID
        //PlayerPrefs.SetString(NewObjectID, StringOfObjectData);*****************
    }
    public void ReleaseObject()
    {
        Camera ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();

        Dictionary<string, float> NewDictionary = new Dictionary<string, float>();

        //Get the position info of the ARCamera when the object was selected

        //float x1 = ARCameraPositionWhenObjectSelected.x;
        //float y1 = ARCameraPositionWhenObjectSelected.y;
        //float z1 = ARCameraPositionWhenObjectSelected.z;

        //Get the current ImageTarget position 

        //float x2 = ImageTarget.transform.position.x;
        //float y2 = ImageTarget.transform.position.y;
        //float z2 = ImageTarget.transform.position.z;


        //Get the object position info when it was selected

        //float x0 = objectPositionWhenSelected.x;
        //float y0 = objectPositionWhenSelected.y;
        //float z0 = objectPositionWhenSelected.z;
        //
        //
        ////Calculate new object position
        ////Since the world center mode is 'FirstTarget', ImageTarget position values for x, y and z are going to be 0.
        //
        //float xFinalPosition =  ARCamera.transform.position.x - (x1 - x0);
        //float yFinalPosition =  ARCamera.transform.position.y - (y1 - y0);
        //float zFinalPosition =  ARCamera.transform.position.z - (z1 - z0);


        


        //Find the selected game object
        ObjectToRelease = GameObject.Find(SelectedGameObject);
        //ObjectToRelease.transform.position = ARCameraPositionWhenObjectReleased;

        //Get its type value using the DistinctiveObjectData component. Hold the value in a float variable
        DistinctiveObjectData distinctiveObjectData = ObjectToRelease.GetComponent<DistinctiveObjectData>();
        float TypeValue = (float)distinctiveObjectData.type;

        //Fill in the type key in the dictionary with the type value
        FillDictionary(NewDictionary, "Type", TypeValue);

        //Assign the object name to the ID value, then fill the dictionary 
        string NewObjectID = ObjectToRelease.name;
        FillDictionary(NewDictionary, "ID", float.Parse(NewObjectID));

        //Assıgn the step value ın which the object ıs released
        FillDictionary(NewDictionary, "StepValue", CurrentStep);
       

        Vector3 Pos = ObjectToRelease.transform.position;
        ObjectToRelease.transform.parent = null;

        ObjectToRelease.transform.position = Pos;


        //After release, the object should go back to the default color white
        Renderer goRenderer = ObjectToRelease.GetComponent<MeshRenderer>();
        goRenderer.enabled = true;
        goRenderer.material.color = new Color(1, 1, 1, 1);

        //Assign the final position info to the Object to release
        //ObjectToRelease.transform.position = new Vector3(xFinalPosition, yFinalPosition, zFinalPosition);

        

        //Fill the position data to the dictionary
        FillDictionary(NewDictionary, "PosX", ObjectToRelease.transform.position.x);   
        FillDictionary(NewDictionary, "PosY", ObjectToRelease.transform.position.y);    
        FillDictionary(NewDictionary, "PosZ", ObjectToRelease.transform.position.z);   
        
        //Save the number of objects that were released
        PlayerPrefs.SetInt("NumberOfObjects", NumberOfObjects);

        //Convert dictionary to string then assign to a variable
        string StringOfObjectData = ConvertDictToString(NewDictionary);
        Debug.Log(StringOfObjectData);

        ////Save the stringified dictionary
        PlayerPrefs.SetString(NewObjectID, StringOfObjectData);

   

        SelectionSuccessChecker = false;
        HitObjectGO = null;
    }
  
    //Cancel the new object creation without releasing it.
    //Cancel the object selection, it will go back to the location where it was selected.
    public void CancelCreationOrSelection()
    {
        //Find the ARCamera in the scene
        Camera ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
       
        //Loop to iterate through all the objects created since the beginning starting from 1
        for (int counter = 1; counter <= NumberOfObjects; counter++)
        {
            //Find the game object with the name counter
            GameObject go = GameObject.Find(counter.ToString());
            
            //If the game object exists with the name counter
            if (go != null)
            {
                //Get the renderer component of the selected object
                Renderer goRenderer = go.GetComponent<MeshRenderer>();

                //If ARCamera is the parent of the of the object
                if (go.transform.parent == ARCamera.transform)
                {
                    //If the object was created and saved earlier, reverse its position back to where it was selected
                    if (PlayerPrefs.HasKey(counter.ToString()))
                    {
                       

                        //Create a dictionary to retrieve the values of the object from its string
                        Dictionary<string, float> dict = new Dictionary<string, float>();
                        dict = ConvertStringToDict(PlayerPrefs.GetString(counter.ToString()));
                        
                        //Selection is going to be canceled so the color of the object should go back to the default
                        goRenderer.material.color = new Color(1, 1, 1, 1);

                        //Set the image target parent and place it where it used to be 
                        go.transform.parent = null;
                        go.transform.position = new Vector3(dict["PosX"], dict["PosY"], dict["PosZ"]);

                        SelectionSuccessChecker = false;
                    }

                    //If a string does not exist, this object was never saved
                    else
                    {
                        //Set its position back to the default place which is the image target position
                        go.transform.position = new Vector3(0, 0, 0);

                        //Set its color back to the default
                        goRenderer.material.color = new Color(1, 1, 1, 1);

                        //Set the flag to false 
                        SelectionSuccessChecker = false;
                    }

                }
                else
                {
                    //No object was created or selected. Thus nothing to reverse
                }

            }
            
         
        }


        

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


    float dist;
    //Function to change the color of the selected object, it will also set the color of 'unchosen' objects to default
    public void HighlightSelectedObject(string id)
    {
        //Find the ARCamera in the scene 
        Camera ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();

        //Find the created and released object number since the beginning
        int numberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");


        //*******************************************************************************************************
        //Loop to iterate through the objects
        //numberOfObjects represents number of objects that were released and saved.
        //NumberOfObjects represents number of objects created on a single runtime.
        //Adding the two values in the for loop ensures even if no object was saved we can still select when an object is first created
        //*******************************************************************************************************

        for (int i = 1; i <= numberOfObjects + NumberOfObjects; i++)
        {
            //Find the gameobject
            GameObject go = GameObject.Find(i.ToString());
           
            if (go != null)
            {

                Debug.Log("ARCamera y position : " + ARCamera.transform.position.y);
                Debug.Log("go y position : " + go.transform.position.y);

                Debug.Log("ARCamera x position : " + ARCamera.transform.position.x);
                Debug.Log("go x position : " + go.transform.position.x);

                Debug.Log("ARCamera z position : " + ARCamera.transform.position.z);
                Debug.Log("go z position : " + go.transform.position.z);

               
                
                //Create a reference to the renderer component of the object
                Renderer renderer = go.GetComponent<Renderer>();

                DistinctiveObjectData distinctiveObjectData = new DistinctiveObjectData();
                
                if(go.GetComponent<DistinctiveObjectData>() != null)
                {
                    if (go.name == id)
                    {
                        //change color to cyan
                        renderer.material.color = new Color(0, 1, 1, 1);

                        //Get the position of the object
                        objectPositionWhenSelected = go.transform.position;

                        dist = Vector3.Distance(objectPositionWhenSelected, ARCamera.transform.position);

                        SelectionSuccessChecker = true;
                        

                        GrabObjectWarning.SetActive(false);
                        CreatedObjectGO = null;
                        

                        //Hold the position vector of the ARCamera in the ARCameraPositionWhenObjectSelected variable
                        //ARCameraPositionWhenObjectSelected = new Vector3(ARCamera.transform.position.x, ARCamera.transform.position.y, ARCamera.transform.position.z);
                    }
                    else
                    {
                        //set the colour default for all other 'unchosen' objects
                        renderer.material.color = new Color(1, 1, 1, 1);
                    }

                    //Display the move buttons
                    MovementButtons.SetActive(true);

                }
                //Among all the objects, only selected object will have the color cyan
            }
        }
    }

    //Function to disable all move functions for the objects when next or previous buttons are clicked.
    //When one of the next or previous buttons are clicked, value of the SelectedGameObject,
    //should be set to something that a gameobject name can never be.
    public void DeselectObject()
    {
        //Set the name 
        SelectedGameObject = "ImpossibleName";

        //Find the created and released object number since the beginning
        int numberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");

        //Loop to iterate through the objects
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
        //Find the selected game object
        GameObject go = GameObject.Find(SelectedGameObject);

        //If the game object with such name exists
        if (go!= null)
        {
            //Delete the object
            Destroy(go);

            //Delete its saved string
            PlayerPrefs.DeleteKey(SelectedGameObject);
        }
        DeselectObject();
    }
    #endregion
    public void ResetPlayerPrefs()
    {

        PlayerPrefs.DeleteAll();


    }
    


    #region Unity Functions
    void Start()
    {
       
        //Hide the move buttons
        MovementButtons.SetActive(false);

        CurrentStep = 1;
        //Get the Main ARCamera GameObject
        //cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCamera>();

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

    bool SelectionSuccessChecker = false;
    
    GameObject HitObjectGO = null;

    float distancefromY = 0.20f;

    void Update()
    {
        Camera ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();
        
        
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
                    HitObjectGO = hit.transform.gameObject;
                    //Get the distinctive id and hold it in a string variable for later use when accessing the touched gameobject

                    //Check if the camera is close enough to the object
                    if (hit.distance <= 1)
                    {
                        SelectedGameObject = HitObjectGO.name;
                        Debug.Log("Selected object was : " + SelectedGameObject);

                   
                        //Change the color of the selected object while setting the color for all other objects to default

                        HighlightSelectedObject(SelectedGameObject);

                        HitObjectGO.transform.parent = ARCamera.transform;
                        HitObjectGO.transform.localPosition = new Vector3(0,0,0.20f);

                        //Set the warning message false when the distance condition is met

                    }
                    else
                    {
                        //Set the warning message true when distance condition is not met
                        GrabObjectWarning.SetActive(true);
                        SelectionSuccessChecker = false;
                    }
                }
                else //Raycast doesn't collide with any objects
                {
                    
                    DeselectObject();
                }
            }
        }

        //Object pos it related to camera
        //if (SelectionSuccessChecker && HitObjectGO != null)
        //{

        //    //float xFinalPosition = ARCamera.transform.position.x - (ARCameraPositionWhenObjectSelected.x - objectPositionWhenSelected.x);
        //    //float yFinalPosition = ARCamera.transform.position.y - (ARCameraPositionWhenObjectSelected.y - objectPositionWhenSelected.y);
        //    //float zFinalPosition = ARCamera.transform.position.z - (ARCameraPositionWhenObjectSelected.z - objectPositionWhenSelected.z);
        //    //
        //    //HitObjectGO.transform.position = new Vector3(xFinalPosition, yFinalPosition, zFinalPosition);
        //    //Set the object position equal to the camera
        //    //HitObjectGO.transform.position = ARCamera.transform.position + dist;
        //    //HitObjectGO.transform.position = (HitObjectGO.transform.position - ARCamera.transform.position).normalized * dist + ARCamera.transform.position;
            
        //   // HitObjectGO.transform.position = new Vector3(ARCamera.transform.position.x, ARCamera.transform.position.y - 0.2f , ARCamera.transform.position.z);
        //    //ARCameraPositionWhenObjectReleased = HitObjectGO.transform.position;         
        //}
        //Object position is related to the image target
        if (CreatedObjectGO != null)
        {
            //Set the object position equal to the camera
            CreatedObjectGO.transform.position = ImageTarget.transform.position;
        }

        //if( ObjectToRelease != null && ImageTarget != null)
        //{
            
        //    ObjectToRelease.transform.position = ObjectToRelease.transform.position - ImageTarget.transform.position;
        //}


    }
    #endregion
}