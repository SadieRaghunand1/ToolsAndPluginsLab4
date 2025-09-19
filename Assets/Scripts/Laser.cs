using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
 

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.Translate(Vector3.up * Time.deltaTime * 8f);

        if (transform.position.y > 11f)
        {
            Destroy(this.gameObject);
        }
    }
}
