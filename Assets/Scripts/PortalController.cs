using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//esta clase es responsable de tletransportar los objetos al otro portal
public class PortalController : MonoBehaviour
{
    public GameObject otherPortal;
    
    private PortalController otherPortalController;
    private GameObject traveller;
    // Start is called before the first frame update
    void Start()
    {
        otherPortalController = otherPortal.GetComponent<PortalController>();
    }

    // tal vez haya que hacerlo en fixed update
    void Update()
    {

        //chequear si algo toca el portal

        //chequear si pasa el portal

        //calcular direccion al protal
        if (traveller != null ) { //solo si tenemos un travellr cerca 
            Vector3 dirTravelerToPortal = transform.position - traveller.transform.position;
            // usamos el producto escalar entre la dir al portal y un vector normal al portal si son opuetos ese resultado es negativo tiene indica que no cruzo
            Vector3 portalNormalVect = transform.rotation * Vector3.forward;
            if (Vector3.Dot(dirTravelerToPortal, portalNormalVect) > 0f)
            {
                Debug.Log("cruzo");
                Teleport(traveller);
            }
        // si pasa teletransportalo y rotarlo
        // avisarle al otro portal que esta salidendo para que no lo vuelva a teletransportar devuelta
        }
    }

    private void Teleport(GameObject Teleportable)
    {
        if (otherPortal != null)
        {
            Teleportable.transform.position = otherPortal.transform.position;
            // calcular angulos entre los dos portales es la cantidad a rotar el judador
            float angleBetweenPortals = otherPortal.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y ;
            float absRotation = angleBetweenPortals + Teleportable.transform.rotation.eulerAngles.y;
            // rotar el jugador con ese angulo. Esto no es bueno ya que solo funciona con los portales verticales, y probablemente el FPAIO tampoco funicone con el judador invertido y esta funcionalidad tiene que estar.
            Teleportable.GetComponent<FirstPersonAIO>().RotateCamera(new Vector2(0, absRotation),true);
            
            otherPortalController.AddIncomingTraveller(Teleportable);
            traveller = null;
        }
    }


    public void AddIncomingTraveller(GameObject teleportable)
    {
        traveller = teleportable;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with: " + other.name);
        //TODO: verificar que sea algo "teletransportable"
        traveller = other.gameObject;

    }
}
