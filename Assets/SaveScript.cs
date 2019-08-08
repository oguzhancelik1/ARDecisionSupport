using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : SelectObject
{
    public string hit_instance;
    public static float x_crd;
    public Transform transform_to_save;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCoordinates()
    {
        GameObject gameObject = GameObject.Find(hit_instance_id);
        transform_to_save = gameObject.transform;


    }
}
