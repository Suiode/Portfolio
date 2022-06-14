using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Rendering;


public class FingerGunScript : MonoBehaviour
{
    public int damage = 40;
    public int limbMultiplier = 1;
    public int headshotMultiplier = 2;
    public float rateOfFire = 0.25f;

    public float range;

    //Magazine stats
    public int magazineSize = 30;
    public int bulletsLoaded = 0;
    public float reloadSpeed = 1f;

    public int bulletsPerTriggerPull = 3;
    public bool readyForNextShot = true;



    //Other Components
    public Camera fpsCam;
    public Animator anim;
    //public LineRenderer bulletTravel;

    public TrailRenderer bulletTravelTrail;
    public LineRenderer bulletLineRenderer;


    public float bulletTravelTime = 0.1f;
    public GameObject muzzle;
    public GameObject bulletTrailResetPoint;
    public ParticleSystem muzzleFlash;
    public LayerMask shootableLayers;




    // Start is called before the first frame update
    void Start()
    {
        //RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }


    // Update is called once per frame
    void Update()
    {
        //When mouse button is clicked, check if we're ready to shoot and if we're ok, shoot
        if (Input.GetMouseButton(0) && readyForNextShot)
        {
            for (int i = 0; i < (bulletsPerTriggerPull); i++)
            {
                Shoot();

            }
        }

    }

    


    public void Shoot()
    {
        bulletLineRenderer.enabled = true;
        Vector3 directionRay = fpsCam.transform.TransformDirection(0, 0, 1);
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, directionRay, out hit, range, shootableLayers))
        {
            GhostFace target = hit.transform.GetComponentInParent<GhostFace>();
            Debug.Log(hit.point);

            if (target != null)
            {
                if (hit.collider.name == "Limbs")
                {
                    target.TakeDamage(damage * limbMultiplier);
                }
                else if (hit.collider.name == "Head")
                {
                    target.TakeDamage(damage * headshotMultiplier);
                }
                else
                    target.TakeDamage(damage);

                Debug.Log("We hit something");
            }

            /*if (hit.collider.tag == "Wall" || hit.collider.tag == "Ground")
            {
                //Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            }*/
        }







        if (hit.collider != null)
        {
           
            StartCoroutine(SpawnLineTrail(hit.point));
        }
        else
        {
            StartCoroutine(SpawnLineTrail(muzzle.transform.position + (fpsCam.transform.forward + directionRay) * range));
        }



        muzzleFlash.Play();

        readyForNextShot = false;
        anim.SetBool("Firing", true);

        StartCoroutine(RateOfFire());
        
    }

    IEnumerator RateOfFire()
    {
        //anim.SetBool("Firing", false);
        yield return new WaitForSeconds(rateOfFire);
        readyForNextShot = true;
        
    }



    public IEnumerator SpawnLineTrail(Vector3 hitPoint)
    {

        //Debug.Log("Starting the bullet trail");
        float time = 0;
        Vector3 startPosition = muzzle.transform.position;


        int currentPositions = bulletLineRenderer.positionCount;
        bulletLineRenderer.positionCount = currentPositions + 2;



        while (time < 1)
        {
            bulletLineRenderer.SetPosition(currentPositions, muzzle.transform.position);
            bulletLineRenderer.SetPosition(currentPositions+ 1, Vector3.Lerp(startPosition, hitPoint, time));
            time += Time.deltaTime / bulletTravelTime;

            //Debug.Log("Balls deep in the bullet trail");
            yield return null;

        }

        
        //bulletLineRenderer.SetPosition(currentPositions + 1, hitPoint);
        //yield return new WaitForSeconds(0.1f);

        bulletLineRenderer.positionCount = currentPositions;
        bulletLineRenderer.enabled = false;
        anim.SetBool("Firing", false);
    }


    /*private void OnEnable()
    {
        Application.onBeforeRender += Shoot;
    }

    private void OnDisable()
    {
        Application.onBeforeRender -= Shoot;
    }*/
}
