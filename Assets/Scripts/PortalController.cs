using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//esta clase es responsable de tletransportar los objetos al otro portal
public class PortalController : MonoBehaviour
{
    public GameObject otherPortal;
    
    private PortalController otherPortalController;
    private List<GameObject> travellers = new List<GameObject>();
    private List<GameObject> clones = new List<GameObject>();
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
        for (int i =travellers.Count -1; i>=0; i--) //iterar la lista para atras para poder ir eliminando elementos
        {
            if (travellers[i] != null)
            { //solo si tenemos un travellr cerca 
                Vector3 dirTravelerToPortal = transform.position - travellers[i].transform.position;
                // usamos el producto escalar entre la dir al portal y un vector normal al portal si son opuetos ese resultado es negativo tiene indica que no cruzo
                Vector3 portalNormalVect = transform.rotation * Vector3.forward;
                if (Vector3.Dot(dirTravelerToPortal, portalNormalVect) > 0f)
                {
                    Debug.Log("cruzo");
                    Teleport(travellers[i]);
                    //sacar al traveller de la lista
                    travellers.RemoveAt(i);
                }
                // si pasa teletransportalo y rotarlo
                // avisarle al otro portal que esta salidendo para que no lo vuelva a teletransportar devuelta
            }
        }
    }
    //pregunta: el Teleportable deberia manejar la teletransportacion?
    private void Teleport(GameObject teleportable)
    {
        if (otherPortal != null)
        {
            if (teleportable.GetComponent<Teleportable>().type == "Player") {
                teleportable.transform.position = PosVectorPortalTransform(teleportable.transform.position, transform, otherPortal.transform);

                Debug.Log("Teleportable.transform.position: " + teleportable.transform.position.ToString("F3"));
                // calcular angulos entre los dos portales es la cantidad a rotar el judador
                float angleBetweenPortals = otherPortal.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
                float absRotation = angleBetweenPortals + teleportable.transform.rotation.eulerAngles.y;
                // rotar el jugador con ese angulo. Esto no es bueno ya que solo funciona con los portales verticales, y probablemente el FPAIO tampoco funicone con el judador invertido y esta funcionalidad tiene que estar.
                float lookAngle = teleportable.GetComponent<FirstPersonAIO>().targetAngles.x;
                teleportable.GetComponent<FirstPersonAIO>().RotateCamera(new Vector2(lookAngle, absRotation), true); // esta fncion seta la orientacion en valor absoluto, no es bueno, queremos que todo sea relativo a los protales
            } else if (teleportable.GetComponent<Teleportable>().type == "Prop") {
                //------Teleportar otro objeto que no sea el jugador-----
                teleportable.transform.position = PosVectorPortalTransform(teleportable.transform.position, transform, otherPortal.transform);

                teleportable.transform.rotation = RotationRelativeToPortal(teleportable.transform.rotation, transform, otherPortal.transform);
                //---- conservar las cantidad de movimiento
                Rigidbody RB = teleportable.GetComponent<Rigidbody>();
                RB.velocity = DirVectorPortalTransform(RB.velocity, transform, otherPortal.transform);
                //---- conservar la cantidad de movmiento angular
                RB.angularVelocity = DirVectorPortalTransform(RB.angularVelocity, transform, otherPortal.transform);
            }
            otherPortalController.AddTraveller(teleportable);
        }
    }

    private Vector3 DirVectorPortalTransform(Vector3 vector, Transform T1, Transform T2)//esto transfroma de un portal al otro un vector;
    {
        Vector3 relVector = T1.InverseTransformDirection(vector);
        relVector = Quaternion.Euler(0, 180, 0) * relVector; //rotrar 180 grados respecto de y
        return T2.TransformDirection(relVector);
    }

    private Vector3 PosVectorPortalTransform(Vector3 vector, Transform T1, Transform T2)//esto transfroma de un portal al otro un vector;
    {
        // encontrar la posicion relativa del objeto al portal en coordenadas locales
        Vector3 relPos = T1.InverseTransformPoint(vector);
        //Debug.Log("relPos: " + relPos.ToString("F3"));
        //girar el vector 180 grados respecto de y
        Vector3 relPosGirado = Quaternion.Euler(0, 180, 0) * relPos; //estaba con esta linea, para calcular la nueva posicion en el otro portal
        //Debug.Log("relPosGirado: " + relPosGirado.ToString("F3"));
        // encontrar posicion absoluta respecto de la posicion relativa al segundo portal
        Vector3 absPos = T2.TransformPoint(relPosGirado);
        //Debug.Log("absPos: " + absPos.ToString("F3"));
        return absPos;
    }
    // esto no esta funcionando la concha de su madre
    private Quaternion RotationRelativeToPortal(Quaternion rot, Transform T1, Transform T2) 
    {
        Vector3 fwrVector = rot * Vector3.forward;
        Vector3 relfowarVector = T1.InverseTransformDirection(fwrVector);
        relfowarVector = Quaternion.Euler(0, 180, 0) * relfowarVector;
        Vector3 absForwardVector = T2.TransformDirection(relfowarVector);
        return Quaternion.LookRotation(absForwardVector,T2.up);
    }

    public void AddTraveller(GameObject teleportable)
    {
        if (!travellers.Contains(teleportable))
        {
            travellers.Add(teleportable);
        }
    }

    public void addClone(GameObject dummyClone)
    {
        if (!clones.Contains(dummyClone))
        {
            travellers.Add(dummyClone);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision with: " + other.name);
        //verificar que sea algo "teletransportable"
        Teleportable teleportableObject = other.GetComponent<Teleportable>();
        if (teleportableObject)
        {
            AddTraveller(other.gameObject);
            //otherPortalController.addClone(teleportableObject.createDummyClone());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        travellers.Remove(other.gameObject);
    }

}
