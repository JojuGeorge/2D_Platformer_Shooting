using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    Transform fireingPoint;

    public float fireRate = 0;
    float timeToFire = 0;
    public int damage;

    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate;

    // Handle camera shaking
    public float camShakeAmt = 0.1f;
    public float camShakeLength;
    CameraShake camShake;

    void Awake()
    {
        fireingPoint = transform.Find("FireingPoint");   
    }

    private void Start()
    {
        camShake = GameManager.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("No cameraShake script found on gamemanager object");
    }

    void Update()
    {
        if (fireRate == 0) {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else {
            //Fire rate
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                Shoot();
                timeToFire = Time.time + 1 / fireRate;
            }
        }
    }

    void Shoot()
    {
        // Since screen-to-world point is different therefore we are getting x,y coordinates
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 fireingPointPosition = new Vector2(fireingPoint.position.x, fireingPoint.position.y);

        RaycastHit2D hit = Physics2D.Raycast(fireingPointPosition, mousePosition - fireingPointPosition, 100, whatToHit);
       // Debug.DrawLine(fireingPointPosition, mousePosition);

        if (hit.collider != null) {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                Debug.Log(hit.collider.name);
            }
            //    Debug.DrawLine(fireingPointPosition, hit.point, Color.red);
            //    Debug.Log(hit.collider.name);
        }

        // Setting spawn rate so that if even though fire rate is high this will limit the spawn rate.
        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            if (hit.collider == null)
                hitPos = (mousePosition - fireingPointPosition) * 30;
            else
                hitPos = hit.point;

            Effect(hitPos);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos) {
        Transform trail = Instantiate(bulletTrailPrefab, fireingPoint.position, fireingPoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null){
            lr.SetPosition(0, fireingPoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.3f);


        Transform flashClone = Instantiate(muzzleFlashPrefab, fireingPoint.position, fireingPoint.rotation) as Transform;

        flashClone.parent = fireingPoint;
        float size = Random.Range(0.4f, 0.6f);
        flashClone.localScale = new Vector3(size, size, size);
        Destroy(flashClone.gameObject, 0.2f);

        // Shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);
    }
}
