using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Meteor : MonoBehaviour
{
    GameManager manager;
    private CinemachineVirtualCamera cam;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseProfile;
    private float secOfShake = 1f;
    private float newFrequency = 5f;
    private float newAmp = 5f;

    //Create a unity event to continue camera shake after this object is destroyed
    public delegate void Event_OnMeteorHit();
    public static event Event_OnMeteorHit meteorHit;

    private void Start()
    {
        manager = FindAnyObjectByType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        transform.Translate(Vector3.down * Time.deltaTime * 2f);

        if (transform.position.y < -11f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        //If the meteor hits the player
        if (whatIHit.tag == "Player")
        {
            //End game and destroy meteor/player
            manager.gameOver = true;
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        } 
        //If the laser hits the meteor, destroy the meteor and increase meteor kill count
        else if (whatIHit.tag == "Laser")
        {
            manager.meteorCount++;
            meteorHit.Invoke();
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }

}
