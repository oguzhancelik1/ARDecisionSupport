using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStuff : MonoBehaviour
{
    public ActorData data = new ActorData();
     InstantiationObject instantiationObject = new InstantiationObject();

    public string name;
    public Vector3 pos;
    public Transform transform;



    public void StoreData()
    {
        try
        {
            name = instantiationObject.InstanceIdGetter();
        }
        catch(Exception ex)
        {

            Debug.Log("instance id :" + instantiationObject.InstanceIdGetter());
        }
       
        GameObject go = GameObject.Find(name);
        Transform tr;
        go.transform.parent = instantiationObject.SetTransform();
        Vector3 vector3 = instantiationObject.SetPosition();
        go.transform.localPosition = vector3;
        tr = go.transform;
        
       /* GameObject go = GameObject.Find("ImageTarget");
        Transform tr = go.transform;*/
      

        data.name = name;
        data.transform = tr;
        //data.pos = vector3;
        


    }



    // FindGameObject is supposed to find the instance on the scene after creationthat pass this knowledge to somewhere

    /*public GameObject FindGameObject(string instance_id)
    {
        GameObject go = GameObject.Find(instance_id);
        return go;
    }
    //assign name using found instance on the scene
    public string AssignName(GameObject go)
    {
        name = go.name;

        return name;
    }
    public Vector3 AssignPosition(Vector3 vector)
    {
        pos = vector;

        return pos;
    }*/
    //assign 
    

    public void LoadData()
    {
    name = data.name;
    transform = data.transform;
        // transform.localPosition = data.pos;



    }

    public void ApplyData()
    {

    SaveData.AddActorData(data);

    }

    private void OnEnable()
    {   
    SaveData.OnLoaded += LoadData;
    SaveData.OnBeforeSave += StoreData;
    SaveData.OnBeforeSave += ApplyData;
    }

    private void OnDisable()
    {
    SaveData.OnLoaded -= LoadData;
    SaveData.OnBeforeSave -= StoreData;
    SaveData.OnBeforeSave -= ApplyData;
    }

}


[Serializable]
public class ActorData
{
    public string name;
    //public Vector3 pos;
    
    public Transform transform;
   
}


