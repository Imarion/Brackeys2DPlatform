using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0f; // 0 = single fire
    public float effectSpawnRate = 10f;
    public int damage = 10;
    public LayerMask whatToHit;

    // Handle camera shaking
    public float camShakeAmt = 0.1f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;

    public Transform BulletTrailPrefab;
    public Transform hitPrefab;
    public Transform MuzzleFlashPrefab;

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
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null) {
            Debug.LogError("No camera CameraShake found on gm object.");
        }
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

        //Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null) {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage." );
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.DamageEnemy(damage);
            }
        }

        if (Time.time >= TimeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            TimeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPosition, Vector3 hitNormal) {
        Transform trail = Instantiate(BulletTrailPrefab, firepoint.position, firepoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null) {
            lr.SetPosition(0, firepoint.position);
            lr.SetPosition(1, hitPosition);
        }
        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(9999, 9999, 9999)) {
            Transform hitParticles = Instantiate(hitPrefab, hitPosition, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticles.gameObject, 0.1f);
        }

        Transform clone = Instantiate(MuzzleFlashPrefab, firepoint.position, firepoint.rotation) as Transform;
        clone.parent = firepoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        //Shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);
    }
}
