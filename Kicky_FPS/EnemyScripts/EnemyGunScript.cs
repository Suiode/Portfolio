using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGunScript : MonoBehaviour
{
	//Keeping these public for tools to interact with these variables. e.g. hacking to jam guns, grenades that daze enemies so they're less accurate
    private bool enemyAttack;
    public float recoilSideToSide;
    public float recoilUp;
    public float recoilDown;
    public float resetRecoilSpeed;
    public float fireRate;
    public float reloadSpeed = 2f;
    public int magazineSize;
    private int bulletsLoaded = 30;
    public float enemyBulletDamage = 10f;
    public float range = 100f;
    private bool isReloading = false;
    public float headshotMultiplier = 4;
    public float limbMultiplier = 0.5f;

    private bool allowFullAuto;
    public bool readyForNextShot;
    private float bulletsPerTriggerPull = 1;

    [SerializeField] private ParticleSystem enemyMuzzleFlash;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private LineRenderer enemyBulletTravel;
    [SerializeField] private GameObject enemyMuzzle;
    //public Animator enemyAnim;







    // Start is called before the first frame update
    void Start()
    {
        readyForNextShot = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset the line renderer so it disappears after the shot
        enemyBulletTravel.SetPosition(0, enemyMuzzle.transform.position);
        enemyBulletTravel.SetPosition(1, enemyMuzzle.transform.position);


        // Shoot once or multiple times depending on fire mode
        if (allowFullAuto && !PauseScript.gameIsPaused)
        {
            if (enemyAttack && readyForNextShot && bulletsLoaded > 0 && !isReloading)
            {
                for (int i = 0; i < (bulletsPerTriggerPull); i++)
                    Shoot();
            }
        }
        else if (enemyAttack && readyForNextShot && bulletsLoaded > 0 && !isReloading && !PauseScript.gameIsPaused)
        {
            for (int i = 0; i < (bulletsPerTriggerPull); i++)
                Shoot();
        }

        // Reload
        if (bulletsLoaded <= 0 && !isReloading)
        {
            bulletsLoaded = 0;
            readyForNextShot = false;
            isReloading = true;
            Reload();
        }

    }



    private void Shoot()
    {
		//Recoil works, but is too random. No first shot accuracy atm
        float ySpread = Random.Range(0, recoilUp);
        float xSpread = Random.Range(-recoilSideToSide, recoilSideToSide);


        Vector3 directionRay = enemyMuzzle.transform.TransformDirection(xSpread, ySpread, 1);
        RaycastHit hit;


        if (Physics.Raycast(enemyMuzzle.transform.position, directionRay, out hit, range))
        {
            PlayerController target = hit.transform.GetComponent<PlayerController>();
            if (target != null)
            {
                if (hit.transform.name == "Limbs")
                {
                    target.TakeDamage(enemyBulletDamage * limbMultiplier);
                }
                else if (hit.transform.name == "Head")
                {
                    target.TakeDamage(enemyBulletDamage * headshotMultiplier);
                }
                else
                    target.TakeDamage(enemyBulletDamage);


            }
            if (hit.collider.tag == "Ground")
            {
                Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        if (hit.collider != null)
        { enemyBulletTravel.SetPosition(1, hit.point); }
        else
        enemyBulletTravel.SetPosition(1, (enemyMuzzle.transform.position + (transform.forward + directionRay) * range));

        bulletsLoaded--;
        readyForNextShot = false;
        enemyMuzzleFlash.Play();
        StartCoroutine(RateOfFire());
    }


    IEnumerator RateOfFire()
    {
        yield return new WaitForSeconds(fireRate);
        readyForNextShot = true;
    }

    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(reloadSpeed);
        isReloading = false;
        bulletsLoaded = magazineSize;
        readyForNextShot = true;
    }

    private void Reload()
    {
        StartCoroutine(ReloadTimer());
    }

}
