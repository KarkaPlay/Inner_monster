using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueResponseController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Response;
    [SerializeField]
    private DialogueManager m_DialogueManager;
    [SerializeField]
    private int m_ReplyNextMsg;

    public Response Resp
    {
        set
        {
            m_Response.text = value.reply;
            m_ReplyNextMsg = value.next;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        m_DialogueManager.GetResponse(m_ReplyNextMsg);
    }
}