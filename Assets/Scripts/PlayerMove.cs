using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController _charController;

    // EmptyObject aidant a la detection du sol
    GameObject _groundChecker;
    [SerializeField]
    float _groundDistance = 0.2f;

    // A remplir avec le "Ground" Layer dans l'editor
    [SerializeField]
    LayerMask _groundLayer;

    [SerializeField]
    float _playerSpeed = 5f;
    [SerializeField]
    float _jumpHeight = 3f;

    // La vitesse en Y sera variable (donc présence d'accélération, ici c'est la gravité)
    // Il faut garder où on en est d'un frame à l'autre
    Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _velocity = Vector3.zero;

        _groundChecker = GameObject.Find("DetecteurDeSol");
    }

    // Update is called once per frame
    void Update()
    {
        // Gerer mouvement X-Z (au sol)
        Vector3 _playerInput = Vector3.zero;
        _playerInput.x = Input.GetAxis("Horizontal");
        _playerInput.z = Input.GetAxis("Vertical");

        if (_playerInput != Vector3.zero)
        {
            _charController.transform.forward = _playerInput;

            // Eviter le mouvement plus rapide en diagonale
            if (_playerInput.magnitude > 1f)
                _playerInput.Normalize();

            _charController.Move(_playerInput * _playerSpeed * Time.deltaTime);
        }

        // Gestion du mouvement en hauteur (aka Jump)

        bool isGrounded = Physics.CheckSphere(_groundChecker.transform.position, _groundDistance, _groundLayer, QueryTriggerInteraction.Ignore);

        // Appliquer la gravité seulement si on ne touche pas au sol
        if (!isGrounded)
        {
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // On doit reset la velocity si on touche au sol
            _velocity.y = 0f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y += Mathf.Sqrt(_jumpHeight * -2 * Physics.gravity.y);
        }

        _charController.Move(_velocity * Time.deltaTime);
    }
}
