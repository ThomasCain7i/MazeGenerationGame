using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallCotroller : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rigid;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            rigid.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rigid.AddForce(-Vector3.right * speed);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            rigid.AddForce(Vector3.forward * speed);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rigid.AddForce(-Vector3.forward * speed);
        }


    }
}
