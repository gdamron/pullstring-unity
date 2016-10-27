using UnityEngine;
using PullString;
using System.Linq;

/// <summary>
/// This class encapsulates basic interactions with the PullString SDK. It handles text and speech input and
/// lightly processes responses.
/// </summary>
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

    // The Web API expects audio as 16-bit mono at 16000 samples per second
    private const int SAMPLE_RATE = 16000;
    // The duration of the AudioClip used to store audio input. It will loop over
    // itself if time runs out.
    private const int CLIP_DUR = 5;

    public void Start(string project)
    {
        // All output from the SDK arrives via OnResponseReceived
        Conversation.OnResponseReceived += OnResponseReceived;

        // Since we'll be processing audio in real time, mute the
        // audio source.
        AudioSource.mute = true;

        if (string.IsNullOrEmpty(ApiKey)) { return; }

        // Prepare basic request and start conversation
        var request = new Request()
        {
            ApiKey = ApiKey
        };

        Conversation.Begin(project, request);
    }

    public void Stop()
    {
        Conversation.OnResponseReceived -= OnResponseReceived;
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
    }

    public void ToggleRecording(bool isRecording)
    {
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
    }
}
