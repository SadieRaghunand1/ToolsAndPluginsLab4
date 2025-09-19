using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BigMeteor : MonoBehaviour
{
    public int hitCount = 0;
    GameManager manager;
     int maxHitCount = 5;

    CinemachineVirtualCamera cam;
    float maxZoom = 150f;
    float minZoom = 125f;
    enum zoomOut
    {
        ZOOMIN,
        STAY,
        ZOOMOUT
    };
    zoomOut zoomState = zoomOut.ZOOMOUT;


    // Start is called before the first frame update
    void Start()
    {
        manager = FindAnyObjectByType<GameManager>();

        //Zoom out when instantiated
        cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        StartCoroutine(ZoomCam());
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.Translate(Vector3.down * Time.deltaTime * 0.5f);

        //Check if the meteor is on screen
        if (transform.position.y < -11f || hitCount >= maxHitCount)
        {
            zoomState = zoomOut.ZOOMIN;
            StartCoroutine(ZoomCam());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        //If meteor hits player
        if (whatIHit.tag == "Player")
        {
            //End game immediately, destroy this
            manager.gameOver = true;
            Destroy(whatIHit.gameObject);
        }
        //If meteor is hit by laser
        else if (whatIHit.tag == "Laser")
        {
            //Increase hit count and destroy the laser
            hitCount++;
            Destroy(whatIHit.gameObject);
        }
    }

    //Control zoom of camera when the big meteor is instantiated, allows smooth zoom in/out
    private IEnumerator ZoomCam()
    {
        yield return new WaitForEndOfFrame();

        //Check if zooming in or out
        if(zoomState == zoomOut.ZOOMOUT && cam.m_Lens.FieldOfView < maxZoom)
        {
            //Zoom over several frames
            cam.m_Lens.FieldOfView += 0.5f;
            if(cam.m_Lens.FieldOfView >= maxZoom)
                    zoomState = zoomOut.STAY;
            StartCoroutine(ZoomCam());
        }
        else if(zoomState == zoomOut.ZOOMIN && cam.m_Lens.FieldOfView > minZoom)
        {
            //Zoom over several frames
            Debug.Log("Zoom in");
            cam.m_Lens.FieldOfView -= 0.5f;
            if (zoomState == zoomOut.ZOOMIN && cam.m_Lens.FieldOfView <= minZoom)
            {
                Destroy(this.gameObject);
            }
            StartCoroutine(ZoomCam());
        }
        

        
    }
}
