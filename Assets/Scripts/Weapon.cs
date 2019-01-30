using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0f; // 0 = single fire
    public float effectSpawnRate = 10f;
    public float damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;

    float TimeToSpawnEffect = 0f;
    float timeToFire = 0f;
    Transform firepoint;

    void Awake()
    {
        firepoint = transform.Find("FirePoint");
        if (firepoint == null) {
            Debug.LogError("No firepoint attached to the weapon !");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot() {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firepoint.position.x, firepoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        if (Time.time >= TimeToSpawnEffect) {
            Effect();
            TimeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null) {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage." );
        }
    }

    void Effect() {
        Instantiate(BulletTrailPrefab, firepoint.position, firepoint.rotation);
    }
}
