# PullString SDK Class Reference

The PullString Web API lets you add text or audio conversational capabilities to your apps, based upon content that you write in the PullString Author environment and publish to the PullString Platform.

## Namespaces

| Name | Description |
| ---- | ---- |
| [`PullString`](#namespace_pullstring)| Main [PullString](#namespace_pullstring) namespace |

### Summary

 Name                        | Description
-----------------------------|---------------------------------------------
`class `[`Conversation`](#conversation) | The [Conversation](#conversation) class can be used to interface with the [PullString](#namespace_pullstring) API.
`class `[`Request`](#request) | Describe the parameters for a request to the [PullString](#namespace_pullstring) Web API.
`class `[`Phoneme`](#phoneme) | Describe a single phoneme for an audio response, e.g., to drive automatic lip sync.
`class `[`Entity`](#entity) | Base class to describe a single entity, such as a label, counter, flag, or list.
`class `[`Label`](#label) | Subclass of [Entity](#entity) to describe a single [Label](#label).
`class `[`Counter`](#counter) | Subclass of [Entity](#entity) to describe a single counter.
`class `[`Flag`](#flag) | Subclass of [Entity](#entity) to describe a single [Flag](#flag).
`class `[`List`](#list) | Subclass of [Entity](#entity) to describe a single [List](#list).
`class `[`Output`](#output) | Base class for outputs that are of type dialog or behavior.
`class `[`DialogOutput`](#dialogoutput) | Subclass of [Output](#output) that represents a dialog response
`class `[`BehaviorOutput`](#behavioroutput) | Subclass of [Output](#Output) that represents a behavior response
`class `[`Status`](#status) | Describe the status and any errors from a Web API response.
`class `[`Response`](#response) | Presents the output of a request to the [PullString](#namespace_pullstring) Web API.
`class `[`VersionInfo`](#versioninfo) | Encapsulates version information for [PullString](#namespace_pullstring)'s Web API.
`class `[`EBuildType`](#ebuildtype) | The asset build type to request for Web API requests.
`class `[`EEntityType`](#eentitytype) | Define the list of entity types
`class `[`EOutputType`](#eoutputtype) | Define the set of outputs that can be returned in a response.
`class `[`EFeatureName`](#eoutputtype) | Define features to check if they are supported.

<a name="namespace_pullstring"></a>

## Namespace PullString
Main PullString SDK namespace

<a name="conversation"></a>

## class Conversation

```
class Conversation
  : public MonoBehaviour
```

The [Conversation](#conversation) class can be used to interface with the [PullString](#namespace_pullstring) API.

To begin a conversation, call the [Begin()](#conversation+begin) method, providing a [PullString](#namespace_pullstring) project ID and a [Request](#request) object specifying your API key.

The Web API returns a [Response](#response) object that can contain zero or more outputs, such as lines of dialog or behaviors. This [Response](#response) object is passed to the OnResponseReceived callback as its sole parameter.

#### Sample Code

```cs

// The API Key and Project ID can be found by logging in to pullstring.com and
// navigating to Account > Web API keys (platform.pullstring.com/accounts/keys/)
const string MY_API_KEY = '...';
const string MY_PROJECT_ID = '...';

Conversation conversation;
bool timerFired = false;
Timer noResponseTimer;


void Start()
{
    // Prepare a timer in case Timed Response Intervals have been set in Author
    noResponseTimer = new Timer() { AutoReset = false };
    noResponseTimer.Elapsed += OnTimerElapsed;

    // create conversation and register response delegate.
    conversation = new Conversation();
    conversation.OnResponseReceived += OnResponse;

    // To start a conversation, pass a valid Project ID and a request containing
    // at least a valid API Key. You can also set the request's conversationId
    // and participantId to a stored values to continue a previous conversation.
    var request = new Request() {
        ApiKey = MY_API_KEY,
    };
    conversation.start(MY_PROJECT_ID, request);
}

void Update()
{
    if (timerFired)
    {
        // Ping the Web API for more input
        timerFired = false;
        conversation.CheckForTimedResponse();
    }
}

...

void OnResponse(Response response)
{
    // response.outputs can contain dialog as well as any behaviors.
    foreach (var output in response.Outputs) {
        if (output.Type.Equals(EOutputType.Dialog)) {
            // Often, we're most concerned with the dialog response text, but
            // dialog responses can contain audio and video uris as well as
            // the line's duration.
            Debug.Log(output.Character + ": " + output.Text);
        }

        // All custom behaviors defined in PullString Author are returned
        // by the Web API when they occur. Others, such as setting a label,
        // are internal to the bot and will not appear in responses.
        if (output.Type.Equals(EOutputType.Behavior)) {
            Debug.Log(output.Behavior);
            Debug.Log(output.Parameters);
        }
    }

    // if timed response interval is set, set a timer to check the Web API for
    // more output in the specified number of seconds.
    if (response.TimedResponseInterval > 0) {
        // convert timed response interval to milliseconds
        var delayTime = response.TimedResponseInterval * 1000;
        // start the timer
        timer.Stop();
        timer.Interval = delayTime;
        timer.Start();
    }
}

// When the timout expires, ping the Web API.
void OnTimerElapsed(object sender, ElapsedEventArgs e)
{
    // CheckForTimedResponse must be called from the main thread, i.e. in Update()
    timerFired = true;
}

...

// At some point, send some text to the bot.
public void HelloWorld()
{
    noResponseTimer.Stop();
    conversation.sendText("Hello, world!");
}

```

### Summary

| Name | Type | Description
| --- | --- | ---
| [`ConversationId`](#conversation+ConversationId) | `string` | The current conversation ID. Conversation IDs can persist across sessions, if desired.
| [`ParticipantId`](#conversation+ParticipantId) | `string` | Get the current participant ID, which identifies the current state for clients. This can persist accross sessions, if desired.
| [`ResponseDelegate`](#conversation+ResponseDelegate) | `delegate` | The signature for a function that receives reseponse from the PullString Web API.
| [`OnResponseReceived`](#conversation+OnResponseReceived) | `ResponseDelegate` | The event for receiving responses from the PullString Web API.

* [Conversation](#conversation)
    * [.Begin(projectName, request)](#conversation+Begin)
    * [.SendText(text, [request])](#conversation+SendText)
    * [.SendActivity(activity, [request])](#conversation+SendActivity)
    * [.SendEvent(event, parameters, [request])](#conversation+SendEvent)
    * [.StartAudio([request])](#conversation+StartAudio)
    * [.AddAudio(buffer)](#conversation+AddAudio)
    * [.StopAudio()](#conversation+StopAudio)
    * [.SendAudio(audio, format, [request])](#conversation+SendAudio)
    * [.GoTo(responseId, [request])](#conversation+GoTo)
    * [.CheckForTimedResponse([request])](#conversation+CheckForTimedResponse)
    * [.GetEntities(entities, [request])](#conversation+GetEntities)
    * [.SetEntities(entities, [request])](#conversation+SetEntities)
    * [.GetConversationId()](#conversation+GetConversationId) ⇒ `string`
    * [.GetParticipantId()](#conversation+GetParticipantId) ⇒ `string`

### Properties

<a name="conversation+ConversationId"></a>
#### `public string ConversationId`

The current conversation ID. [Conversation](#conversation) IDs can persist across sessions, if desired.

<a name="conversation+ParticipantId"></a>
#### `public string ParticipantId`

Get the current participant ID, which identifies the current state for clients. This can persist accross sessions, if desired.

<a name="conversation+ResponseDelegate"></a>
#### `public delegate void ResponseDelegate(`[`Response`](#response)` response)`

The signature for a function that receives reseponse from the [PullString](#namespace_pullstring) Web API.

```cs
void OnPullstringResponse(Response reseponse)
{
    // handle response...
}
```

**Parameters**
* `response` A [Response](#response) object from the Web API

<a name="conversation+OnResponseReceived"></a>
### `public ResponseDelegate OnResponseReceived`

The event for receiving responses from the PullString Web API.

```cs
conversation.OnResponseReceived += OnPullStringResponse;

// ...

void OnPullStringResponse(Response response)
{
  // handle response...
}
```

### Methods

<a name="conversation+Begin"></a>
#### `public void Begin(string project,`[`Request`](#request)` request)`

Start a new conversation with the Web API and receive a response via the OnResponseReceived event.

```cs
var request = new Request () {
    ApiKey = MY_API_KEY
};
conversation.Begin(MY_PROJECT, request);
```

**Parameters**
* `project` The PullSring project ID.
* `request` A [Request](#request) object with a valid ApiKey value set

<a name="conversation+SendText"></a>
#### `public void SendText(string text,`[`Request`](#request)` request = null)`

Send user input text to the Web API and receive a response via the OnResponseReceived event.

**Parameters**
* `text` User input text.
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+SendActivity"></a>
#### `public void SendActivity(string activity,`[`Request`](#request)` request = null)`

Send an activity name or ID to the Web API and receive a response via the OnResponseReceived event.

**Parameters**
* `activity` The activity name or ID.
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+SendEvent"></a>
#### `public void SendEvent(string eventName,Dictionary< string, object > parameters,`[`Request`](#request)` request = null)`

Send an event to the Web API and receive a response via the OnResponseReceived event.

**Parameters**
* `eventName` The event name.
* `parameters` Any accompanying paramters.
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+GoTo"></a>
#### `public void GoTo(string responseId,`[`Request`](#request)` request = null)`

Jump the conversation directly to a response.

**Parameters**
* `responseId` The UUID of the response to jump to.
* `request` [Optional] A request object with at least apiKey and conversationId set.


<a name="conversation+CheckForTimedResponse"></a>
#### `public void CheckForTimedResponse(`[`Request`](#request)` request = null)`

Call the Web API to see if there is a time-based response to process. You nly need to call this if the previous response returned a value for the timedResponseInterval >= 0. In this case, set a timer for that value (in seconds) and then call this method. If there is no time-based response, OnResponseReceived will be passed an empty [Response](#response) object.

**Parameters**
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+GetEntities"></a>
#### `public void GetEntities(string [] names,`[`Request`](#request)` request = null)`

[Request](#request) the values of the specified entities (i.e.: labels, counters, flags, and lists) from the Web API.

**Parameters**
* `names` An array of entity names.
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+SetEntities"></a>
#### `public void SetEntities(`[`Entity`](#entity)` [] entities,`[`Request`](#request)` request = null)`

Change the value of the specified entities (i.e.: labels, counters, flags, and lists) via the Web API.

**Parameters**
* `entities` An array specifying the entities to set (with their new values).
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+SendAudio"></a>
#### `public void SendAudio(AudioClip clip,`[`Request`](#request)` request = null`

Send an entire audio sample of the user speaking to the Web API. Audio must be, mono 16-bit linear PCM at a sample rate of 16000 samples per second.

**Parameters**
* `clip` Mono 16-bit linear PCM audio clip at 16k Hz.
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+StartAudio"></a>
#### `public void StartAudio(`[`Request`](#request)` request = null)`

Initiate a progressive (chunked) streaming of audio data, where supported.

If not supported, such as in the Web Player, this will batch up all audio and send it all at once when [EndAudio()](#conversation+EndAudio) is called.

**Parameters**
* `request` [Optional] A request object with at least apiKey and conversationId set.

<a name="conversation+AddAudio"></a>
#### `public void AddAudio(float [] audio)`

Add a chunk of raw audio samples. You must call [StartAudio()](#conversation+StartAudio) first. The format of the audio must be mono linear PCM audio data at a sample rate of 16000 samples per second.

**Parameters**
* `audio`

<a name="conversation+EndAudio"></a>
#### `public void EndAudio()`

Signal that all audio has been provided via [AddAudio()](#conversation+AddAudio) calls. This will complete the audio request and return the Web API response via the OnResponseReceived event.

<a name="request"></a>
# class Request

Describe the parameters for a request to the [PullString](#namespace_pullstring) Web API.


### Summary

| Name | Type | Description
| ---- | ---- | ----------- |
| [`ApiKey`](#request+ApiKey) | `string` | Your API key, required for all requests.
| [`ParticipantId`](#request+ParticipantId) | `string` | Identifies state to the Web API and can persist across sessions.
| [`BuildType`](#request+BuildType) | `string` | Defaults to [EBuildType.Production](#ebuildtype+Production).
| [`ConversationId`](#request+ConversationId) | `string` | Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.
| [`Language`](#request+Language) | `string` | ASR language; defaults to 'en-US'.
| [`Locale`](#request+Locale) | `string` | User locale; defaults to'en-US'.
| [`TimezoneOffset`](#request+TimezoneOffset) | `int` | A value in seconds representing the offset in UTC. For example, PST would be -28800.
| [`RestartIfModified`](#request+RestartIfModified) | `bool` | Restart this conversation if a newer version of the project has been published. Default value is true.
| [`AccountId`](#request+AccountId) | `string` |

* [Request](#request)
    * [.ToString()](#request+ToString)

### Properties

<a name="request+ApiKey"></a>
#### `public string ApiKey`

Your API key, required for all requests.

<a name="request+participantId"></a>
#### `public string ParticipantId`

Identifies state to the Web API and can persist across sessions.

<a name="request+BuildType"></a>
#### `public string BuildType`

Defaults to [EBuildType.Production](#e_build_type_1a2fd7fe2276119ccec8d74098e708a133).

<a name="request+ConversationId"></a>
#### `public string ConversationId`

Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.

<a name="request+Language"></a>
#### `public string Language`

ASR language; defaults to 'en-US'.

<a name="request+Locale"></a>
#### `public string Locale`

User locale; defaults to'en-US'.

<a name="request+TimezoneOffset"></a>
#### `public int TimezoneOffset`

A value in seconds representing the offset in UTC. For example, PST would be -28800.

<a name="request+RestartIfModified"></a>
#### `public bool RestartIfModified`

Restart this conversation if a newer version of the project has been published. Default value is true.

<a name="request+AccountId"></a>
#### `public string AccountId`

### Methods

<a name="request+ToString"></a>
#### `public override string ToString()`

Prints a JSON-like representation.

<a name="ebuildtype"></a>
# class EBuildType

The asset build type to request for Web API requests.

* [EBuildType.Production](#ebuildtype+Production) (default)
* [EBuildType.Staging](#ebuildtype+Staging)
* [EBuildType.Sandbox](#ebuildtypeSandbox)

### Properties

<a name="ebuildtype+Production"></a>
#### `public const string Production`

Contains raw value "production"

<a name="ebuildtype+Staging"></a>
#### `public const string Staging`

Contains raw value "staging"

<a name="ebuildtype+Sandbox"></a>
#### `public const string Sandbox`

Contains raw value "sandbox"


<a name="response"></a>
# class Response

Presents the output of a request to the [PullString](#namespace_pull_string) Web API.

### Summary

 Name | Type | Descriptions
----- | ---- | ------------ |
| [`Status`](#response+Status) | [`Status`](#status) | Represents status and any errors returned by Web API
| [`ConversationId`](#response+ConversationId) | `string` | Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.
| [`ParticipantId`](#response+ParticipantId) | `string` |  Identifies state to the Web API and can persist across sessions.
| [`ETag`](#response+ETag) | `string` |  Unique identifier for a version of the content.
| [`Outputs`](#response+Outputs) | [`Output`](#output)`[]` | Dialog or behaviors returned from the Web API.
| [`Entities`](#response+Entities) | [`Entity`](#entity)`[]` | Counters, flags, etc for the conversation.
| [`LastModified`](#response+LastModified) | `DateTime` | Time of content modification.
| [`TimedResponseInterval`](#response+TimedResponseInterval) | `double` | Indicates that there may be another response to process in the specified number of seconds. Set a timer and call checkForTimedResponse() from a conversation to retrieve it.
| [`AsrHypothesis`](#response+AsrHypothesis) | `string` |  The recognized speech, if audio has been submitted.
| [`Endpoint`](#response+Endpoint) | `string` |  The public endpoint for the current conversation.

* [Response](#response)
    * [Conversation(Dictionary< string, object> json)](#response+new)
    * [.ToString()](#response+ToString)

### Properties

<a name="response+Status"></a>
#### `public `[`Status`](#status)` Status`

Represents status and any errors returned by Web API

<a name="response+conversationId"></a>
#### `public string ConversationId`

Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.

<a name="response+ParticipantId"></a>
#### `public string ParticipantId`

Identifies state to the Web API and can persist across sessions.

<a name="response+ETag"></a>
#### `public string ETag`

Unique identifier for a version of the content.

<a name="response+Outputs"></a>
#### `public `[`Output`](#output)` [] Outputs`

Dialog or behaviors returned from the Web API.

<a name="response+Entities"></a>
#### `public `[`Entity`](#entity)` [] Entities`

Counters, flags, etc for the conversation.

<a name="response+LastModified"></a>
#### `public DateTime LastModified`

Time of content modification.

<a name="response+TimedResponseInterval"></a>
#### `public double TimedResponseInterval`

Indicates that there may be another response to process in the specified number of seconds. Set a timer and call checkForTimedResponse() from a conversation to retrieve it.

<a name="response+AsrHypothesis"></a>
#### `public string AsrHypothesis`

The recognized speech, if audio has been submitted.

<a name="response+Endpoint"></a>
#### `public string Endpoint`

The public endpoint for the current conversation.

### Methods

<a name="response+new"></a>
#### `public  Response(Dictionary<string, object> json)`

Constructs a `Response` object from JSON that has been parsed into a `Dictionary`

<a name="response+ToString"></a>
#### `public override string ToString()`

Prints a JSON-like representation.

<a name="dialogoutput"></a>
# class DialogOutput

```
class DialogOutput
  : public PullString.Output
```

Subclass of [Output](#output) that represents a dialog response

### Summary

 Name | Type | Description
----- | ---- | ----------- |
| [`Type`](#dialogoutput+Type) | `string` | [EOutputType.Dialog](#eoutputtype+Dialog) (read only)
| [`Text`](#dialogoutput+Text) | `string` | A character's text response.
| [`AudioUri`](#dialogoutput+AudioUri) | `string` | Location of recorded audio, if available.
| [`VideoUri`](#dialogoutput+VideoUri) | `string` | Location of recorded video, if available.
| [`Character`](#dialogoutput+Character) | `string` | The speaking character.
| [`Duration`](#dialogoutput+Duration) | `double` | Duration of spoken line in seconds.
| [`UserData`](#dialogoutput+UserData) | `string` | Optional arbitrary string data that was associated with the dialog line within [PullString](#namespace_pull_string) Author.
| [`Phonemes`](#dialogoutput+Phonemes) | [`Phoneme`](#phoneme)`[]` | Mouth shapes for the spoken line.

* [DialogOutput](#dialogoutput)
    * [DialogOutput(Dictionary< string, object > json)](#dialogoutput+new)
    * [.ToString()](#dialogoutput+ToString)

### Properties

<a name="dialogoutput+Type"></a>
#### `public override string Type`

[EOutputType.Dialog](#eoutputtype+Dialog) (read only)

<a name="dialogoutput+Text"></a>
#### `public string Text`

A character's text response.

<a name="dialogoutput+AudioUri"></a>
#### `public string AudioUri`

Location of recorded audio, if available.

<a name="dialogoutput+VideoUri"></a>
#### `public string VideoUri`

Location of recorded video, if available.

<a name="dialogoutput+Character"></a>
#### `public string Character`

The speaking character.

<a name="dialogoutput+Duration"></a>
#### `public double Duration`

Duration of spoken line in seconds.

<a name="dialogoutput+UserData"></a>
#### `public string UserData`

Optional arbitrary string data that was associated with the dialog line within PullString Author.

<a name="dialogoutput+Phonemes"></a>
#### `public `[`Phoneme`](#phoneme)` [] Phonemes`

Optional arbitrary string data that was associated with the dialog line within PullString Author.

### Methods

<a name="dialogoutput+new"></a>
#### `public  DialogOutput(Dictionary< string, object > json)`

Constructs a `DialogOutput` object from JSON that has been parsed into a `Dictionary`.

<a name="dialogoutput+ToString"></a>
#### `public override string ToString()`

Prints a JSON-like representation.


<a name="behavioroutput"></a>
# class BehaviorOutput

```
class BehaviorOutput
  : public PullString.Output
```

Subclass of [Output](#output) that represents a behavior response

### Summary

 Name | Type | Description
----- | ---- | ----------- |
`Type` | `string` | [EOutputType.Behavior](#eoutputtype+Behavior) (read only)
`Behavior` | `string` | The name of the behavior.
`Parameters` | `Dictionary<string, ParameterValue>` | A dictionary with any parameters defined for the behavior

* [BehaviorOutput](#behavioroutput)
    * [BehaviorOutput(Dictionary< string, object > json)](#behavioroutput+new)
    * [.ToString()](behavioroutput+ToString)

### Properties

<a name="behavioroutput+Type"></a>
#### `public override string Type`

[EOutputType.Behavior](#eoutputtype.Behavior) (read only)

<a name="behavioroutput+Behavior"></a>
#### `public string Behavior`

The name of the behavior.

<a name="behavioroutput+ParameterValue"></a>
#### `public Dictionary< string, ParameterValue > Parameters`

A dictionary with any parameters defined for the behavior

### Methods

<a name="behavioroutput+new"></a>
#### `public  BehaviorOutput(Dictionary< string, object > json)`

Construct a `BehaviorOutput` object from JSON that has been parsed into a `Dictionary`.

<a name="behavioroutput+ToString"></a>
#### `public override string ToString()`

Print a JSON-like representation.

<a name="output"></a>
# class Output

Base class for outputs that are of type dialog or behavior.


### Summary

| Name | Type | Description |
| ---- | ---- | ----------- |
| [`Type`](#output+Type) | `abstract string` | An [EOutputType](#eoutputtype), i.e. [EOutputType.Dialog](#eoutputtype+Dialog)
| [`Guid`](#output+Guid) | `string` | Unique identifier for this [Output](#output).

### Properties

<a name="output+Type"></a>
#### `public abstract string Type`

An [EOutputType](#e_output_type), i.e.

* [EOutputType.Dialog](#eoutputtype.Dialog)
* [EOutputType.Behavior](#eoutputtype.Behavior)

<a name="behavioroutput+Guid"></a>
#### `public string Guid`

Unique identifier for this [Output](#output).

<a name="eoutputtype"></a>
# class EOutputType

Define the set of outputs that can be returned in a response.

* [EOutputType.Dialog](#eoutputtype.Dialog)
* [EOutputType.Behavior](#eoutputtype.Behavior)

#### Summary

| Name | Type | Description |
| ---- | ---- | ----------- |
| [`Dialog`](#eoutputtype+Dialog) | `string` | "dialog"
| [`Behavior`](#eoutputtype+Behavior) | `string` | "behavior"

### Properties

<a name="eoutputtype+Dialog"></a>
#### `public const string Dialog`

Contains raw value "dialog"

<a name="eoutputtype+Behavior"></a>
#### `public const string Behavior`

Contains raw value "behavior"


<a name="counter"></a>
# class Counter

```
class Counter
  : public PullString.Entity
```

Subclass of [Entity](#entity) to describe a single counter.

### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`Type`](#counter+Type) | `string` | [EEntityType.Counter](#eentitytype+Counter) (read only)
| [`DoubleValue`](#counter+DoubleValue) | `double` | Safely cast Value as a double

* [Counter](#counter)
    * [Counter(string name, double val)](#counter+new)

### Properties

<a name="counter+Type"></a>
#### `public override string Type`

[EEntityType.Counter](#eentitytype+Counter) (read only)

<a name="counter+DoubleValue"></a>
#### `public double DoubleValue`

Safely cast Value as a double

### Methods

<a name="counter+new"></a>
#### `public  Counter(string name, double val)`


<a name="flag"></a>
# class Flag

```
class Flag
  : public PullString.Entity
```

Subclass of [Entity](#entity) to describe a single [Flag](#flag).


### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`Type`](#flag+Label) | `string` | [EEntityType.Flag](#eentitytype+Flag) (read only)
| [`BoolValue`](#flag+BoolValue) | `bool` |  Safely cast Value as a bool.

* [Flag](#flag)
    * [Flag(string name,bool val)](#flag+new)

### Properties

<a name="flag+Type"></a>
#### `public override string Type`

[EEntityType.Flag](#eentitytype+Flag) (read only)

<a name="flag+BoolValue"></a>
#### `public bool BoolValue`

Safely cast Value as a bool.

### Methods

<a name="flag+new"></a>
#### `public  Flag(string name,bool val)`


<a name="label"></a>
# class Label

```
class Label
  : public PullString.Entity
```

Subclass of [Entity](#entity) to describe a single [Label](#label).

### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`Type`](#label+Type) | `string` | [EEntityType.Label](#eentitytype+Label) (read only)
| [`StringValue`](#label+StringValue) | `string` | Safely cast Value object as string.

* [Label](#label)
    * [Label(string name,string val)](#label+new)

### Members

<a name="label+Type"></a>
#### `public override string Type`

[EEntityType.Label](#eentitytype+Label) (read only)

<a name="label+StringValue"></a>
#### `public string StringValue`

Safely cast Value object as string.

### Methods

<a name="label+new"></a>
#### `public  Label(string name,string val)`

<a name="list"></a>
# class List
[//]: # (list)

```
class List
  : public PullString.Entity
```

Subclass of [Entity](#entity) to describe a single [List](#list).


### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`Type`](#list+Type) | `string` | [EEntityType.List](#eentitytype+List) (read only)
| [`ArrayValue`](#label+ArrayValue) | `object[]` | Safely cast Value object as an object[].

* [List](#list)
    * [List(string name, object [] val)](#list+new)

### Properties

<a name="list+Type"></a>
#### `public override string Type`

[EEntityType.List](#eentitytype+List) (read only)

<a name="list+ArrayValue"></a>
#### `public object [] ArrayValue`

Safely cast Value as an object[]

### Methods

<a name="list+new"></a>
#### `public  List(string name,object [] val)`


<a name="entity"></a>
# class Entity

Base class to describe a single entity, such as a label, counter, flag, or list.

### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`Type`](#entity+Type) | `abstract string` | [EEntityType](#e_entity_type) for this object, i.e. [EEntityType.Label](#eentitytype+Label)
| [`Name`](#entity+Name) | `string` | Descriptive name of entity.
| [`Value`](#entity+Value) | `object` | The value of the entity, the type of which will vary depending on the subclass.

* [Entity](#entity)
    * [.ToString()](#entity+ToString)

### Properties

<a name="entity+Type"></a>
#### `public abstract string Type`

[EEntityType](#eentitytype) for this object, i.e.

* [EEntityType.Label](#eentitytype+Label)
* [EEntityType.Counter](#eentitytype+Counter)
* [EEntityType.Flag](#eentitytype+Flag)
* [EEntityType.List](#eentitytype+List)

<a name="entity+Name"></a>
#### `public string Name`

Descriptive name of entity.

<a name="entity+Value"></a>
#### `public object Value`

The value of the entity, the type of which will vary depending on the subclass.

* [Label](#label): string
* [Counter](#counter): double
* [Flag](#flag): bool
* [List](#list): object[]

### Methods

<a name="entity+ToString></a>
#### `public override string ToString()`


<a name="eentitytype"></a>
# class EEntityType

Define the list of entity types

* [EEntityType.Label](#eentitytype+Label)
* [EEntityType.Counter](#eentitytype+Counter)
* [EEntityType.Flag](#eentitytype+Flag)
* [EEntityType.List](#eentitytype+List)

### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [ `Label`](#eentitytype+Label) | `string` | "label"
| [ `Counter`](#eentitytype+Counter) | `string` | "counter"
| [ `Flag`](#eentitytype+Flag) | `string` | "flag"
| [ `List`](#eentitytype+List) | `string` | "list"

### Properties

<a name="eentitytype+Label"></a>
#### `public const string Label`

Contains raw value "label"

<a name="eentitytype+Counter"></a>
#### `public const string Counter`

Contains raw value "counter"

<a name="eentitytype+Flag"></a>
#### `public const string Flag`

Contains raw value "flag"

<a name="eentitytype+List"></a>
#### `public const string List`

Contains raw value "list"

<a name="phoneme"></a>
# class Phoneme

Describe a single phoneme for an audio response, e.g., to drive automatic lip sync.

### Summary

| Name | Type |
| ---- | ---- |
| [`Name`](#phoneme+Name) | `string` |
| [`SecondsSinceStart`](#phoneme+SecondsSinceStart) | `double`

* [Phonome](#phoneme)
    * [Phoneme(Dictionary<string, object> json)](#phoneme+new)
    * [.ToString()](#phoneme+ToString)

### Properties

<a name="phoneme+Name"></a>
#### `public string Name`

<a name="phoneme+SecondsSinceStart"></a>
#### `public double SecondsSinceStart`

### Methods

<a name="phoneme+new"></a>
#### `public  Phoneme(Dictionary< string, object > json)`

<a name="phoneme+ToString"></a>
#### `public override string ToString()`

Prints a JSON-like representation


<a name="status"></a>
# class Status

Describe the status and any errors from a Web API response.

### Summary

| Name | Type |
| ---- | ---- |
| [`Success`](#status+Success) | `bool`
| [`StatusCode`](#status+StatusCode) | `long`
| [`ErrorMessage`](#status+ErrorMessage) | `string`

* [Status](#status)
    * [ToString()](#phoneme+ToString)

### Properties

<a name="status+Sucess"></a>
#### `public bool Success`

<a name="status+StatusCode"></a>
#### `public long StatusCode`

<a name="status+ErrorMessage"></a>
#### `public string ErrorMessage`

### Methods

<a name="status+ToString"></a>
#### `public override string ToString()`

Prints a JSON-like representation


<a name="versioninfo"></a>
# class VersionInfo

Encapsulates version information for [PullString](#namespace_pullstring)'s Web API.

### Summary

| Name | Type | Description
| ---- | ---- |------------ |
| [`ApiBaseUrl`](#versioninfo+ApiBaseUrl) | `string` | The public-facing endpoint of the [PullString](#namespace_pullstring) Web API.

* [VersionInfo](#versioninfo)
    * [HasFeature](#versioninfo+HasFeature)

### Properties

<a name="versioninfo+ApiBaseUrl"></a>
#### `public const string ApiBaseUrl`

The public-facing endpoint of the [PullString](#namespace_pullstring) Web API.

### Methods

<a name="versioninfo+HasFeature"></a>
#### `public static bool HasFeature`

Check if the SDK currently supports a feature.

**Returns** `true` if the feature is supported.

