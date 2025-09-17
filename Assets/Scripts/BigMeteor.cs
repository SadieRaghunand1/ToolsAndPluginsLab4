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
        transform.Translate(Vector3.down * Time.deltaTime * 0.5f);

        if (transform.position.y < -11f || hitCount >= maxHitCount)
        {
            zoomState = zoomOut.ZOOMIN;
            StartCoroutine(ZoomCam());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if (whatIHit.tag == "Player")
        {
            manager.gameOver = true;
            Destroy(whatIHit.gameObject);
        }
        else if (whatIHit.tag == "Laser")
        {
            hitCount++;
            Destroy(whatIHit.gameObject);
        }
    }

    private IEnumerator ZoomCam()
    {
        yield return new WaitForEndOfFrame();

        if(zoomState == zoomOut.ZOOMOUT && cam.m_Lens.FieldOfView < maxZoom)
        {
            cam.m_Lens.FieldOfView += 0.5f;
            if(cam.m_Lens.FieldOfView >= maxZoom)
                    zoomState = zoomOut.STAY;
            StartCoroutine(ZoomCam());
        }
        else if(zoomState == zoomOut.ZOOMIN && cam.m_Lens.FieldOfView > minZoom)
        {
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
