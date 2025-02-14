using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private float _bulletSpeed;

    [SerializeField]
    private Transform _gunOffset;

    [SerializeField]
    private float _timeBetweenShots;

    [SerializeField]
    private float _reloadTime;  // Temps de rechargement après 10 tirs

    private float _lastFireTime;
    private int _shotsFired = 0;  // Compteur de tirs
    private bool _isReloading = false;  // Indique si l'arme est en train de se recharger

    void Update()
    {
        if (_isReloading)
        {
            // Si l'arme est en train de se recharger, attendre que le temps de recharge passe
            return;
        }

        // Vérifie si suffisamment de temps est passé depuis le dernier tir
        float timeSinceLastFire = Time.time - _lastFireTime;

        if (timeSinceLastFire >= _timeBetweenShots)
        {
            FireBullet();
            _lastFireTime = Time.time;

            _shotsFired++;  // Incrémente le compteur de tirs

            // Si 10 tirs ont été effectués, recharge l'arme
            if (_shotsFired >= 10)
            {
                StartCoroutine(ReloadWeapon());
            }
        }
    }

    private void FireBullet()
    {
        // Instancie une balle à la position du canon et dans la direction actuelle
        GameObject bullet = Instantiate(_bulletPrefab, _gunOffset.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();

        // Donne une vélocité à la balle selon la direction du joueur
        rigidbody.linearVelocity = _bulletSpeed * transform.up;
    }

    private IEnumerator ReloadWeapon()
    {
        _isReloading = true;  // L'arme est en train de se recharger

        // Attendre pendant le temps de rechargement
        yield return new WaitForSeconds(_reloadTime);

        _shotsFired = 0;  // Réinitialise le compteur de tirs après le rechargement
        _isReloading = false;  // Fin du rechargement
    }
}