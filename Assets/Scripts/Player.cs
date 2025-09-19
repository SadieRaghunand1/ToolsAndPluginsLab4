using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject laserPrefab;

    private float speed = 6f;
    private bool canShoot = true;

    private PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();   
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    void Movement()
    {
        //Use player's input on arrows or WASD to move player
        Vector2 playerInputVal = playerInput.Player.Move.ReadValue<Vector2>();
        transform.Translate((new Vector3(playerInputVal.x, playerInputVal.y, 0)) * speed * Time.deltaTime);

       
    }

    void Shooting()
    {
        //Check if shot this frame
        bool input = playerInput.Player.Shooting.WasPressedThisFrame();
        if (input && canShoot)
        {
            //Instantiate laser
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            canShoot = false;
            StartCoroutine("Cooldown");
        }
    }

    //Prevent multiple shots in one frame
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1f);
        canShoot = true;
    }
}
