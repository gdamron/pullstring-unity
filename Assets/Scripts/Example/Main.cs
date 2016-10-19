using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PullString;

[RequireComponent(typeof(AudioSource))]
public class Main : MonoBehaviour
{
    private const string API_KEY = "9fd2a189-3d57-4c02-8a55-5f0159bff2cf";
    private const string PROJECT = "e50b56df-95b7-4fa1-9061-83a7a9bea372";
    private Conversation conversation;

    // Audio recording management
    private int audioOffset = 0;
    private AudioSource audioSource;
    private bool recording = false;

    // GUI management
    Vector2 scrollPosition = Vector2.zero;
    string userInput = string.Empty;
    bool linesAdded = false;
    private List<string> lines = new List<string>();

    // The Web API expects audio as 16-bit mono at 16000 samples per second
    private const int SAMPLE_RATE = 16000;
    // The duration of the AudioClip used to store audio input. It will loop over
    // itself if time runs out.
    private const int CLIP_DUR = 5;

    // GUI Layout constants
    private const int FONT_SIZE = 36;
    private const int STATUS_BAR = 40;
    private const float INPUT_HEIGHT = 64.0f;
    private const float INPUT_MARGIN = INPUT_HEIGHT + 12;
    private const int BUTTON_WIDTH = 128;

    void Start()
    {
        // prepare request and start conversation immediately
        conversation = gameObject.AddComponent<Conversation>();
        conversation.OnResponseReceived += OnResponseReceived;

        var request = new Request()
        {
            ApiKey = API_KEY
        };

        conversation.Begin(PROJECT, request);

        // since we'll be processing audio in real time, mute the
        // audio source.
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = true;
    }

    void Update()
    {
        if (Microphone.IsRecording(null))
        {
            // Get latest audio samples and pass them to the SDK
            var count = Microphone.GetPosition(null) - audioOffset;
            if (count < 0)
            {
                count = (int)(audioSource.clip.length * audioSource.clip.frequency) - audioOffset;
            }

            if (count == 0) return;

            var samples = new float[count];
            audioSource.clip.GetData(samples, audioOffset);
            conversation.AddAudio(samples);
            audioOffset = Microphone.GetPosition(null);
        }
    }

    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.contentColor = Color.black;

        var skin = GUI.skin;
        skin.label.fontSize = FONT_SIZE;
        skin.textField.fontSize = FONT_SIZE;
        skin.button.fontSize = FONT_SIZE;
        skin.textField.fixedHeight = INPUT_HEIGHT;
        skin.button.fixedHeight = INPUT_HEIGHT;

        // accomodate status bar in iOS
        GUILayout.Space(STATUS_BAR);

        // chat display window
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - INPUT_MARGIN - STATUS_BAR));
        for (int i = 0; i < lines.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(lines[i]);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        // listen for return key for sending input (only works in standalone)
        if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return)
        {
            sendText();
        }

        // user input
        userInput = GUILayout.TextField(userInput);
        if (GUILayout.Button("send", GUILayout.Width(BUTTON_WIDTH)))
        {
            sendText();
        }

        if (GUILayout.Button(recording ? "stop" : "record", GUILayout.Width(BUTTON_WIDTH)))
        {
            recording = !recording;
            if (recording)
            {
                audioSource.clip = Microphone.Start(null, true, CLIP_DUR, SAMPLE_RATE);
                audioSource.Play();
                conversation.StartAudio();
            }
            else
            {
                audioSource.Stop();
                Microphone.End(null);
                conversation.EndAudio();
            }
        }

        GUILayout.EndHorizontal();

        // autoscroll
        if (linesAdded)
        {
            scrollPosition.y = Mathf.Infinity;
            linesAdded = false;
        }
    }

    void sendText()
    {
        if (userInput.Equals(string.Empty))
        {
            return;
        }
        conversation.SendText(userInput);
        lines.Add("You: " + userInput);
        linesAdded = true;
        userInput = string.Empty;
    }

    void OnResponseReceived(Response response)
    {
        if (response == null) { return; }
        // As reaponses arrive from Web API, print any dialog to the chat window
        // responses will also contain speech recognition results
        if (response.AsrHypothesis != null)
        {
            var asrLine = "You: " + response.AsrHypothesis;
            lines.Add(asrLine);
            linesAdded = true;
        }

        if (response.Outputs != null)
        {
            var outputs = response.Outputs.Where(o => o.Type.Equals(EOutputType.Dialog));
            foreach (var output in outputs)
            {
                var dialog = (DialogOutput)output;
                var line = dialog.Character + ": " + dialog.Text;
                lines.Add(line);
                linesAdded = true;
            }
        }
    }
}
