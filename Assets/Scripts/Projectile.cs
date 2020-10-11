using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float destroyAfter = 5f;
    public bool destoryOnImpact = false;
    public float minDestoryTime = 0.01f;
    public float maxDestoryTime = 0.05f;

    [Header("Impact Effects")] 
    public Transform[] impactPrefabs;

    private Rigidbody _rigidbody;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.drag = 0.65f;
        _rigidbody.angularDrag = 0.05f;

        StartCoroutine(DestroyAfter());
    }

    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(destroyAfter);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (destoryOnImpact)
            Destroy(gameObject);
        else
            StartCoroutine(DestroyTimer());

        //different impacts for different 'materials' using impactPrefabs later.
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(Random.Range(minDestoryTime, maxDestoryTime));
    }
}
