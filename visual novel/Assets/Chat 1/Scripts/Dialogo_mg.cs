using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogoManager : MonoBehaviour
{
    [Header("Conversación")]
    public List<DialogoNodo> dialogos;

    [Header("UI Opciones")]
    public Transform opcionesParent;
    public GameObject botonOpcionPrefab;

    int nodoActual = 0;

    void Start()
    {
        
        if (Mensajes.instance == null)
        {
            Debug.LogError("No existe el objeto 'sms' con el script Mensajes en la escena");
            return;
        }

        if (opcionesParent == null)
        {
            Debug.LogError("OpcionesParent no asignado en DialogoManager");
            return;
        }

        if (botonOpcionPrefab == null)
        {
            Debug.LogError("BotonOpcionPrefab no asignado");
            return;
        }

        MostrarNodo();
    }

    void MostrarNodo()
    {
        if (nodoActual < 0 || nodoActual >= dialogos.Count)
        {
            Debug.Log("Fin de la conversación");
            return;
        }

        DialogoNodo nodo = dialogos[nodoActual];

        
        if (!string.IsNullOrEmpty(nodo.Mensaje))
        {
            Mensajes.instance.AddMessage(nodo.Jugador, nodo.Mensaje);
        }

        
        foreach (Transform child in opcionesParent)
            Destroy(child.gameObject);

      
        if (nodo.opciones != null && nodo.opciones.Length > 0)
        {
            for (int i = 0; i < nodo.opciones.Length; i++)
            {
                int index = i;

                GameObject btn = Instantiate(botonOpcionPrefab, opcionesParent);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = nodo.opciones[i];

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    SeleccionarOpcion(nodo.opciones[index], nodo.siguientes[index]);
                });
            }
        }
        else
        {
           
            if (nodo.siguientes != null && nodo.siguientes.Length > 0)
            {
                nodoActual = nodo.siguientes[0];
                Invoke(nameof(MostrarNodo), 0.8f);
            }
        }
    }

    void SeleccionarOpcion(string textoJugador, int siguienteNodo)
    {
       
        Mensajes.instance.AddMessage(true, textoJugador);

      
        foreach (Transform child in opcionesParent)
            Destroy(child.gameObject);

     
        nodoActual = siguienteNodo;

        Invoke(nameof(MostrarNodo), 0.5f);
    }
}