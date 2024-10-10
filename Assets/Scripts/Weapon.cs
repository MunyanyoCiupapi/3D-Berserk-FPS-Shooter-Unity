using UnityEngine;
using UnityEngine.Events;

//Mano interface 0.5t
public interface IWeapon
{
    void Shoot();
    void Reload();
    int GetAmmo();
}
public sealed class Weapon : MonoBehaviour
{

    public float distance;
    public GameObject holePrefab;
    private bool autofire = true;
    private float fireRate = 10;
    private float lastShootTime;
    public AudioClip shootSound;
    public AudioClip outOfAmmo;
    [Range(0f, 1f)]
    public float volume;//Range 0.5t




    public int maxAmmo = 90;
    public int ammo;
    public int clipSize = 30;
    public int clipAmmo;
    public float spreadAngle = 5;
    public int shellCount = 3;
    public Health health;
    public int damage = 3;

    private AudioSource source;

    public UnityEvent onShoot;
 



    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        if (ammo == 0) ammo = maxAmmo;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (autofire && Input.GetButton("Fire1"))
        {
            if (Time.time > lastShootTime + (1f / fireRate))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        lastShootTime = Time.time;

        if (clipAmmo > 0)
        {
            clipAmmo--;
        }
        else
        {
            if (ammo > 0)
            {
                Reload();
            }
        }
        if (clipAmmo <= 0 && ammo == 0)
        {
            source.volume = volume;
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(outOfAmmo);
            return;
        }
        //TIKRAI SAUDYS
        onShoot.Invoke();


        for (int i = 0; i < shellCount; i++)
        {
            ShootShell();
        }

        source.volume = volume;
        source.pitch = Random.Range(0.8f, 1.1f);
        source.PlayOneShot(shootSound);

    }

    private void ShootShell()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

 
        Physics.Raycast(ray, out var hit, distance);

        if (hit.collider == null) return;

        var obj = hit.collider.gameObject;

        if (obj.CompareTag("Enemy"))
        {
            var enemyHealth = obj.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(damage);
            }
        }
        else
        {
            var hole = Instantiate(holePrefab);
            hole.transform.position = hit.point + hit.normal * 0.01f;
            hole.transform.forward = hit.normal;
        }
    }



    void Reload()
    {
        clipAmmo = Mathf.Min(clipSize, ammo);
        ammo -= clipAmmo;
    }
}
