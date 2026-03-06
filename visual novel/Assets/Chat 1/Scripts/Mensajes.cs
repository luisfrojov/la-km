using UnityEngine;
using TMPro;

public class Mensajes : MonoBehaviour
{
    public static Mensajes instance;

    public Transform Content_Parent;
    public GameObject Playersms_Ins;
    public GameObject ContactSms_Ins;

    void Awake()
    {
        instance = this;
    }

    public void AddMessage(bool senderPlayer, string message)
    {
        if (Content_Parent == null)
        {
            Debug.LogError("Content_Parent no asignado en sms");
            return;
        }

        GameObject prefab = senderPlayer ? Playersms_Ins : ContactSms_Ins;

        if (prefab == null)
        {
            Debug.LogError("Prefab no asignado en sms");
            return;
        }

        GameObject go = Instantiate(prefab, Content_Parent);

        Script t_Ref = go.GetComponent<Script>();

        if (t_Ref == null)
        {
            Debug.LogError("El prefab no tiene el Script");
            return;
        }

        if (t_Ref.Text_Reference == null)
        {
            Debug.LogError("Text_Reference no asignado en el prefab");
            return;
        }

        t_Ref.Text_Reference.text = message;
    }
}