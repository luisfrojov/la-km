using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Dialogo_mg : MonoBehaviour
{
    [Header("Conversación")]
    public List<Dialogo_nd> dialogos;

    [Header("UI Opciones")]
    public RectTransform opcionesParent; // Cambia Transform por RectTransform
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
        // 1. Safety Check
        if (nodoActual < 0 || nodoActual >= dialogos.Count)
        {
            Debug.Log("Fin de la conversación");
            return;
        }

        Dialogo_nd nodo = dialogos[nodoActual];

        // 2. Show the NPC/Current message
        if (!string.IsNullOrEmpty(nodo.Mensaje))
        {
            Mensajes.instance.AddMessage(nodo.Jugador, nodo.Mensaje);
        }

        // 3. Clear UI immediately
        foreach (Transform child in opcionesParent)
        {
            child.gameObject.SetActive(false); // Lo ocultas al instante
            Destroy(child.gameObject);         // Lo borras al final del frame
        }
        // 4. Handle Branching
        if (nodo.opciones != null && nodo.opciones.Length > 0)
        {
            for (int i = 0; i < nodo.opciones.Length; i++)
            {
                // Local copy of i is vital for the listener closure
                int index = i; 
                GameObject btn = Instantiate(botonOpcionPrefab, opcionesParent);
                
                var textComp = btn.GetComponentInChildren<TextMeshProUGUI>();
                if(textComp != null) textComp.text = nodo.opciones[index];

                btn.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // Check if index is valid for the 'siguientes' array
                    int next = (index < nodo.siguientes.Length) ? nodo.siguientes[index] : -1;
                    SeleccionarOpcion(nodo.opciones[index], next);
                });
            }
        }
        else
        {
            // Auto-advance if no options exist
            if (nodo.siguientes != null && nodo.siguientes.Length > 0)
            {
                nodoActual = nodo.siguientes[0];
                CancelInvoke(nameof(MostrarNodo)); // Prevent overlapping invokes
                Invoke(nameof(MostrarNodo), 1.2f); // Slightly longer for readability
            }
        }
    }

    void SeleccionarOpcion(string textoJugador, int siguienteNodo)
    {
        Mensajes.instance.AddMessage(true, textoJugador);

        // Clear buttons immediately so player can't double-click
        foreach (Transform child in opcionesParent)
            Destroy(child.gameObject);

        nodoActual = siguienteNodo;
        CancelInvoke(nameof(MostrarNodo));
        Invoke(nameof(MostrarNodo), 0.6f);
    }

}