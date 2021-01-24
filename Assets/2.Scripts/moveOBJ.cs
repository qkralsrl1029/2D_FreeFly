using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveOBJ : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] Transform endPos;
    [SerializeField] Transform startPos;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        posReset();
    }

    void posReset()
    {
        if (transform.position.x <= endPos.position.x)
        {
            transform.Translate(Vector3.right * -(endPos.position.x - startPos.position.x));

            if (transform.tag == "Block")
                transform.Translate(Vector3.up * Random.Range(-2f, 2f));
        }

    }
}
