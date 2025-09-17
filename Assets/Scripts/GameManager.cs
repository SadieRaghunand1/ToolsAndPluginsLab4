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

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        playerObj = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnMeteor", 1f, 2f);

        cam = FindAnyObjectByType<CinemachineVirtualCamera>();

        TrackPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            CancelInvoke();
        }

        reload = playerInput.Player.Reload.WasPressedThisFrame();
        if (reload && gameOver)
        {
            SceneManager.LoadScene("Week5Lab");
        }

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
        cam.Follow = playerObj.transform;
        cam.LookAt = playerObj.transform;
    }
}
