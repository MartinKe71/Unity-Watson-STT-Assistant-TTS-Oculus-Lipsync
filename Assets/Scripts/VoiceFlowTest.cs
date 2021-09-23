using IBM.Cloud.SDK.Utilities;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class VoiceFlowTest : MonoBehaviour
{
    public string User_Id;
    public string UserInput;

    [SerializeField]
    private VoiceFlowSettings Setting;

    public InputField inputField;

    private string _voiceflowUrl;

    // Start is called before the first frame update
    void Start()
    {
        _voiceflowUrl = $"https://general-runtime.voiceflow.com/state/{Setting.VoiceFlowVersionId}/user/{User_Id}/interact";

        StartCoroutine(LaunchChat());
        StartCoroutine(ProcessChat(""));

        inputField = gameObject.AddComponent<InputField>();
        inputField.textComponent = gameObject.AddComponent<Text>();
        inputField.onValueChanged.AddListener(delegate { Runnable.Run(ProcessChat(inputField.text)); });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LaunchChat()
    {
        using (var client = new HttpClient())
        {
            var obj = new JObject() { { "type", "launch" }};
            JObject body = new JObject();
            body.Add("request", obj);

            client.DefaultRequestHeaders.Add("Authorization", Setting.VoiceFlowApiKey);
            client.PostAsync(_voiceflowUrl, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
        }
        yield return null;
    }

    public IEnumerator ProcessChat(string chatInput)
    {
        UserInput = chatInput;
        using (var client = new HttpClient())
        {
            var obj = new JObject() { { "type", "text" }, { "payload", "Hello" } };
            JObject body = new JObject();
            body.Add("request", obj);

            client.DefaultRequestHeaders.Add("Authorization", Setting.VoiceFlowApiKey);
            var response = client.PostAsync(_voiceflowUrl, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            var content = response.Result.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            Debug.LogError(response.Status.ToString());
            Debug.LogError(response.Result.ToString());
            Debug.LogError(content.ReadAsStringAsync()?.Result);

            response = client.PostAsync(_voiceflowUrl, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            content = response.Result.Content;
            jsonContent = content.ReadAsStringAsync().Result;
            Debug.LogError(response.Status.ToString());
            Debug.LogError(response.Result.ToString());
            Debug.LogError(content.ReadAsStringAsync()?.Result);
        }

        yield return null;
    }
}
