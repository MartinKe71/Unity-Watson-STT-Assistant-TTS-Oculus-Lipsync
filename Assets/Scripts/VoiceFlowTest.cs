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

    public bool VoiceflowServiceLaunched = false;

    //Keep track of whether IBM Watson Assistant should process input or is
    //processing input to create a chat response.
    public enum ProcessingStatus { Process, Processing, Idle, Processed };
    private ProcessingStatus chatStatus;

    [SerializeField]
    private VoiceFlowSettings Setting;

    public InputField inputField;
    public Text ResponseText;

    private string _voiceflowUrl;

    // Start is called before the first frame update
    void Start()
    {
        _voiceflowUrl = $"https://general-runtime.voiceflow.com/state/{Setting.VoiceFlowVersionId}/user/{User_Id}/interact";

        LaunchChat();
        //StartCoroutine(ProcessChat(""));

        inputField = gameObject.AddComponent<InputField>();
        inputField.textComponent = gameObject.AddComponent<Text>();
        //inputField.onValueChanged.AddListener(delegate { Runnable.Run(ProcessChat(inputField.text)); });
        inputField.onValueChanged.AddListener(delegate { ProcessChat(inputField.text); });

        ProcessChat("Hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchChat()
    {
        using (var client = new HttpClient())
        {
            var obj = new JObject() { { "type", "launch" }};
            JObject body = new JObject();
            body.Add("request", obj);

            client.DefaultRequestHeaders.Add("Authorization", Setting.VoiceFlowApiKey);
            var response = client.PostAsync(_voiceflowUrl, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            Debug.LogError(response.Status.ToString());
            Debug.LogError(response.Result.ToString());
            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                VoiceflowServiceLaunched = true;
                chatStatus = ProcessingStatus.Idle;
            }
        }
    }

    public void ProcessChat(string chatInput)
    {
        UserInput = chatInput;
        chatStatus = ProcessingStatus.Processing;
        using (var client = new HttpClient())
        {
            var obj = new JObject() { { "type", "text" }, { "payload", UserInput } };
            JObject body = new JObject();
            body.Add("request", obj);

            client.DefaultRequestHeaders.Add("Authorization", Setting.VoiceFlowApiKey);
            var post = client.PostAsync(_voiceflowUrl, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            var content = post.Result.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            Debug.LogError(post.Status.ToString());
            Debug.LogError(post.Result.ToString());
            Debug.LogError(content.ReadAsStringAsync()?.Result);

            if (post.Result.StatusCode == HttpStatusCode.OK)
            {
                chatStatus = ProcessingStatus.Processed;

                JArray responseArray = JArray.Parse(jsonContent);
                foreach (var response in responseArray)
                {
                    if (response["type"].ToString() == "speak" || response["type"].ToString() == "text")
                    {
                        string txt = response["payload"]["message"].ToString();
                        if (ResponseText != null)
                        {
                            ResponseText.text = txt;
                        }
                        Debug.LogError(txt);
                    }                    
                }
            }
        }
    }

    public ProcessingStatus GetStatus()
    {
        return chatStatus;
    }
}
