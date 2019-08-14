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
            
            if (PlayerPrefs.HasKey("DistinctiveNumber"))
            {
                int dinstinctiveNumber = PlayerPrefs.GetInt("DistinctiveNumber");
                GameObject gameObject;
                for (int counter = 1; counter <= dinstinctiveNumber; counter++)
                {
                    Dictionary<string, float> dict = new Dictionary<string, float>();
                    string counterString = counter.ToString();
                    string stringHolder = PlayerPrefs.GetString(counterString);
                    dict = ConvertStringToDict(stringHolder);
                    //instantiate prefab instance 
                    gameObject = Instantiate(ExistingPrefabSphere);
                    //set the image target as parent of prefab instance
                    gameObject.transform.parent = ImageTarget.transform;
                    //Set the local location values of the instance using the information being held in the dictionaries
                    gameObject.transform.localPosition = new Vector3(dict["PosX"] , dict["PosY"], dict["PosZ"]);
                    //Set its rotation to default
                    gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);


                    DistinctiveSphereData distinctiveSphereData_ = gameObject.GetComponent<DistinctiveSphereData>();
                    distinctiveSphereData_.id = counter;
                    //set every prefab instance in layer 9 to make sure that they are the only collidable objects in the scene when raycasting 
                    gameObject.layer = 9;
                    //Assign instance tag the distinctive number
                    
                    
                }
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
