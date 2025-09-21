using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameManager manager;
    public Transform player;
    //Base speed, which will change in start and the radius to keep
    private float orbitSpeed = 2f;
    private float orbitRadius;


    //Players angle
    private float angle;

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }

        orbitRadius = Random.Range(2f, 5f);

        //Scale orbit speed based on distance, as requirement 
        orbitSpeed = orbitSpeed + orbitRadius * 0.2f;

        //Initialize angle based on starting position
        Vector3 offset = transform.position - player.position;
        angle = Mathf.Atan2(offset.y, offset.x);
    }

    void Update()
    {
        Vector3 center;

        if (player != null)
        {
            center = player.position;
        }
        else
        {
            center = Vector3.zero;
        }

        //Advance angle over time
        angle += orbitSpeed * Time.deltaTime;

        //Offset for orbit
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * orbitRadius;

        //Move enemy around the center
        transform.position = center + offset;

        //Point at the center
        PointToPlayer(center);
    }

    private void PointToPlayer(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private void OnTriggerEnter2D(Collider2D whatIHit)
    {
        //If the laser hits the meteor, destroy the meteor and increase meteor kill count
        if (whatIHit.tag == "Laser")
        {
            Destroy(whatIHit.gameObject);
            Destroy(this.gameObject);
        }
    }

}
