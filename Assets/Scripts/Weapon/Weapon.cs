using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int ammo = 7;
    public float recoil = 1f;
    public float bulletForce = 400f;
    public Projectile bulletPrefab;
    public GameObject casingPrefab;

    [Header("Spawn Points")]
    public Transform bulletSpawnPoint;
    public Transform casingSpawnPoint;

    [Header("Sound Clips")]
    public AudioSource mainAudioSource;
    public AudioClip shootSound;

    [Header("Controls")]
    public XRNode inputSource;

    private int _currentAmmo;
    private bool _outOfAmmo;
    private Rigidbody _rigidbody;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentAmmo = ammo;
    }

    private void OnEnable()
    {
        _grabInteractable.onActivate.AddListener(Shoot);
    }

    private void OnDisable()
    {
        _grabInteractable.onDeactivate.RemoveListener(Shoot);
    }

    private void Update()
    {
        if (_currentAmmo == 0)
            _outOfAmmo = true;

        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primaryButton, out bool clicked);

        if (clicked)
        {
            _currentAmmo = 7;
            _outOfAmmo = false;
        }
    }

    private void Shoot(XRBaseInteractor baseInteractor)
    {
        if (!_outOfAmmo)
        {
            _currentAmmo -= 1;
            PlayAudio();
            SpawnProjectile();
            //SpawnCasing();
            AddRecoil();
        }
    }

    private void Reload()
    {
        _currentAmmo = ammo;
    }

    private void PlayAudio()
    {
        if (mainAudioSource.clip != shootSound)
            mainAudioSource.clip = shootSound;
        mainAudioSource.Play();
    }

    private void AddRecoil()
    {
        _rigidbody.AddRelativeForce(Vector3.up * recoil, ForceMode.Impulse);
        _rigidbody.AddRelativeForce(Vector3.back * recoil, ForceMode.Impulse);
    }

    private void SpawnProjectile()
    {
        Projectile projectileGameObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        projectileGameObject.GetComponent<Rigidbody>().velocity = projectileGameObject.transform.forward * bulletForce;
    }
}
