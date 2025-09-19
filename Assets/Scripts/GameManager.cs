using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject meteorPrefab;
    public GameObject bigMeteorPrefab;
    public bool gameOver = false;

    public int meteorCount = 0;

    private PlayerInput playerInput;
    bool reload;

    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera cam;
    private GameObject playerObj;
    private CinemachineBasicMultiChannelPerlin noiseProfile;
    private float secOfShake = 0.5f;
    private float newFrequency = 5f;
    private float newAmp = 5f;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();

        //Get camera variables
        noiseProfile = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //Add listener for event for on enemy killed to shake the camera
        Meteor.meteorHit += Callback_OnMeteorHitWrapper;
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        //Instantiate player and start spawning  meteors
        playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnMeteor", 1f, 2f);

        //Find camera
        cam = FindAnyObjectByType<CinemachineVirtualCamera>();

        //Assign Follow and LookAt on camera
        TrackPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the game is over, stop spawning meteors
        if (gameOver)
        {
            CancelInvoke();
        }

        //Check if the player is reloading the game, reload game if input
        reload = playerInput.Player.Reload.WasPressedThisFrame();
        if (reload && gameOver)
        {
            SceneManager.LoadScene("Week5Lab");
        }

        //If there have been 5 normal meteors spawned, then spawn a big meteor
        if (meteorCount == 5)
        {
            BigMeteor();
        }
    }

    void SpawnMeteor()
    {
        Instantiate(meteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void BigMeteor()
    {
        meteorCount = 0;
        Instantiate(bigMeteorPrefab, new Vector3(Random.Range(-8, 8), 7.5f, 0), Quaternion.identity);
    }

    void TrackPlayer()
    {
        //Assign Cinemachine camera's follow and look at properties to the player
        cam.Follow = playerObj.transform;
        cam.LookAt = playerObj.transform;
    }

    //Wrapper for coroutine
    void Callback_OnMeteorHitWrapper()
    {
        StartCoroutine(OnCameraShakeEnumerator());
    }

    private IEnumerator OnCameraShakeEnumerator()
    {
        //Change noise filter on camera to shake
        noiseProfile.m_FrequencyGain = newFrequency;
        noiseProfile.m_AmplitudeGain = newAmp;

        //After time passes, retturn to normal
        yield return new WaitForSeconds(secOfShake);
        noiseProfile.m_FrequencyGain = 0;
        noiseProfile.m_AmplitudeGain = 0;
    }
}
