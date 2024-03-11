using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    Animator animator;
    CharacterController characterController;
    
    public float speed = 0.6f;

    public float roatationSpeed = 25;
    
    public float jumpSpeed = 7.5f;
    public float gravity = 20.0f;

    Vector3 inputVec;
    Vector3 tragetDiraction;

    private Vector3 moveDiraction = Vector3.zero;
    void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float x = -(Input.GetAxisRaw("Vertical"));
        float z = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Input x :", z);
        animator.SetFloat("Input z :", -(x));
            
        inputVec = new Vector3(x,0,z);
        if (x != 0 || z != 0){
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
        }else{
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);   
        }
        
        // jump
        if(characterController.isGrounded){
            moveDiraction = new Vector3(Input.GetAxis("Horizontal"),0.0f,Input.GetAxis("Vertical"));
            moveDiraction *= speed;
        }
        characterController.Move(moveDiraction * Time.deltaTime);
        UpdateMoviement();
    }
    void UpdateMoviement() {
        Vector3 motion = inputVec;
        motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1)?.7f:1;
        RotateTowardMovmoentDiraction();
        getCameraRealtive();

        if (motion.magnitude > 0){
            if( !audioSource.isPlaying){
                audioSource.Play();
            }
        }else{
            audioSource.Stop();
        }
    }

    void RotateTowardMovmoentDiraction(){
        if(inputVec != Vector3.zero){
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(tragetDiraction),Time.deltaTime * roatationSpeed);
        }
    }

    void getCameraRealtive() {
        Transform cameraTranform = Camera.main.transform;
        Vector3 forward = cameraTranform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z,0,-forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        tragetDiraction = (h * right) + (v * forward);
    }
}
