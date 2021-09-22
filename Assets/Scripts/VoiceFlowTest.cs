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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessChat(string chatInput)
    {
        UserInput = chatInput;
        using (var client = new HttpClient())
        {
            var url = $"https://general-runtime.voiceflow.com/state/{Setting.VoiceFlowVersionId}/user/{User_Id}/interact";
            var body = "{ 'request': { 'type': 'text', 'payload': 'Hello world!'}";

            client.DefaultRequestHeaders.Add("Authorization", Setting.VoiceFlowApiKey);
            var response = client.PostAsync(url, new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            var content = response.Result.Content;
            string jsonContent = content.ReadAsStringAsync().Result;
            Debug.LogError(response.Status.ToString());
            Debug.LogError(response.Result.ToString());
        }
    }
}
