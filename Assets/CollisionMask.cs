using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionMask : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Collider> colliders = new List<Collider>();
    public Collider col;
    void Start()
    {
        //busca los que ya esta colisionando
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Portal")) // no desahbilitar las colisiones con los portales
        {
            Debug.Log("Entro: " + other.name);
            IgnoreCollisionsInList(other, true);
            colliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Portal"))// no desahbilitar las colisiones con los portales
        {
            Debug.Log("Salio: " + other.name);
            IgnoreCollisionsInList(other, false);
            colliders.Remove(other);
        }
    }



    private void IgnoreCollisionsInList(Collider other, bool ignore)
    {
        foreach (Collider col in colliders)
        {
            Physics.IgnoreCollision(col, other,ignore);
        }
    }
}
