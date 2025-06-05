using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;
using System;

public class GroqChatAI : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI aiTextDisplay;

    [Header("Groq Settings")]
    public string systemPrompt = "You are an assistant that shares short, helpful facts about climate change and sustainability.";
    public string userMessage = "Give me one eco-friendly lifestyle tip.";
    public string model = "llama3-70b-8192";
    public string groqApiKey = "YOUR_GROQ_API_KEY_HERE"; // <- paste it in Inspector

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;

        public Message(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }

    [System.Serializable]
    public class RequestData
    {
        public string model;
        public Message[] messages;
        public float temperature;
        public int max_tokens;

        public RequestData(string model, Message[] messages, float temperature, int max_tokens)
        {
            this.model = model;
            this.messages = messages;
            this.temperature = temperature;
            this.max_tokens = max_tokens;
        }
    }

    [System.Serializable]
    public class GroqResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
    }

    void Start()
    {
       // StartCoroutine(FetchGroqResponse());
    }

    public void StartAISpeech()
    {
        StartCoroutine(FetchGroqResponse());
    }
    IEnumerator FetchGroqResponse()
    {
        string url = "https://api.groq.com/openai/v1/chat/completions";

        Message[] messages = new Message[]
        {
            new Message("system", systemPrompt),
            new Message("user", userMessage)
        };

        RequestData requestData = new RequestData(model, messages, 0.7f, 100);
        string json = JsonUtility.ToJson(requestData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + groqApiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GroqResponse result = JsonUtility.FromJson<GroqResponse>(FixJson(request.downloadHandler.text));
            string reply = result.choices[0].message.content;
            aiTextDisplay.text = reply;
            Debug.Log("AI Reply: " + reply);  // <-- This logs the reply text to Console
        }
        else
        {
            aiTextDisplay.text = $"Error: {request.error}";
        }
    }

    // Fix Groq's response to be compatible with Unity's JSON system
    string FixJson(string json)
    {
        // Unity's JsonUtility needs array fields to be wrapped in an object
        // Workaround: wrap "choices": [...] into a class with that key
        if (!json.Contains("\"choices\"")) return json;

        int startIndex = json.IndexOf("\"choices\"");
        int arrayStart = json.IndexOf("[", startIndex);
        int arrayEnd = json.IndexOf("]", arrayStart);

        string arrayContent = json.Substring(arrayStart, arrayEnd - arrayStart + 1);
        return "{\"choices\":" + arrayContent + "}";
    }
}
