using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private float speed = 2f;

    private bool isDead = false;

    [HideInInspector] public GameObject bulletHole;
    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null)
            return;

        if(isDead)
            return;
        
        if (Vector3.Distance(target.transform.position, this.transform.position) < 2)
        {
            if (GetComponent<Animator>().GetLayerWeight(1) == 0)
            {
                GetComponent<Animator>().SetLayerWeight(1, 1);
            }
            return;
        }

        if (GetComponent<Animator>().GetLayerWeight(1) == 1)
        {
            
            GetComponent<Animator>().SetLayerWeight(1,0);
        }
        var step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 lookatDirection = target.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(lookatDirection,transform.up);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            
        }
    }

    public void Damage()
    {
        Die();
    }

    void Die()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Animator>().SetLayerWeight(1,0);
        GetComponent<Animator>().SetTrigger("Die");
        isDead = true;
        Invoke(nameof(Disablethis),5f);
    }

    void Disablethis()
    {
        if (bulletHole != null)
            bulletHole.transform.parent = null;
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
