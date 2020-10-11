using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int ammo = 7;
    public float recoil = 5f;
    public float bulletForce = 400f;
    public GameObject bulletPrefab;
    public GameObject casingPrefab;

    [Header("Effect Settings")] 
    public bool muzzleFlash;

    [Header("Spawn Points")]
    public Transform bulletSpawnPoint;
    public Transform casingSpawnPoint;

    [Header("Sound Clips")] 
    public AudioSource shootSound;

    private Rigidbody _rigidbody;
    private XRGrabInteractable _grabInteractable;

    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _grabInteractable.onActivate.AddListener(Shoot);
    }

    private void OnDisable()
    {
        _grabInteractable.onDeactivate.RemoveListener(Shoot);
    }

    private void Shoot(XRBaseInteractor baseInteractor)
    {
        shootSound.Play();
        CreateProjectile();
        //CreateCasing();
        AddRecoil();
    }

    private void CreateCasing()
    {
        
    }

    private void AddRecoil()
    {
        _rigidbody.AddRelativeForce(Vector3.up * recoil, ForceMode.Impulse);
        _rigidbody.AddRelativeForce(Vector3.back * recoil, ForceMode.Impulse);
    }

    private void CreateProjectile()
    {
        GameObject projectileGameObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        projectileGameObject.GetComponent<Rigidbody>().velocity = projectileGameObject.transform.forward * bulletForce;
    }
}
