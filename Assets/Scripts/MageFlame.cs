using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MageFlame : MonoBehaviour
{
    [SerializeField] float timer;

    [SerializeField] float delay;

    public GameObject projectile;

    private void Awake()
    {
        //Destroy(gameObject, 2);
    }


    private void Update()
    {
        timer += Time.deltaTime;

        if (gameObject.activeSelf)
        {
            if (timer >= delay)
            {
                timer = 0;

                Instantiate(projectile, transform.position, Quaternion.identity);
                //Instantiate(projectile, transform.position, Quaternion.identity);
                //Instantiate(projectile, transform.position, Quaternion.identity);
            }
        }
    }

}
