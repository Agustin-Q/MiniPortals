using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase es responsable de hacer que el jugador vea a traves del protal
public class PortalViewController : MonoBehaviour
{
    public GameObject otherPortal;
    public Camera otherPortalCamera;
    private Camera playerCamera;
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;
    // Start is called before the first frame update
    private void Awake()
    {
        playerCamera = Camera.main;
        
    }

    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {


        //igual la configuracion de la camara del portal a la camara del jugador
        otherPortalCamera.projectionMatrix = playerCamera.projectionMatrix;

        //mover la camara del otro protal
     
        //obtener coordenadas de la camara del juagdor en espacio local respceto del portal
        Vector3 PCLPosition =  transform.InverseTransformPoint(playerCamera.transform.position) ;
        // Griar la posiciion 180 grados respexto del eje Y (esto hay que revisar si es siempre asi)
        PCLPosition = Quaternion.Euler(0, 180, 0) * PCLPosition;
        otherPortalCamera.transform.position = otherPortal.transform.TransformPoint( PCLPosition);

        //rotar la camara al mismo angulo que la camara del jugador pero tiene que ser relativo
        
        //calcular direccion de la camara del juador
        Vector3 playerCameraDir = playerCamera.transform.rotation * Vector3.forward;
        //encontrar la direccion relativa de la camara del jugador al primer portal
        Vector3 relDirToPortal = transform.InverseTransformDirection(playerCameraDir);
        //girar el vector 180 respecto del eje Y (esto hay que revisar si sempre es asi)
        relDirToPortal = Quaternion.Euler(0, 180, 0) * relDirToPortal;
        //pasar a worl space respecto del otro portal
        Vector3 dirOfOtherCamera = otherPortal.transform.TransformDirection(relDirToPortal);
        //setear rotation con este vector
        otherPortalCamera.transform.rotation = Quaternion.LookRotation(dirOfOtherCamera, otherPortal.transform.up);


        SetNearClipPlane();
    }

    void SetNearClipPlane()
    {
        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf

        Camera portalCam = otherPortalCamera;
        Camera playerCam = playerCamera;

        Transform clipPlane = otherPortal.transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs(camSpaceDst) > nearClipLimit)
        {
            Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
        }
        else
        {
            portalCam.projectionMatrix = playerCam.projectionMatrix;
        }
    }
}
