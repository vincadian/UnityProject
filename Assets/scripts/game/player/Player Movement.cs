using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables pour le mouvement
    [SerializeField]
    private float _speed;           // La vitesse de déplacement du joueur

    [SerializeField]
    private float _rotationSpeed;   // La vitesse de rotation du joueur

    [SerializeField]
    private float _screenBorder;

    // Variables pour la gestion du Rigidbody2D
    private Rigidbody2D _rigidbody;     
    private Vector2 _movementInput;     // Entrée de mouvement (touches ou joystick)
    private Vector2 _smoothedMovementInput; // Entrée de mouvement lissée pour un déplacement fluide
    private Vector2 _movementInputSmoothVelovity; // Vitesse de lissage du mouvement

    private Camera _camera;

    // Variables pour la gestion de la caméra et de la rotation vers la souris
    private Camera mainCam;        // Référence à la caméra principale
    private Vector3 mousePos;      // Position de la souris dans le monde 2D

    // Initialisation des composants
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // Récupère le Rigidbody2D attaché au joueur
        _camera = Camera.main;
    }

    // Mise à jour physique à chaque frame fixe
    private void FixedUpdate()
    {
        SetPlayerVelocity();        // Applique la vélocité lissée du joueur
        RotateInDirectionOfInput(); // Effectue la rotation du joueur vers l'entrée
    }

    // Méthode pour ajuster la vélocité du joueur (mouvement lissé)
    private void SetPlayerVelocity()
    {
        // Lissage du mouvement du joueur pour un déplacement plus fluide
        _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelovity, 0.1f);

        // Applique la vélocité calculée au Rigidbody2D
        _rigidbody.linearVelocity = _smoothedMovementInput * _speed;

        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
       Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _rigidbody.linearVelocity.x < 0) ||
            (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody.linearVelocity.x > 0))
        {
            _rigidbody.linearVelocity = new Vector2(0, _rigidbody.linearVelocity.y);
        }

        if ((screenPosition.y < _screenBorder && _rigidbody.linearVelocity.y < 0) ||
                (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody.linearVelocity.y > 0))
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0);
        }
    }

    // Méthode pour effectuer la rotation du joueur en fonction de l'entrée
    private void RotateInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero) // Si une entrée de mouvement est détectée
        {
            // Effectue la rotation en direction du mouvement (si applicable)
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            _rigidbody.MoveRotation(rotation);
        }
    }

    // Méthode pour gérer l'entrée de mouvement (via clavier ou joystick)
    private void OnMove(InputValue inputValue)
    {
        // Récupère l'entrée de mouvement (Vector2) et la stocke dans _movementInput
        _movementInput = inputValue.Get<Vector2>();
    }

    // Initialisation pour récupérer la caméra principale
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Mise à jour à chaque frame pour la rotation du joueur vers la souris
    void Update()
    {
        // Récupère la position de la souris dans le monde 2D
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Calculer la direction entre la position du joueur et la souris
        Vector3 direction = mousePos - transform.position;

        // Nous ajustons la position de la souris pour la coordonnée Z (car nous travaillons en 2D)
        direction.z = 0;

        // Calcule l'angle de rotation à appliquer (en radians) et le convertit en degrés
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Compense la rotation initiale du sprite (ajoutez 90° ou ajustez selon le pivot de votre sprite)
        rotZ -= 90f;  // Cette ligne ajuste la rotation de 90° pour corriger la direction initiale

        // Applique la rotation calculée à l'objet joueur
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}