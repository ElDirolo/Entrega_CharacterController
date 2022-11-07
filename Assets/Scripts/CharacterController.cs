using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    public Transform cam;
    public Transform LookAtTransform;

    //variables para controlar velocidad, altura de salto y gravedad
    public float speed = 5;
    public float jumpHeight = 1;
    public float gravity = -9.81f;

    //variables para el ground sensor
    public bool isGrounded;
    public Transform groundSensor;
    public float sensorRadius = 0.1f;
    public LayerMask ground;
    private Vector3 playerVelocity;

    //variables para rotacion del personaje
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public GameObject[] cameras;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MovementTP();
    }


    void MovementTP()
    {
        float z = Input.GetAxisRaw("Vertical");
        //anim.SetFloat("VelZ", z);
        float x = Input.GetAxisRaw("Horizontal");
        //anim.SetFloat("VelX", x);
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, xAxis.Value, 0);
        LookAtTransform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, LookAtTransform.eulerAngles.z);

        if (Input.GetButton("Fire2"))
        {
            cameras[0].SetActive(false);
            cameras[1].SetActive(true);
        }
        else
        {
            cameras[0].SetActive(true);
            cameras[1].SetActive(false);
        }


        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        void Jump()
        {

            isGrounded = Physics.CheckSphere(groundSensor.position, sensorRadius, ground);

            //anim.SetBool("Jump", !isGrounded);

            if (isGrounded && playerVelocity.y < 0)
            {
                playerVelocity.y = 0;
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {

                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            playerVelocity.y += gravity * Time.deltaTime;

            //controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}
