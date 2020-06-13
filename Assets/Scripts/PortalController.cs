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

    // transforma un vector en world coordinates a otro en world coordinates pero respecto del otro portal
    private Vector3 DirVectorPortalTransform(Vector3 vector, Transform T1, Transform T2)//esto transfroma de un portal al otro un vector;
    {
        Vector3 relVector = T1.InverseTransformDirection(vector); //vector realitvo al primer portal
        relVector = Quaternion.Euler(0, 180, 0) * relVector; //rotrar 180 grados respecto de y
        return T2.TransformDirection(relVector); //posision absoluta respecto del segudo portal
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
    // este metodo esta repetido en otra clase no la mejor etiqueta de programacion
    private Quaternion RotationRelativeToPortal(Quaternion rot, Transform T1, Transform T2) 
    {
        // la linea de abajo es magica, la magia de los Quaternions,
        // basicamente lo que hace es agarra la rotacion que le pasamos,
        // luego la rota por la inversa de la rotacion del portal,
        // esto nos da la rotacion realtiva al primer portal,//
        // luego la rota 180 grados respecto de y, esto es porque estos
        // protales lo que entrea sale por el mismo lado pero del otro portal,
        // luego lo rota por la rotacion del segundo portal,
        // las rotaciones se leen de derecha a izquierda
        return T2.rotation*Quaternion.Euler(0,180,0)* Quaternion.Inverse(T1.rotation)*rot;
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
