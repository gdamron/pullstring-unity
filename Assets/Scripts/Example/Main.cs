//
// Assets/Scripts/Example/Main.cs
//
// A simple chat client using the PullString SDK
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using UnityEngine;
using System.Collections.Generic;
using PullString;

/// <summary>
/// A simple example showing basic interactions with the PullString SDK. This component adds a chat client
/// overlay to the screen. User input and dialog responses are printed to the chat window while any behavior
/// outputs are simply printed to the console.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Main : MonoBehaviour
{
    // Api key and project id for the Rock, Paper, Scissors bot. Try replacing these with your own. After
    // logging in to your account on pullstring.com, they can be found under Account > Web API Keys.
    private const string API_KEY = "9fd2a189-3d57-4c02-8a55-5f0159bff2cf";
    private const string PROJECT = "e50b56df-95b7-4fa1-9061-83a7a9bea372";

    // PullStringClient is shared across examples and contains the details of interacting with the SDK.
    private PullStringClient pullstring;
    private bool recording = false;

    // GUI management
    Vector2 scrollPosition = Vector2.zero;
    string userInput = string.Empty;
    bool linesAdded = false;
    private List<string> lines = new List<string>();

    // GUI Layout constants
    private const int FONT_SIZE = 36;
    private const int STATUS_BAR = 40;
    private const float INPUT_HEIGHT = 64.0f;
    private const float INPUT_MARGIN = INPUT_HEIGHT + 12;
    private const int BUTTON_WIDTH = 128;

    void Awake()
    {
        var conversation = gameObject.AddComponent<Conversation>();
        var audioSource = GetComponent<AudioSource>();
        pullstring = new PullStringClient()
        {
            Conversation = conversation,
            AudioSource = audioSource
        };
    }

    void Start()
    {
        pullstring.OnAsrReceived += OnAsrReceived;
        pullstring.OnDialogReceived += OnDialogReceived;
        pullstring.OnBehaviorReceived += OnBehaviorReceived;

        // being a conversation immediately
        pullstring.ApiKey = API_KEY;
        pullstring.Start(PROJECT);
    }

    void Stop()
    {
        pullstring.OnAsrReceived -= OnAsrReceived;
        pullstring.OnDialogReceived -= OnDialogReceived;
        pullstring.OnBehaviorReceived -= OnBehaviorReceived;

        pullstring.Stop();
    }

    void Update()
    {
        pullstring.Update();
    }

    void OnGUI()
    {
        // draw the chat window and input UI
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
            pullstring.ToggleRecording(recording);
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
        pullstring.SendText(userInput);
        lines.Add("You: " + userInput);
        linesAdded = true;
        userInput = string.Empty;
    }

    void OnAsrReceived(string asr)
    {
        // Print recognized speech to chat window
        var asrLine = "You: " + asr;
        lines.Add(asrLine);
        linesAdded = true;
    }

    void OnDialogReceived(DialogOutput[] dialogs)
    {
        // Print dialog responses to the chat window
        foreach (var output in dialogs)
        {
            var dialog = (DialogOutput)output;
            var line = dialog.Character + ": " + dialog.Text;
            lines.Add(line);
            linesAdded = true;
        }
    }

    void OnBehaviorReceived(BehaviorOutput[] behaviors)
    {
        // Print behaviors to the console
        foreach (var output in behaviors)
        {
            Debug.Log(output);
        }
    }
}
