using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GunScript : MonoBehaviour
{
    //Recoil numbers
    public float recoilSideToSide;
    public float recoilUp;
    public float recoilDown;
    public float xDefaultRecoil = 0.001f;
    public float yDefaultRecoil = 0.001f;
    public float resetRecoilSpeed = 0.25f;
    public float headshotMultiplier = 4;
    public float limbMultiplier = 0.5f;



    public float fireRate;


    public float reloadSpeed = 2f;
    public float magazineSize;
    [SerializeField] private float bulletsLoaded = 30f;
    [SerializeField] private float bulletDamage = 10f;
    public float range = 100f;
    [SerializeField] private bool isReloading = false;


    [SerializeField] private bool allowFullAuto;
    [SerializeField] private bool readyForNextShot;
    [SerializeField] private float bulletsPerTriggerPull = 1;

    
    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshPro ammoCounterFloating;
    [SerializeField] private TextMeshPro floatingFiringMode;
    [SerializeField] private Text weaponType;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private LineRenderer bulletTravel;
    [SerializeField] private GameObject muzzle;
    public Misc_options cheat_options;


    // Recoil test
    public bool recoilOn;





    // Start is called before the first frame update
    void Start()
    {
        readyForNextShot = true;
        ammoCounterFloating.text = (bulletsLoaded + "/" + magazineSize);


        SetFiringMode();

    }

    // Update is called once per frame
    void Update()
    {
        bulletTravel.SetPosition(1, muzzle.transform.position);
        //ResetGun();

        // Firing mode
        if (allowFullAuto && !PauseScript.gameIsPaused)
        {
            if (Input.GetMouseButton(0) && readyForNextShot && bulletsLoaded > 0 && !isReloading)
            {
                for (int i = 0; i < (bulletsPerTriggerPull); i++)
                {
                    Shoot();
                    if (!cheat_options.laserGun)
                    {
                        Recoil();
                    }
                }

            }
        }
        else if (Input.GetMouseButtonDown(0) && readyForNextShot && bulletsLoaded > 0 && !isReloading && !PauseScript.gameIsPaused)
        {
            for (int i = 0; i < (bulletsPerTriggerPull); i++)
            {
                Shoot();
                Recoil();
            }
        }

        // Reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            bulletsLoaded = 0f;
            readyForNextShot = false;
            isReloading = true;
            Reload();
        }

        // Change fire mode
        if (Input.GetKeyDown(KeyCode.B))
        {
            allowFullAuto = !allowFullAuto;
            SetFiringMode();
        }

        if (recoilUp < 0 && readyForNextShot)
        {
            StartCoroutine(ResetGunHeat());
            recoilUp = 0;
            recoilSideToSide = 0;
        }


    }

    private void LateUpdate()
    {
        ResetGun();
    }








    private void Shoot()
    {
        float ySpread = Random.Range(recoilUp, recoilUp);
        float xSpread = Random.Range(-recoilSideToSide, recoilSideToSide);


        Vector3 directionRay = fpsCam.transform.TransformDirection(xSpread, ySpread, 1);
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, directionRay, out hit, range))
        {
            Enemy target = hit.transform.GetComponentInParent<Enemy>();
            
            if (target != null)
            {
                if (hit.collider.name == "Limbs")
                {
                    target.TakeDamage(bulletDamage * limbMultiplier);
                }
                else if (hit.collider.name == "Head")
                {
                    target.TakeDamage(bulletDamage * headshotMultiplier);
                }
                else 
                    target.TakeDamage(bulletDamage);
            }

            if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground")
            {
                Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        if (hit.collider != null)
        {
            bulletTravel.SetPosition(1, hit.point);
        }
        else
        {
            bulletTravel.SetPosition(1, (muzzle.transform.position + (fpsCam.transform.forward + directionRay) * range));
        }

        if (!cheat_options.infiniteAmmo)
        {
            bulletsLoaded--;
        }


        if (!cheat_options.laserGun)
        {
            readyForNextShot = false;
            anim.SetBool("Firing", true) ;
        }
        else
        {
            readyForNextShot = true;
            anim.SetBool("Firing", false);
        }


        ammoCounterFloating.text = (bulletsLoaded + "/" + magazineSize);
        StartCoroutine(RateOfFire());
    }


    IEnumerator RateOfFire()
    {
        if (!cheat_options.laserGun)
        {
            yield return new WaitForSeconds(fireRate);
            readyForNextShot = true;
        }
        else { 
        yield return new WaitForSeconds(fireRate);
        readyForNextShot = true;
        }
    }


    IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(reloadSpeed);
        isReloading = false;
        bulletsLoaded = magazineSize;
        ammoCounterFloating.text = (bulletsLoaded + "/" + magazineSize);
        readyForNextShot = true;
    }

    private void Reload()
    {
        anim.SetBool("ReloadAnimationBool", true);
        StartCoroutine(ReloadTimer());
    }

    private void SetFiringMode()
    {
        if (allowFullAuto == true)
        {
            floatingFiringMode.text = ("PEW PEW PEW");
        }
        else
        {
            floatingFiringMode.text = ("PEW");

        }
    }

    private void ResetGun()
    {
        bulletTravel.SetPosition(0, muzzle.transform.position);

        // Disable animations when they're not running
        anim.SetBool("ReloadAnimationBool", false);
        anim.SetBool("Firing", false);

        if (recoilUp > 0)
        {
            recoilUp -= recoilDown * Time.deltaTime;
            //Debug.Log("Recoil Up: " + recoilUp);
            
        }
        if (recoilSideToSide > 0)
        {
            recoilSideToSide -= recoilDown * Time.deltaTime;
            //Debug.Log("Recoil Side: " + recoilSideToSide);
        }

    }

   


    public void Recoil()
    {
        if(recoilUp < 0.15)
        {
            recoilUp += yDefaultRecoil;
        }

        if (recoilSideToSide < 0.30)
        {
            recoilSideToSide += xDefaultRecoil;
        }

        if (recoilSideToSide >= 0.30)
        {
            xDefaultRecoil *= -1;
        }
        else if(recoilSideToSide >= -0.30)
        {
            xDefaultRecoil *= -1;
        }

        //recoilOn = false;
    }

    IEnumerator ResetGunHeat()
    {
        yield return new WaitForSeconds(resetRecoilSpeed);

        if (recoilUp < 0 && readyForNextShot)
        {
            recoilUp = 0;
            recoilSideToSide = 0;
        }
    }

}
