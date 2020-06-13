using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//esta clase se encarga de verificar que un objeto pueda pasar por el portal, y de hacer los respectibos colones
public class Teleportable : MonoBehaviour
{
    public string type = "Prop";
    private GameObject graphicsClone;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGraphicsClone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // basicamente borra todo lo que no sea un meshRenederer y un MeshFilter
    //tiene errores porque hay componentes que no se pueden borrar
    // pero funciona igual
    private void GenerateGraphicsClone()
    {
        graphicsClone = Instantiate(gameObject);
        graphicsClone.SetActive(false);
        Component[] components = graphicsClone.GetComponentsInChildren<Component>();
        Debug.Log("----- components del clone -----");

        for (int i = components.Length - 1; i >= 0; i--)
        {
            string typeString = components[i].GetType().ToString();
            Debug.Log(typeString);
            if (!components[i].GetType().Equals(typeof(MeshFilter)) && !components[i].GetType().Equals(typeof(MeshRenderer)) && !components[i].GetType().Equals(typeof(Transform)))
            {
                Destroy(components[i]);
                Debug.Log("Destruido!!!!");

            }
        }
    }
   
}
