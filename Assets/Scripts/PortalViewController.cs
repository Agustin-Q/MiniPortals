using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase es responsable de hacer que el jugador vea a traves del protal
public class PortalViewController : MonoBehaviour
{
    public GameObject otherPortal;
    public GameObject otherPortalCamera;
    public GameObject playerCamera; 
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {


        //igual la configuracion de la camara del portal a la camara del jugador
        otherPortalCamera.GetComponent<Camera>().projectionMatrix = playerCamera.GetComponent<Camera>().projectionMatrix;

        //calcular direccion al protal
        Vector3 dirToPortal = transform.position - playerCamera.transform.position;



        //mover la camara del otro protal
     
        //obtener coordenadas de la camara del juagdor en espacio local respceto del portal
        Vector3 PCLPosition =  transform.InverseTransformPoint(playerCamera.transform.position) ;
        // Griar la posiciion 180 grados respexto del eje Y (esto hay que revisar si es siempre asi)
        PCLPosition = Quaternion.Euler(0, 180, 0) * PCLPosition;
        otherPortalCamera.transform.position = otherPortal.transform.TransformPoint( PCLPosition);

        //rotar la camara al mismo angulo que la camara del jugador
        otherPortalCamera.transform.rotation = playerCamera.transform.rotation;

    }

    void LogPosition(Vector3 pos, string name ="")
    {
        Debug.Log(name + " Position: " + pos.x + ", " + pos.y + ", " + pos.z);
    }
}
