using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Mensajes : MonoBehaviour
{
    public static Mensajes instance;

    [Header("Configuracion de UI")]
    public RectTransform Content_Parent;
    public ScrollRect scrollRect;

    [Header("Prefabs de Mensajes")]
    public GameObject Playersms_Ins;
    public GameObject ContactSms_Ins;

    void Awake() => instance = this;

    public void AddMessage(bool senderPlayer, string message)
    {
        if (Content_Parent == null) return;

        GameObject prefab = senderPlayer ? Playersms_Ins : ContactSms_Ins;
        if (prefab == null) return;

        GameObject go = Instantiate(prefab, Content_Parent);
        Script t_Ref = go.GetComponent<Script>();
        
        if (t_Ref != null && t_Ref.Text_Reference != null)
            t_Ref.Text_Reference.text = message;

        StartCoroutine(ForceScrollDown());
    }

    IEnumerator ForceScrollDown()
    {
        // Esperamos dos frames para que el motor de UI de Unity 6 calcule el tamaño
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (scrollRect != null) scrollRect.verticalNormalizedPosition = 0f;
    }
}