using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//esta clase se encarga de verificar que un objeto pueda pasar por el portal, y de hacer los respectibos colones
public class Teleportable : MonoBehaviour
{
    public string type = "Prop";
    public GameObject graphicsClone;
    private PortalController currentPortal;
    private PortalController otherPortal;
    // Start is called before the first frame update
    void Start()
    {
        if (graphicsClone == null)
        {
            GenerateGraphicsClone();
        }
        graphicsClone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //si el clon esta activo
        if (graphicsClone.activeSelf)
        {
            //actualziar la posicion del clon
            graphicsClone.transform.position = transform.position;
            graphicsClone.transform.rotation = transform.rotation;
            graphicsClone.transform.position = currentPortal.PosVectorPortalTransform(graphicsClone.transform.position, currentPortal.transform, otherPortal.transform);
            graphicsClone.transform.rotation = currentPortal.RotationRelativeToPortal(graphicsClone.transform.rotation, currentPortal.transform, otherPortal.transform);

        }

    }

    public void OnPortalEnter(PortalController m_CurrentPortal, PortalController m_OtherPortal) // tecincamente no es un evento, lo llama directo el protal
    {
        //Debug.Log("OnPortalEnter");
        currentPortal = m_CurrentPortal;
        otherPortal = m_OtherPortal;
        graphicsClone.SetActive(true);
        // activar el clon y colocarlo en la posicion correcta
    }

    public void OnPortalLeave(PortalController portal) // tecincamente no es un evento, lo llama directo el protal
    {
        if (currentPortal.Equals(portal)){ //igorar si estoy ya entre a orto portal
           // Debug.Log("OnPortalLeave");
            currentPortal = null;
            otherPortal = null;
            graphicsClone.SetActive(false);
        }
    }

    // basicamente borra todo lo que no sea un meshRenederer y un MeshFilter
    //tiene errores porque hay componentes que no se pueden borrar
    // pero funciona igual
    private void GenerateGraphicsClone()
    {
        graphicsClone = Instantiate(gameObject);
        graphicsClone.SetActive(false);
        Component[] components = graphicsClone.GetComponentsInChildren<Component>();
        //Debug.Log("----- components del clone -----");

        for (int i = components.Length - 1; i >= 0; i--)
        {
            string typeString = components[i].GetType().ToString();
            //Debug.Log(typeString);
            if (!components[i].GetType().Equals(typeof(MeshFilter)) && !components[i].GetType().Equals(typeof(MeshRenderer)) && !components[i].GetType().Equals(typeof(Transform)))
            {
                Destroy(components[i]);
                //Debug.Log("Destruido!!!!");

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colision!!! de objeto: " + this.name + " con " + collision.transform.name);
    }

}
