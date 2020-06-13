using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 1f;
    public bool invertBullet = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            Fire();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Fire!!");
            Fire();
        }
    }

    void Fire()
    {
        Quaternion bulletRot;
        if (invertBullet)
        {
            bulletRot = firePoint.rotation * Quaternion.Euler(0,0,180);
        } else
        {
            bulletRot = firePoint.rotation;
        }


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRot);


        Vector3 bulletVel = new Vector3(0, 0, bulletSpeed);
        bulletVel = bullet.transform.rotation * bulletVel;
        bullet.GetComponent<Rigidbody>().velocity = bulletVel;
        Destroy(bullet, 10f);
    }

}
