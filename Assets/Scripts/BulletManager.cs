using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using Player;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> _bulletPools;
    public static BulletManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        StartCoroutine(BulletPooling(50));
    }
    private IEnumerator BulletPooling(int count)
    {
        for (int i = count; i > 0; i--)
        {
            GameObject bullet = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
            _bulletPools.Add(bullet);
            bullet.SetActive(false);
        }
        yield break;
    }
    public void Shoot(Vector3 direction)
    {
        Debug.Log(direction);
        Transform spawnPoint = FindObjectOfType<PlayerController>().transform;
        GameObject lastBullet = _bulletPools.Last();
        lastBullet.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y+0.5f, spawnPoint.position.z);
        lastBullet.GetComponent<BulletTransform>().direction = direction;
        lastBullet.GetComponent<TrailRenderer>().enabled = true;
        _bulletPools.Remove(_bulletPools.Last());
        lastBullet.SetActive(true);
    }

    public void ResetBullet(GameObject bullet)
    {
        bullet.GetComponent<TrailRenderer>().enabled = false;
        _bulletPools.Add(bullet);
        bullet.SetActive(false);
    }
}