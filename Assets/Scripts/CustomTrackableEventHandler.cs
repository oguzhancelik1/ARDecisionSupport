using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : MainManager, ITrackableEventHandler
{
    //public GameObject ExistingPrefab;
    //public GameObject ImageTarget;
    #region PROTECTED_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName +
                  " " + mTrackableBehaviour.CurrentStatus +
                  " -- " + mTrackableBehaviour.CurrentStatusInfo);

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        if (mTrackableBehaviour)
        {
#if false
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
#region previous version
            //Restore Saved position data
            //If the restored data are all zeros, ignore

            /*
            if (PlayerPrefs.GetFloat("TransformPosX") == 0
                && PlayerPrefs.GetFloat("TransformPosY") == 0
                && PlayerPrefs.GetFloat("TransformPosZ") == 0)
            {
                Debug.Log("All restored data are zeros.");
            }
            else //There are valid position data
            {
                Debug.Log("There are valid data restored.");


                
                //Instantiate a new GameObject
                GameObject prefabInstance;
                prefabInstance = Instantiate(ExistingPrefab);

                //Set the ImageTarget as a parent
                prefabInstance.transform.parent = ImageTarget.transform;

                //Set the local position of the newly created gameobject into the ones in PlayerPrefs
                prefabInstance.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("TransformPosX"), PlayerPrefs.GetFloat("TransformPosY"), PlayerPrefs.GetFloat("TransformPosZ"));
                //Set the rotation to Zeros
                prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                //Set the layer for raycast masking
                prefabInstance.layer = 9;
                
                
            }*/
#endregion
#endif   

            if (PlayerPrefs.HasKey("DistinctiveNumber"))
            {
                int dinstinctiveNumber = PlayerPrefs.GetInt("DistinctiveNumber");
                int sphereType = 0;
                int arrowType = 1;
                int cubeType = 2;
                GameObject MyGameObject;
                for (int counter = 1; counter <= dinstinctiveNumber; counter++)
                {
                    //Check if the key exists
                    if (PlayerPrefs.HasKey(counter.ToString()))
                    {
                        //Create a dictionary
                        Dictionary<string, float> dict_ = new Dictionary<string, float>();
                        //Hold the string version of the counter
                        string counterString_ = counter.ToString();
                        //Hold the  "key" name of the string, which is equal to the counter in every iteration
                        string stringHolder = PlayerPrefs.GetString(counterString_);
                        //Convert the string content to the dictionary 
                        dict_ = ConvertStringToDict(stringHolder);
                        //If the object is sphere  instatiate a sphere prefab
                        if (dict_["Type"] == sphereType)
                        {
                           
                            //instantiate prefab instance 
                            MyGameObject = Instantiate(ARObjectsArray[sphereType]);
                            //Assign its name
                            MyGameObject.name = counterString_;
                            //set the image target as parent of prefab instance
                            MyGameObject.transform.parent = ImageTarget.transform;
                            //Set the local location values of the instance using the information being held in the dictionaries
                            MyGameObject.transform.localPosition = new Vector3(dict_["PosX"], dict_["PosY"], dict_["PosZ"]);
                            
                            //Set its rotation to default
                            MyGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            //Only render step 1
                            //If dict_["step_no"]!= 1
                            //Disable renderer
                            if (dict_["StepValue"] != 1f)
                            {
                                //Disable renderer and collider components of the object
                               
                                Renderer renderer = MyGameObject.GetComponent<MeshRenderer>();
                                Collider collider = MyGameObject.GetComponent<Collider>();
                                //If not in the first step disable its renderer and collider
                                renderer.enabled = false;
                                collider.enabled = false;
                            }


                            DistinctiveObjectData distinctiveObjectData_ = MyGameObject.GetComponent<DistinctiveObjectData>();
                            distinctiveObjectData_.id = counter;
                            //Assign its type value according to the dictionary
                            distinctiveObjectData_.type = dict_["Type"];

                            //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                            MyGameObject.layer = 9;
                            
                        }
                        else if (dict_["Type"] == arrowType)//Arrow object
                        {
                            
                            //instantiate prefab instance 
                            MyGameObject = Instantiate(ARObjectsArray[arrowType]);
                            //Assign its name
                            MyGameObject.name = counterString_;
                            
                            //set the image target as parent of prefab instance
                            MyGameObject.transform.parent = ImageTarget.transform;
                            //Set the local location values of the instance using the information being held in the dictionaries
                            MyGameObject.transform.localPosition = new Vector3(dict_["PosX"], dict_["PosY"], dict_["PosZ"]);
                            //Set its rotation to default
                            MyGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            //Only render step 1
                            //If dict_["step_no"]!= 1
                            //Disable renderer
                            if (dict_["StepValue"] != 1f)
                            {
                                //Disable renderer and collider components of the object
                                //Get the children of the arrow prefab
                                GameObject childArrowObject1 = MyGameObject.transform.GetChild(0).gameObject;
                                GameObject childArrowObject2 = MyGameObject.transform.GetChild(1).gameObject;
                                //Create references to the child object renderers and colliders
                                Renderer rendererChildObject1 = childArrowObject1.GetComponent<MeshRenderer>();
                                Renderer rendererChildObject2 = childArrowObject2.GetComponent<MeshRenderer>();
                                Collider collider = MyGameObject.GetComponent<Collider>();
                                //Disable renderers of the children and collider of the prefab
                                rendererChildObject1.enabled = false;
                                rendererChildObject2.enabled = false;
                                collider.enabled = false;
                            }


                            DistinctiveObjectData distinctiveObjectData_ = MyGameObject.GetComponent<DistinctiveObjectData>();
                            distinctiveObjectData_.id = counter;
                            //Assign its type value according to the dictionary
                            distinctiveObjectData_.type = dict_["Type"];
                            //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                            MyGameObject.layer = 9;




                        }
                        else if (dict_["Type"] == cubeType)//cube object
                        {
                            
                            //instantiate prefab instance 
                            MyGameObject = Instantiate(ARObjectsArray[cubeType]);
                            //Assign its name
                            MyGameObject.name = counterString_;
                            //set the image target as parent of prefab instance
                            MyGameObject.transform.parent = ImageTarget.transform;
                            //Set the local location values of the instance using the information being held in the dictionaries
                            MyGameObject.transform.localPosition = new Vector3(dict_["PosX"], dict_["PosY"], dict_["PosZ"]);
                            //Set its rotation to default
                            MyGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                            //Only render step 1
                            //If dict_["step_no"]!= 1
                            //Disable renderer
                            if (dict_["StepValue"] != 1f)
                            {
                                //Disable renderer and collider components of the object
                               
                                Renderer renderer = MyGameObject.GetComponent<Renderer>();
                                Collider collider = MyGameObject.GetComponent<Collider>();
                                renderer.enabled = false;
                                collider.enabled = false;
                            }


                            DistinctiveObjectData distinctiveObjectData_ = MyGameObject.GetComponent<DistinctiveObjectData>();
                            distinctiveObjectData_.id = counter;
                            distinctiveObjectData_.type = dict_["Type"];
                            //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                            MyGameObject.layer = 9;



                        }
                      
                        
                    }
                    else
                    {


                    }
                    
                        
                    
                    
                    


                    
                  

                 
                }

#if false
                //*******************************************************************************************************************************
                //restore distinctive number and the current step
                int numberofObjects = PlayerPrefs.GetInt("DistinctiveNumber");
                float currentStep = PlayerPrefs.GetFloat("Step");

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
                    //Convert counter to string
                    counterString = counter.ToString();
                    //Get the corresponding objects data
                    keyStringHolder = PlayerPrefs.GetString(counterString);
                    //Convert the data to dictionary type to access its step value
                    dict = ConvertStringToDict(keyStringHolder);

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
                        collider = go.GetComponent<Collider>();
                        renderer.enabled = false;
                        collider.enabled = false;
                    }
                }

#endif
            }
            else
            {
                Debug.Log("There is no object created click create object button to create an object");

            }


        }
    }


    protected virtual void OnTrackingLost()
    {
        if (mTrackableBehaviour)
        {
            var rendererComponents = mTrackableBehaviour.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = mTrackableBehaviour.GetComponentsInChildren<Collider>(true);
            var canvasComponents = mTrackableBehaviour.GetComponentsInChildren<Canvas>(true);

            // Disable rendering:
            foreach (var component in rendererComponents)
                component.enabled = false;

            // Disable colliders:
            foreach (var component in colliderComponents)
                component.enabled = false;

            // Disable canvas':
            foreach (var component in canvasComponents)
                component.enabled = false;
        }
    }

#endregion // PROTECTED_METHODS
}
