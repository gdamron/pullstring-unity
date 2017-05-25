//
// Assets/Scripts/Example/PullStringClient.cs
//
// Demonstrates basic interactions with the PullString SDK. It handles text and speech input and
// lightly processes responses.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using UnityEngine;
using PullString;
using System.Linq;
using System.Timers;

public class PullStringClient
{
    public string ApiKey { get; set; }
    public Conversation Conversation { get; set; }
    public AudioSource AudioSource { get; set; }

    // Return ASR reponses, dialog outputs, and behavior outputs through
    // separate delegates for convenience.
    public delegate void AsrDelegate(string asr);
    public delegate void DialogDelegate(DialogOutput[] output);
    public delegate void BehaviorDelegate(BehaviorOutput[] behavior);

    public event AsrDelegate OnAsrReceived;
    public event DialogDelegate OnDialogReceived;
    public event BehaviorDelegate OnBehaviorReceived;

    // Track the position within the Microphone's audio clip.
    private int audioOffset = 0;

    // Keep track of TimedResponseIntervals
    private Timer noResponseTimer;
    private bool timerFired = false;

    // The Web API expects audio as 16-bit mono at 16000 samples per second
    private const int SAMPLE_RATE = 16000;
    // The duration of the AudioClip used to store audio input. It will loop over
    // itself if time runs out.
    private const int CLIP_DUR = 5;

    public void Start(string project, string buildType = EBuildType.Production)
    {
        noResponseTimer = new Timer { AutoReset = false };
        noResponseTimer.Elapsed += OnTimerElapsed;

        // All output from the SDK arrives via OnResponseReceived
        Conversation.OnResponseReceived += OnResponseReceived;

        // Since we'll be processing audio in real time, mute the
        // audio source.
        AudioSource.mute = true;

        if (string.IsNullOrEmpty(ApiKey)) { return; }

        // Prepare basic request and start conversation
        var request = new Request()
        {
            ApiKey = ApiKey,
            BuildType = buildType
        };

        Conversation.Begin(project, request);
    }

    public void Stop()
    {
        Conversation.OnResponseReceived -= OnResponseReceived;
        noResponseTimer.Elapsed -= OnTimerElapsed;
    }

    public void Update()
    {
        if (Microphone.IsRecording(null))
        {
            // Get latest audio samples and pass them to the SDK
            var count = Microphone.GetPosition(null) - audioOffset;
            if (count < 0)
            {
                count = (int)(AudioSource.clip.length * AudioSource.clip.frequency) - audioOffset;
            }

            if (count == 0) return;

            var samples = new float[count];
            AudioSource.clip.GetData(samples, audioOffset);
            Conversation.AddAudio(samples);
            audioOffset = Microphone.GetPosition(null);
        }

        if (timerFired)
        {
            timerFired = false;
            Conversation.CheckForTimedResponse();
        }
    }

    public void ToggleRecording(bool isRecording)
    {
        noResponseTimer.Stop();

        if (isRecording)
        {
            AudioSource.clip = Microphone.Start(null, true, CLIP_DUR, SAMPLE_RATE);
            AudioSource.Play();
            Conversation.StartAudio();
        }
        else
        {
            AudioSource.Stop();
            Microphone.End(null);
            Conversation.EndAudio();
        }
    }

    public void SendText(string userInput)
    {
        Conversation.SendText(userInput);
    }

    void OnResponseReceived(Response response)
    {
        if (response == null) { return; }

        var delayTime = 0.0;

        if (!string.IsNullOrEmpty(response.AsrHypothesis) && OnAsrReceived != null)
        {
            OnAsrReceived(response.AsrHypothesis);
        }

        if (response.Outputs != null)
        {
            // Select DialogOutput objects
            var dialogs = response.Outputs
            .Where(o => o.Type.Equals(EOutputType.Dialog))
            .Select(o => (DialogOutput)o)
            .ToArray();

            if (dialogs != null && dialogs.Length > 0 && OnDialogReceived != null)
            {
                OnDialogReceived(dialogs);
            }

            delayTime = dialogs.Select(d => d.Duration).Aggregate((sum, next) => sum + next);


            // And then BehaviorOutput objects
            var behaviors = response.Outputs
            .Where(o => o.Type.Equals(EOutputType.Behavior))
            .Select(o => (BehaviorOutput)o)
            .ToArray();

            if (behaviors != null && behaviors.Length > 0 && OnBehaviorReceived != null)
            {
                OnBehaviorReceived(behaviors);
            }
        }

        if (response.TimedResponseInterval > 0)
        {
            delayTime += response.TimedResponseInterval;
            noResponseTimer.Stop();
            noResponseTimer.Interval = delayTime * 1000;
            noResponseTimer.Start();
        }
    }

    void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        timerFired = true;
    }
}
