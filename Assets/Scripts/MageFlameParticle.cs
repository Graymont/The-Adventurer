using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageFlameParticle : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}
