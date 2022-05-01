using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class BallRotation : MonoBehaviour
{

    private Vector2 _moveInput;
    private Vector3 _movementInput;
    private Rigidbody _rb;
    public bool _ballMode;

    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _gravity;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _rollAcceleration;
    [SerializeField] private float _torqueMultiplier;
    [SerializeField] private float _interactionRadius =3f;
    private bool _groundedPlayer;
    public GameObject _ball;
    [SerializeField] private VisualEffect _enterBallEffect;
    [SerializeField] private string _enterBallEventName;
    [SerializeField] private float _jumpHeight;
    private float _turnSpeed =10f;
    public int pid;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(-_moveInput.x, 0f, -_moveInput.y).normalized;
        _movementInput = moveInput;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        
    }

    private void FixedUpdate()
    {
        if (_ballMode)
        {
            BallTorque();
            transform.position = _ball.transform.position;
        }
        else
        {
            Vector3 up = Vector3.up;
            Vector3 right = Camera.main.transform.right;
            Vector3 forward = Vector3.Cross(right, up);
            Vector3 moveInput = forward * _moveInput.y + right * _moveInput.x;
            Vector3 targetAcceleration = moveInput * _speed;
            Vector3 currentAcceleration = _rb.velocity;
            Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _acceleration;
            finalAcceleration.y = -_gravity;
            _rb.AddForce(finalAcceleration);
        }


        Vector3 LookDirection = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;
        float turnDirection = -Mathf.Sign(Vector3.Cross(_movementInput, transform.forward).y);
        float turnMagnitude = 1f - Mathf.Clamp01(Vector3.Dot(transform.forward, LookDirection));
        if (_moveInput.magnitude < 0.1f) turnMagnitude = 0f;
        float angularVelocity = turnMagnitude * _turnSpeed * turnDirection;
        _rb.angularVelocity = new Vector3(0f, angularVelocity, 0f);


    }

    public void BallTorque()
    {
            Rigidbody rb = _ball.GetComponent<Rigidbody>();
            Vector3 targetAcceleration = _movementInput * _rollSpeed;
            Vector3 currentAcceleration = rb.velocity;
            Vector3 finalAcceleration = (targetAcceleration - currentAcceleration) * _rollAcceleration;
            Vector3 Torque = Vector3.Cross(Vector3.up, finalAcceleration);
            rb.AddTorque(Torque * _torqueMultiplier);
    }
    //public void OnFire()
    //{
    //    if (!_ballMode)
    //        OnMount();
    //    else OnUnmount();
    //    GetComponent<Animator>().SetTrigger("Interact");
    //    _enterBallEffect.SendEvent(_enterBallEventName);

    //}

    public void OnMount()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.SetParent(_ball.transform);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _ball.GetComponent<Collider>(), true);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        _ballMode = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
    public void OnUnmount()
    {
        gameObject.transform.SetParent(null);
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), _ball.GetComponent<Collider>(), false);
        _ballMode = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        StartCoroutine(resetRotation());
    }

    IEnumerator resetRotation()
    {
        yield return new WaitForSeconds(0.5f);
        transform.rotation = Quaternion.Euler(new Vector3 (0, transform.rotation.eulerAngles.y, 0));

    }

    public void OnJump()
    {
        if (!_ballMode && IsGrounded())
        {
            _rb.AddForce(Vector3.up * _jumpHeight);
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }


    public void OnInteract()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, _interactionRadius);
        foreach (Collider c in col)
        {
            if (c.tag == "HamsterBall")
            {
                _ball = c.gameObject;
                    if (!_ballMode)
                        OnMount();
                    else OnUnmount();
                   GetComponent<Animator>().SetTrigger("Interact");
                   _enterBallEffect.SendEvent(_enterBallEventName);
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
       if (collision.gameObject.CompareTag("Player")) {
            Rigidbody otherrb = collision.rigidbody;
            // kewk
            FlattenHamster(otherrb.gameObject);
        }
    }

    public void FlattenHamster(GameObject otherHamster) {
        // Kewk
        otherHamster.gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 1f);
    }

}
