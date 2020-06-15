using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase es responsable de hacer que el jugador vea a traves del protal
public class PortalViewController : MonoBehaviour
{
    public GameObject otherPortal;
    private Camera otherPortalCamera;
    private Camera playerCamera;
    private RenderTexture renderTexture;
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;
    public Shader portalShader;
    private Material portalMaterial;
    // Start is called before the first frame update
    private void Awake()
    {
        playerCamera = Camera.main;
        otherPortalCamera = otherPortal.GetComponentInChildren<Camera>();
        //setup textures
        renderTexture = new RenderTexture(1024, 1024, 24);
        //setup other portal camera to output to other portal render texture
        otherPortalCamera.targetTexture = renderTexture;
        //generate material
        portalMaterial = new Material(portalShader);
        // agregar la textura al material
        portalMaterial.SetTexture("PortalTexture", renderTexture);
        //buscar el material de la pantalla del portal y asignar el material
        MeshRenderer portalScreen = GetComponentInChildren<MeshRenderer>(); //tiene que haber un solo MeshReneder en el protal
        portalScreen.material = portalMaterial;
         
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
        otherPortalCamera.transform.rotation = RotationRelativeToPortal(playerCamera.transform.rotation, transform, otherPortal.transform);
        SetNearClipPlane();
    }

    void SetNearClipPlane()
    {
        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf

        Camera portalCam = otherPortalCamera;
        Camera playerCam = playerCamera;

        Transform clipPlane = otherPortal.transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, otherPortal.transform.position - portalCam.transform.position));

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

    // este metodo esta repetido en portal controller no la mejor etiqueta de programacion
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
        return T2.rotation * Quaternion.Euler(0, 180, 0) * Quaternion.Inverse(T1.rotation) * rot;
    }
}
