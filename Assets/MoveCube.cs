using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{

   /* public int x;
    public int y;
    public int z;
   */
    //GameObject Arrow = GameObject.Find("Cube");
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 4, 3);
    }
}
