using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : InstantiationObject, ITrackableEventHandler
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

            string str = PlayerPrefs.GetString("Sphere");
            if (string.IsNullOrEmpty(str))
            {
                Debug.Log("There is no sphere object created");

            }
            else
            {
                Dictionary<string, float> dict = ConvertStringToDict(str);

                //Creation of instance of an array which will help construct the instances by holding the information of each instance's transform data
                IList myArryList1 = new ArrayList();

                //float[] f= new float[1000];

                int i = 0;
                //fill in all the values into an array to construct all the instances with correct data
                foreach (KeyValuePair<string, float> entry in dict)
                {

                    myArryList1.Add(entry.Value);
                    
                    
                }
                //Counter and iter are used so that the consecutive x,y,z values are extracted for all objects, to instantiate the objects.
                //Since objects have 3 data (x,y,z), iteration should continue from where it ended. The array values are extracted 3 by 3
                int arrayLength = myArryList1.Count;
                int counter = 0;
                do
                {
                    for (int iter = counter; iter <= counter ; iter++) 
                    {
                        float x = (float)myArryList1[iter];
                        float y = (float)myArryList1[iter+1];
                        float z = (float)myArryList1[iter+2];
                        //Instantiate a new GameObject
                        GameObject prefabInstance;
                        prefabInstance = Instantiate(ExistingPrefabSphere);

                        //Set the ImageTarget as a parent
                        prefabInstance.transform.parent = ImageTarget.transform;

                        prefabInstance.transform.localPosition = new Vector3(x,y,z);
                        //Set the rotation to Zeros
                        prefabInstance.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        //Set the layer for raycast masking
                        prefabInstance.layer = 9;
                        prefabInstance.name = prefabInstance.GetInstanceID().ToString();

                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosX", prefabInstance.transform.localPosition.x);
                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosY", prefabInstance.transform.localPosition.y);
                        FillSphereDictionary(sphereObjectsDictionary, prefabInstance.name + "PosZ", prefabInstance.transform.localPosition.z);
                        

                    }
                    //increment the counter by 3, to move on to the next object if there are any
                    counter = counter + 3;
                } while (counter+3<arrayLength);

                string_that_holds_sphere_info = ConvertDictToString(sphereObjectsDictionary, string_that_holds_sphere_info);
                Debug.Log(string_that_holds_sphere_info);
                PlayerPrefs.SetString("Sphere", string_that_holds_sphere_info);

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
