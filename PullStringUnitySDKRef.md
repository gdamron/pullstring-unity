# namespace PullString



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`class `[``BehaviorOutput``](#class_pull_string_1_1_behavior_output)    | Subclass of [Output](#class_pull_string_1_1_output) that represents a behavior response
`class `[``Conversation``](#class_pull_string_1_1_conversation)    | The [Conversation](#class_pull_string_1_1_conversation) class can be used to interface with the [PullString](#namespace_pull_string) API.
`class `[``Counter``](#class_pull_string_1_1_counter)    | Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single counter.
`class `[``DialogOutput``](#class_pull_string_1_1_dialog_output)    | Subclass of [Output](#class_pull_string_1_1_output) that represents a dialog response
`class `[``EBuildType``](#class_pull_string_1_1_e_build_type)    | The asset build type to request for Web API requests.
`class `[``EEntityType``](#class_pull_string_1_1_e_entity_type)    | Define the list of entity types
`class `[``Entity``](#class_pull_string_1_1_entity)    | Base class to describe a single entity, such as a label, counter, flag, or list.
`class `[``EOutputType``](#class_pull_string_1_1_e_output_type)    | Define the set of outputs that can be returned in a response.
`class `[``Flag``](#class_pull_string_1_1_flag)    | Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [Flag](#class_pull_string_1_1_flag).
`class `[``Label``](#class_pull_string_1_1_label)    | Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [Label](#class_pull_string_1_1_label).
`class `[``List``](#class_pull_string_1_1_list)    | Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [List](#class_pull_string_1_1_list).
`class `[``Output``](#class_pull_string_1_1_output)    | Base class for outputs that are of type dialog or behavior.
`class `[``Phoneme``](#class_pull_string_1_1_phoneme)    | Describe a single phoneme for an audio response, e.g., to drive automatic lip sync.
`class `[``Request``](#class-request)    | Describe the parameters for a request to the [PullString](#namespace_pull_string) Web API.
`class `[``Response``](#class_pull_string_1_1_response)    | Presents the output of a request to the [PullString](#namespace_pull_string) Web API.
`class `[``Status``](#class_pull_string_1_1_status)    | Describe the status and any errors from a Web API response.
`class `[``VersionInfo``](#class-versioninfo)    | Encapsulates version information for [PullString](#namespace_pull_string)'s Web API.

# class Conversation 
[//]: # (class_pull_string_1_1_conversation)

```
class Conversation
  : public MonoBehaviour
```  

The [Conversation](#class_pull_string_1_1_conversation) class can be used to interface with the [PullString](#namespace_pull_string) API.

To begin a conversation, call the [Begin()](#class_pull_string_1_1_conversation_1ac6621cb8bdf0a3285108f44c1233e1ec) method, providing a [PullString](#namespace_pull_string) project ID and a [Request](#class_pull_string_1_1_request) object specifying your API key.

The Web API returns a [Response](#class_pull_string_1_1_response) object that can contain zero or more outputs, such as lines of dialog or behaviors. This [Response](#class_pull_string_1_1_response) object is passed to the callback as its sole parameter. 


```cs
// create a Request with valid API key
var request = new Request() {
    ApiKey = MY_API_KEY
};

// create conversation and reqister response delegate
var conversation = new Conversation();
conversation.OnResponseReceived += (Response response) =>
{
    // Debug.Log(response);
}

// start conversation
conversation.Begin(MY_PROJECT, request);
```

### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public string ConversationId` | The current conversation ID. [Conversation](#class_pull_string_1_1_conversation) IDs can persist across sessions, if desired.
`public string ParticipantId` | Get the current participant ID, which identifies the current state for clients. This can persist accross sessions, if desired.
`package string Endpoint` | 
`public delegate void ResponseDelegate(`[`Response`](#class_pull_string_1_1_response)` response)` | The signature for a function that receives reseponse from the [PullString](#namespace_pull_string) Web API.
`public void Begin(string project,`[`Request`](#class_pull_string_1_1_request)` request)` | Start a new conversation with the Web API and receive a response via the OnResponseReceived event.
`public void SendText(string text,`[`Request`](#class_pull_string_1_1_request)` request)` | Send user input text to the Web API and receive a response via the OnResponseReceived event.
`public void SendActivity(string activity,`[`Request`](#class_pull_string_1_1_request)` request)` | Send an activity name or ID to the Web API and receive a response via the OnResponseReceived event.
`public void SendEvent(string eventName,Dictionary< string, object > parameters,`[`Request`](#class_pull_string_1_1_request)` request)` | Send an event to the Web API and receive a response via the OnResponseReceived event.
`public void GoTo(string responseId,`[`Request`](#class_pull_string_1_1_request)` request)` | Jump the conversation directly to a response.
`public void CheckForTimedResponse(`[`Request`](#class_pull_string_1_1_request)` request)` | Call the Web API to see if there is a time-based response to process. You nly need to call this if the previous response returned a value for the timedResponseInterval >= 0. In this case, set a timer for that value (in seconds) and then call this method. If there is no time-based response, OnResponseReceived will be passed an empty [Response](#class_pull_string_1_1_response) object.
`public void GetEntities(string [] names,`[`Request`](#class_pull_string_1_1_request)` request)` | [Request](#class_pull_string_1_1_request) the values of the specified entities (i.e.: labels, counters, flags, and lists) from the Web API.
`public void SetEntities(`[`Entity`](#class_pull_string_1_1_entity)` [] entities,`[`Request`](#class_pull_string_1_1_request)` request)` | Change the value of the specified entities (i.e.: labels, counters, flags, and lists) via the Web API.
`public void SendAudio(AudioClip clip,`[`Request`](#class_pull_string_1_1_request)` request)` | Send an entire audio sample of the user speaking to the Web API. Audio must be, mono 16-bit linear PCM at a sample rate of 16000 samples per second.
`public void StartAudio(`[`Request`](#class_pull_string_1_1_request)` request)` | Initiate a progressive (chunked) streaming of audio data, where supported.
`public void AddAudio(float [] audio)` | Add a chunk of raw audio samples. You must call [StartAudio()](#class_pull_string_1_1_conversation_1acc0d905c4c8d89f6b3da72f73a7bb29b) first. The format of the audio must be mono linear PCM audio data at a sample rate of 16000 samples per second.
`public void EndAudio()` | Signal that all audio has been provided via [AddAudio()](#class_pull_string_1_1_conversation_1a9776d0733f135dcf4cc7e54192e62e4f) calls. This will complete the audio request and return the Web API response via the OnResponseReceived event.

### Members

#### `public string ConversationId` 
[//]: # (class_pull_string_1_1_conversation_1ac1338fcf53ce11f6ad51f9f109348d2c)

The current conversation ID. [Conversation](#class_pull_string_1_1_conversation) IDs can persist across sessions, if desired.

#### Returns
The current conversation ID

#### `public string ParticipantId` 
[//]: # (class_pull_string_1_1_conversation_1a0fae8cef9bd97b77e82ffffd3b06d1c0)

Get the current participant ID, which identifies the current state for clients. This can persist accross sessions, if desired.

#### Returns
The current participant ID.

#### `package string Endpoint` 
[//]: # (class_pull_string_1_1_conversation_1a32c1a96f61a9470d3daebf07ba112c9a)





#### `public delegate void ResponseDelegate(`[`Response`](#class_pull_string_1_1_response)` response)` 
[//]: # (class_pull_string_1_1_conversation_1a389f217e3ca2934f153aecfb10329cef)

The signature for a function that receives reseponse from the [PullString](#namespace_pull_string) Web API.

```cs
void OnPullstringResponse(PullString.Response reseponse)
{
    // handle response...
}
```



#### Parameters
* `response` A [PullString.Response](#class_pull_string_1_1_response) object from the Web API

#### `public void Begin(string project,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1ac6621cb8bdf0a3285108f44c1233e1ec)

Start a new conversation with the Web API and receive a response via the OnResponseReceived event.

```cs
var request = new Request () {
    ApiKey = MY_API_KEY
};
conversation.Begin(MY_PROJECT, request);
```



#### Parameters
* `project` The PullSring project ID.


* `request` A [Request](#class_pull_string_1_1_request) object with a valid ApiKey value set

#### `public void SendText(string text,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1ae4ae5cab1adf63c3c24ebb3121bc2339)

Send user input text to the Web API and receive a response via the OnResponseReceived event.

#### Parameters
* `text` User input text.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void SendActivity(string activity,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a5e6b8d1a125b5388bde7432eaf6db9d2)

Send an activity name or ID to the Web API and receive a response via the OnResponseReceived event.

#### Parameters
* `activity` The activity name or ID.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void SendEvent(string eventName,Dictionary< string, object > parameters,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a90f6a50f6bfe5ef2b59ff1bb2b640cc5)

Send an event to the Web API and receive a response via the OnResponseReceived event.

#### Parameters
* `eventName` The event name.


* `parameters` Any accompanying paramters.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void GoTo(string responseId,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a37d203ecae25e61d417e9da1ba279e84)

Jump the conversation directly to a response.

#### Parameters
* `responseId` The UUID of the response to jump to.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void CheckForTimedResponse(`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a4c4311f106b97475d91f299cc2fca2b1)

Call the Web API to see if there is a time-based response to process. You nly need to call this if the previous response returned a value for the timedResponseInterval >= 0. In this case, set a timer for that value (in seconds) and then call this method. If there is no time-based response, OnResponseReceived will be passed an empty [Response](#class_pull_string_1_1_response) object.

#### Parameters
* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void GetEntities(string [] names,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a84c1ef2437a651be618d3054506c90fc)

[Request](#class_pull_string_1_1_request) the values of the specified entities (i.e.: labels, counters, flags, and lists) from the Web API.

#### Parameters
* `names` An array of entity names.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void SetEntities(`[`Entity`](#class_pull_string_1_1_entity)` [] entities,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a422ace9283902adf6cb883bd1234e401)

Change the value of the specified entities (i.e.: labels, counters, flags, and lists) via the Web API.

#### Parameters
* `entities` An array specifying the entities to set (with their new values).


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void SendAudio(AudioClip clip,`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1a95c6266f732d72734272bd81acd49b86)

Send an entire audio sample of the user speaking to the Web API. Audio must be, mono 16-bit linear PCM at a sample rate of 16000 samples per second.

#### Parameters
* `clip` Mono 16-bit linear PCM audio clip at 16k Hz.


* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void StartAudio(`[`Request`](#class_pull_string_1_1_request)` request)` 
[//]: # (class_pull_string_1_1_conversation_1acc0d905c4c8d89f6b3da72f73a7bb29b)

Initiate a progressive (chunked) streaming of audio data, where supported.

Note, chunked streaming is not currently implemented, so this will batch up all audio and send it all at once when [EndAudio()](#class_pull_string_1_1_conversation_1af96afb8a51c9a6e8021ed7bbd8afe0d3) is called. 


#### Parameters
* `request` [Optional] A request object with at least apiKey and conversationId set.

#### `public void AddAudio(float [] audio)` 
[//]: # (class_pull_string_1_1_conversation_1a9776d0733f135dcf4cc7e54192e62e4f)

Add a chunk of raw audio samples. You must call [StartAudio()](#class_pull_string_1_1_conversation_1acc0d905c4c8d89f6b3da72f73a7bb29b) first. The format of the audio must be mono linear PCM audio data at a sample rate of 16000 samples per second.

#### Parameters
* `audio`

#### `public void EndAudio()` 
[//]: # (class_pull_string_1_1_conversation_1af96afb8a51c9a6e8021ed7bbd8afe0d3)

Signal that all audio has been provided via [AddAudio()](#class_pull_string_1_1_conversation_1a9776d0733f135dcf4cc7e54192e62e4f) calls. This will complete the audio request and return the Web API response via the OnResponseReceived event.




# class Request 
[//]: # (class_pull_string_1_1_request)


Describe the parameters for a request to the [PullString](#namespace_pull_string) Web API.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public string ApiKey` | Your API key, required for all requests.
`public string ParticipantId` | Identifies state to the Web API and can persist across sessions.
`public string BuildType` | Defaults to [EBuildType.Production](#class_pull_string_1_1_e_build_type_1a2fd7fe2276119ccec8d74098e708a133).
`public string ConversationId` | Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.
`public string Language` | ASR language; defaults to 'en-US'.
`public string Locale` | User locale; defaults to'en-US'.
`public int TimezoneOffset` | A value in seconds representing the offset in UTC. For example, PST would be -28800.
`public bool RestartIfModified` | Restart this conversation if a newer version of the project has been published. Default value is true.
`public string AccountId` | 
`public  Request()` | 
`public override string ToString()` | 

### Members

#### `public string ApiKey` 
[//]: # (class_pull_string_1_1_request_1a0e48696f8e1ff2b29dffcbd31110411b)

Your API key, required for all requests.



#### `public string ParticipantId` 
[//]: # (class_pull_string_1_1_request_1ab63bfe9b1887cdda80ab4252a738e90b)

Identifies state to the Web API and can persist across sessions.



#### `public string BuildType` 
[//]: # (class_pull_string_1_1_request_1a6d00c58c5ad28985db7ac73dac64af43)

Defaults to [EBuildType.Production](#class_pull_string_1_1_e_build_type_1a2fd7fe2276119ccec8d74098e708a133).



#### `public string ConversationId` 
[//]: # (class_pull_string_1_1_request_1aa97c9f051af135107057da4dde93d1f8)

Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.



#### `public string Language` 
[//]: # (class_pull_string_1_1_request_1abba55140e1f426e7fa09553f276de554)

ASR language; defaults to 'en-US'.



#### `public string Locale` 
[//]: # (class_pull_string_1_1_request_1ae0a8f724ac2bef1b7349bd5d102bbe03)

User locale; defaults to'en-US'.



#### `public int TimezoneOffset` 
[//]: # (class_pull_string_1_1_request_1aa4bfb1b1397377b6fa781c28b3e9e827)

A value in seconds representing the offset in UTC. For example, PST would be -28800.



#### `public bool RestartIfModified` 
[//]: # (class_pull_string_1_1_request_1aa1ef38a8d144aae24ca17e0d2486bb5e)

Restart this conversation if a newer version of the project has been published. Default value is true.



#### `public string AccountId` 
[//]: # (class_pull_string_1_1_request_1ae59f082a00245c9c8ce5a9e3320df16a)





#### `public  Request()` 
[//]: # (class_pull_string_1_1_request_1a27645c1a025a5323e219b6329321767a)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_request_1a9936cac35153afb78c4969ec1b620732)






# class EBuildType 
[//]: # (class_pull_string_1_1_e_build_type)


The asset build type to request for Web API requests.

* [EBuildType.Production](#class_pull_string_1_1_e_build_type_1a2fd7fe2276119ccec8d74098e708a133) (default)


* [EBuildType.Staging](#class_pull_string_1_1_e_build_type_1a36157ea9cc92204cb61e9c2b66d2247c)


* [EBuildType.Sandbox](#class_pull_string_1_1_e_build_type_1a6ce16a6a4578a4f1ac0c19b8155ba6b1) /summary>

### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public const string Production` | 
`public const string Staging` | 
`public const string Sandbox` | 

### Members

#### `public const string Production` 
[//]: # (class_pull_string_1_1_e_build_type_1a2fd7fe2276119ccec8d74098e708a133)





#### `public const string Staging` 
[//]: # (class_pull_string_1_1_e_build_type_1a36157ea9cc92204cb61e9c2b66d2247c)





#### `public const string Sandbox` 
[//]: # (class_pull_string_1_1_e_build_type_1a6ce16a6a4578a4f1ac0c19b8155ba6b1)






# class Response 
[//]: # (class_pull_string_1_1_response)


Presents the output of a request to the [PullString](#namespace_pull_string) Web API.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public `[`Status`](#class_pull_string_1_1_status)` Status` | Represents status and any errors returned by Web API
`public string ConversationId` | Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.
`public string ParticipantId` | Identifies state to the Web API and can persist across sessions.
`public string ETag` | Unique identifier for a version of the content.
`public `[`Output`](#class_pull_string_1_1_output)` [] Outputs` | Dialog or behaviors returned from the Web API.
`public `[`Entity`](#class_pull_string_1_1_entity)` [] Entities` | Counters, flags, etc for the converation.
`public DateTime LastModified` | Time of content modification.
`public double TimedResponseInterval` | Indicates that there may be another response to process in the specified number of seconds. Set a timer and call checkForTimedResponse() from a conversation to retrieve it.
`public string AsrHypothesis` | The recognized speech, if audio has been submitted.
`public string Endpoint` | The public endpoint for the current conversation.
`public  Response()` | 
`public  Response(Dictionary< string, object > json)` | 
`public override string ToString()` | 

### Members

#### `public `[`Status`](#class_pull_string_1_1_status)` Status` 
[//]: # (class_pull_string_1_1_response_1acc4e50d5c0adc4d7d006ed734842cf83)

Represents status and any errors returned by Web API



#### `public string ConversationId` 
[//]: # (class_pull_string_1_1_response_1aa411ca281644c99eadd4f8a9d5b2152e)

Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a conversation is started.



#### `public string ParticipantId` 
[//]: # (class_pull_string_1_1_response_1ab7ed05a2acc115c80f7b5d2ac2d5de92)

Identifies state to the Web API and can persist across sessions.



#### `public string ETag` 
[//]: # (class_pull_string_1_1_response_1a772aa21fe57d7601a286c686f33a20ae)

Unique identifier for a version of the content.



#### `public `[`Output`](#class_pull_string_1_1_output)` [] Outputs` 
[//]: # (class_pull_string_1_1_response_1a151a5d261c9e3b4a22e5bfd8fc946ea6)

Dialog or behaviors returned from the Web API.



#### `public `[`Entity`](#class_pull_string_1_1_entity)` [] Entities` 
[//]: # (class_pull_string_1_1_response_1a84394aadb716230f49f8a0e55c734a18)

Counters, flags, etc for the converation.



#### `public DateTime LastModified` 
[//]: # (class_pull_string_1_1_response_1acab537d309987c23cea81f12bb757908)

Time of content modification.



#### `public double TimedResponseInterval` 
[//]: # (class_pull_string_1_1_response_1a5cd5e121542005eaa0f4df627e25d844)

Indicates that there may be another response to process in the specified number of seconds. Set a timer and call checkForTimedResponse() from a conversation to retrieve it.



#### `public string AsrHypothesis` 
[//]: # (class_pull_string_1_1_response_1aaa19d9197dc1cd4ff7f80f0ad4510fa9)

The recognized speech, if audio has been submitted.



#### `public string Endpoint` 
[//]: # (class_pull_string_1_1_response_1a6c807792bf27ab0d34a85786bb134994)

The public endpoint for the current conversation.

#### Returns

#### `public  Response()` 
[//]: # (class_pull_string_1_1_response_1a1c394f1ae04e6eec37ef3246d3ca3134)





#### `public  Response(Dictionary< string, object > json)` 
[//]: # (class_pull_string_1_1_response_1aba2bc862d2db7322a0955160a1a48495)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_response_1a6e1ac9274eb03642bb4746a9a4dc6aab)






# class DialogOutput 
[//]: # (class_pull_string_1_1_dialog_output)

```
class DialogOutput
  : public PullString.Output
```  

Subclass of [Output](#class_pull_string_1_1_output) that represents a dialog response



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EOutputType.Dialog](#class_pull_string_1_1_e_output_type_1a4657e1c524f87cde94427332abd4af66)
`public string Text` | A character's text response.
`public string AudioUri` | Location of recorded audio, if available.
`public string VideoUri` | Location of recorded video, if available.
`public string Character` | The speaking character.
`public double Duration` | Duration of spoken line in seconds.
`public string UserData` | Optional arbitrary string data that was associated with the dialog line within [PullString](#namespace_pull_string) Author.
`public `[`Phoneme`](#class_pull_string_1_1_phoneme)` [] Phonemes` | Optional arbitrary string data that was associated with the dialog line within [PullString](#namespace_pull_string) Author.
`public  DialogOutput(Dictionary< string, object > json)` | 
`public  DialogOutput()` | 
`public override string ToString()` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_dialog_output_1a14cffce9cef1773febf94aed768c039a)

[EOutputType.Dialog](#class_pull_string_1_1_e_output_type_1a4657e1c524f87cde94427332abd4af66)



#### `public string Text` 
[//]: # (class_pull_string_1_1_dialog_output_1a050d7fd7672257280f2019721ec20184)

A character's text response.



#### `public string AudioUri` 
[//]: # (class_pull_string_1_1_dialog_output_1a7bc66f8c18c98bedcf93d1506bbb93f3)

Location of recorded audio, if available.



#### `public string VideoUri` 
[//]: # (class_pull_string_1_1_dialog_output_1ab1f3599d54cdbdb37d189989812ffc8a)

Location of recorded video, if available.



#### `public string Character` 
[//]: # (class_pull_string_1_1_dialog_output_1a0a353f68258a6d5fa28333781167efa9)

The speaking character.



#### `public double Duration` 
[//]: # (class_pull_string_1_1_dialog_output_1ae9094bf47fee983729db26735dd93e2d)

Duration of spoken line in seconds.



#### `public string UserData` 
[//]: # (class_pull_string_1_1_dialog_output_1af22439a51fe41792f617ecd595b06368)

Optional arbitrary string data that was associated with the dialog line within [PullString](#namespace_pull_string) Author.



#### `public `[`Phoneme`](#class_pull_string_1_1_phoneme)` [] Phonemes` 
[//]: # (class_pull_string_1_1_dialog_output_1af157310a04216312462f4c497da6c43a)

Optional arbitrary string data that was associated with the dialog line within [PullString](#namespace_pull_string) Author.



#### `public  DialogOutput(Dictionary< string, object > json)` 
[//]: # (class_pull_string_1_1_dialog_output_1ab04d7832f5a1c1de7587627ba08ebeec)





#### `public  DialogOutput()` 
[//]: # (class_pull_string_1_1_dialog_output_1a855637d9c65b302a1125f64ce9dae362)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_dialog_output_1a8b53435712fce8bab80db49414bb344d)






# class BehaviorOutput 
[//]: # (class_pull_string_1_1_behavior_output)

```
class BehaviorOutput
  : public PullString.Output
```  

Subclass of [Output](#class_pull_string_1_1_output) that represents a behavior response



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EOutputType.Behavior](#class_pull_string_1_1_e_output_type_1a9a2b3446847c113607ba01af659e5957)
`public string Behavior` | The name of the behavior.
`public Dictionary< string, ParameterValue > Parameters` | A dictionary with any parameters defined for the behavior
`public  BehaviorOutput(Dictionary< string, object > json)` | 
`public  BehaviorOutput()` | 
`public override string ToString()` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_behavior_output_1a6eed6e1a80200be8cdcbc167e1fb3f63)

[EOutputType.Behavior](#class_pull_string_1_1_e_output_type_1a9a2b3446847c113607ba01af659e5957)



#### `public string Behavior` 
[//]: # (class_pull_string_1_1_behavior_output_1a0feecfc29d36cdbc841b064e625ab49a)

The name of the behavior.



#### `public Dictionary< string, ParameterValue > Parameters` 
[//]: # (class_pull_string_1_1_behavior_output_1a16f3f16ffe8a346bc46e8e800ea08830)

A dictionary with any parameters defined for the behavior



#### `public  BehaviorOutput(Dictionary< string, object > json)` 
[//]: # (class_pull_string_1_1_behavior_output_1a1f2f00eb8679ca89819ccf6a6bdd93f7)





#### `public  BehaviorOutput()` 
[//]: # (class_pull_string_1_1_behavior_output_1a8662069fa041e9351517b978dfa20bad)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_behavior_output_1aa9b2e637f126641b7cbd9eaac58b33ca)






# class Output 
[//]: # (class_pull_string_1_1_output)


Base class for outputs that are of type dialog or behavior.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public abstract string Type` | An [EOutputType](#class_pull_string_1_1_e_output_type), i.e.
`public string Guid` | Unique identifier for this [Output](#class_pull_string_1_1_output).

### Members

#### `public abstract string Type` 
[//]: # (class_pull_string_1_1_output_1ab88fcedac23bd2ade0cbe531e6645bed)

An [EOutputType](#class_pull_string_1_1_e_output_type), i.e.

* [EOutputType.Dialog](#class_pull_string_1_1_e_output_type_1a4657e1c524f87cde94427332abd4af66)


* [EOutputType.Behavior](#class_pull_string_1_1_e_output_type_1a9a2b3446847c113607ba01af659e5957)

#### `public string Guid` 
[//]: # (class_pull_string_1_1_output_1a222725eae1edbaf11a7898f68e701926)

Unique identifier for this [Output](#class_pull_string_1_1_output).




# class EOutputType 
[//]: # (class_pull_string_1_1_e_output_type)


Define the set of outputs that can be returned in a response.

* [EOutputType.Dialog](#class_pull_string_1_1_e_output_type_1a4657e1c524f87cde94427332abd4af66)


* [EOutputType.Behavior](#class_pull_string_1_1_e_output_type_1a9a2b3446847c113607ba01af659e5957)

### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public const string Dialog` | 
`public const string Behavior` | 

### Members

#### `public const string Dialog` 
[//]: # (class_pull_string_1_1_e_output_type_1a4657e1c524f87cde94427332abd4af66)





#### `public const string Behavior` 
[//]: # (class_pull_string_1_1_e_output_type_1a9a2b3446847c113607ba01af659e5957)






# class Counter 
[//]: # (class_pull_string_1_1_counter)

```
class Counter
  : public PullString.Entity
```  

Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single counter.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EEntityType.Counter](#class_pull_string_1_1_e_entity_type_1aa001281f4cf8a4d6f4929192033796ee)
`public double DoubleValue` | Safely cast Value as a double
`public  Counter(string name,double val)` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_counter_1ae31af92b304f583cf806fc84bce6ebe3)

[EEntityType.Counter](#class_pull_string_1_1_e_entity_type_1aa001281f4cf8a4d6f4929192033796ee)



#### `public double DoubleValue` 
[//]: # (class_pull_string_1_1_counter_1acc6762f3af50569b9ce78828c9f46f75)

Safely cast Value as a double



#### `public  Counter(string name,double val)` 
[//]: # (class_pull_string_1_1_counter_1aa1fbb5add4cdf6ad5d087cd3102d0bb9)






# class Flag 
[//]: # (class_pull_string_1_1_flag)

```
class Flag
  : public PullString.Entity
```  

Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [Flag](#class_pull_string_1_1_flag).



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EEntityType.Flag](#class_pull_string_1_1_e_entity_type_1a6c368bb9f034b90d9ed1f124ac787d38)
`public bool BoolValue` | Safely cast Value as a bool.
`public  Flag(string name,bool val)` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_flag_1a232fc768a399f9eb35af941ff91ded84)

[EEntityType.Flag](#class_pull_string_1_1_e_entity_type_1a6c368bb9f034b90d9ed1f124ac787d38)



#### `public bool BoolValue` 
[//]: # (class_pull_string_1_1_flag_1ace90f095a599cafe3db174f9d51f12d2)

Safely cast Value as a bool.



#### `public  Flag(string name,bool val)` 
[//]: # (class_pull_string_1_1_flag_1a72d4bbcca37f8f6cd49f1a5a7ad4680c)






# class Label 
[//]: # (class_pull_string_1_1_label)

```
class Label
  : public PullString.Entity
```  

Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [Label](#class_pull_string_1_1_label).



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EEntityType.Label](#class_pull_string_1_1_e_entity_type_1adaf958316d9e20869a6791e3096de0ea)
`public string StringValue` | Safely cast Value object as string.
`public  Label(string name,string val)` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_label_1ac329c58713763a68e08c6cfbf8c21e64)

[EEntityType.Label](#class_pull_string_1_1_e_entity_type_1adaf958316d9e20869a6791e3096de0ea)



#### `public string StringValue` 
[//]: # (class_pull_string_1_1_label_1a1a5be6473403ef0664c7546d6f7b40be)

Safely cast Value object as string.



#### `public  Label(string name,string val)` 
[//]: # (class_pull_string_1_1_label_1a5da40e4ba1cc8570573a0178cc639015)






# class List 
[//]: # (class_pull_string_1_1_list)

```
class List
  : public PullString.Entity
```  

Subclass of [Entity](#class_pull_string_1_1_entity) to describe a single [List](#class_pull_string_1_1_list).



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public override string Type` | [EEntityType.List](#class_pull_string_1_1_e_entity_type_1a06a23f98a47b02734364519d0e763510)
`public object [] ArrayValue` | Safely cast Value as an object[]
`public  List(string name,object [] val)` | 

### Members

#### `public override string Type` 
[//]: # (class_pull_string_1_1_list_1a8f374f8cecc5a1c0b97ed9e84471bc29)

[EEntityType.List](#class_pull_string_1_1_e_entity_type_1a06a23f98a47b02734364519d0e763510)



#### `public object [] ArrayValue` 
[//]: # (class_pull_string_1_1_list_1aa6826113fbc76a875e030e53e87f5cea)

Safely cast Value as an object[]



#### `public  List(string name,object [] val)` 
[//]: # (class_pull_string_1_1_list_1a857336c69db43ca5b92c56189940d095)






# class Entity 
[//]: # (class_pull_string_1_1_entity)


Base class to describe a single entity, such as a label, counter, flag, or list.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public abstract string Type` | [EEntityType](#class_pull_string_1_1_e_entity_type) for this object, i.e.
`public string Name` | Descriptive name of entity.
`public object Value` | The value of the entity, the type of which will vary depending on the subclass.
`public override string ToString()` | 

### Members

#### `public abstract string Type` 
[//]: # (class_pull_string_1_1_entity_1a1f253c9124109e7103943b9584e294c4)

[EEntityType](#class_pull_string_1_1_e_entity_type) for this object, i.e.

* [EEntityType.Label](#class_pull_string_1_1_e_entity_type_1adaf958316d9e20869a6791e3096de0ea)


* [EEntityType.Counter](#class_pull_string_1_1_e_entity_type_1aa001281f4cf8a4d6f4929192033796ee)


* [EEntityType.Flag](#class_pull_string_1_1_e_entity_type_1a6c368bb9f034b90d9ed1f124ac787d38)


* [EEntityType.List](#class_pull_string_1_1_e_entity_type_1a06a23f98a47b02734364519d0e763510)

#### `public string Name` 
[//]: # (class_pull_string_1_1_entity_1ab575497bf16755b874a1ead7ae726f51)

Descriptive name of entity.



#### `public object Value` 
[//]: # (class_pull_string_1_1_entity_1a906581ae0885d33bd83937dbd1ac10ee)

The value of the entity, the type of which will vary depending on the subclass.

* [Label](#class_pull_string_1_1_label): string


* [Counter](#class_pull_string_1_1_counter): double


* [Flag](#class_pull_string_1_1_flag): bool


* [List](#class_pull_string_1_1_list): object[]

#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_entity_1a32c3f918a5984340537bd2ac912b6059)






# class EEntityType 
[//]: # (class_pull_string_1_1_e_entity_type)


Define the list of entity types

* [EEntityType.Label](#class_pull_string_1_1_e_entity_type_1adaf958316d9e20869a6791e3096de0ea)


* [EEntityType.Counter](#class_pull_string_1_1_e_entity_type_1aa001281f4cf8a4d6f4929192033796ee)


* [EEntityType.Flag](#class_pull_string_1_1_e_entity_type_1a6c368bb9f034b90d9ed1f124ac787d38)


* [EEntityType.List](#class_pull_string_1_1_e_entity_type_1a06a23f98a47b02734364519d0e763510)

### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public const string Label` | 
`public const string Counter` | 
`public const string Flag` | 
`public const string List` | 

### Members

#### `public const string Label` 
[//]: # (class_pull_string_1_1_e_entity_type_1adaf958316d9e20869a6791e3096de0ea)





#### `public const string Counter` 
[//]: # (class_pull_string_1_1_e_entity_type_1aa001281f4cf8a4d6f4929192033796ee)





#### `public const string Flag` 
[//]: # (class_pull_string_1_1_e_entity_type_1a6c368bb9f034b90d9ed1f124ac787d38)





#### `public const string List` 
[//]: # (class_pull_string_1_1_e_entity_type_1a06a23f98a47b02734364519d0e763510)






# class Phoneme 
[//]: # (class_pull_string_1_1_phoneme)


Describe a single phoneme for an audio response, e.g., to drive automatic lip sync.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public string Name` | 
`public double SecondsSinceStart` | 
`public  Phoneme(Dictionary< string, object > json)` | 
`public override string ToString()` | 

### Members

#### `public string Name` 
[//]: # (class_pull_string_1_1_phoneme_1af8e731936106587adbba67fb624e1370)





#### `public double SecondsSinceStart` 
[//]: # (class_pull_string_1_1_phoneme_1a274e802abe660c43b760196076314d3e)





#### `public  Phoneme(Dictionary< string, object > json)` 
[//]: # (class_pull_string_1_1_phoneme_1ac40f569593e4eba93fc3d7af4ef3acd4)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_phoneme_1a50641838ac90935cec5478f5949d697f)






# class Status 
[//]: # (class_pull_string_1_1_status)


Describe the status and any errors from a Web API response.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public bool Success` | 
`public long StatusCode` | 
`public string ErrorMessage` | 
`public  Status()` | 
`public override string ToString()` | 

### Members

#### `public bool Success` 
[//]: # (class_pull_string_1_1_status_1a16ea2ceebff60bfec3269e695a521fb9)





#### `public long StatusCode` 
[//]: # (class_pull_string_1_1_status_1a76cdab2af73b733f6483c99884872030)





#### `public string ErrorMessage` 
[//]: # (class_pull_string_1_1_status_1aef489c04e5737c82a5a053182d789947)





#### `public  Status()` 
[//]: # (class_pull_string_1_1_status_1a0ef229e34a42cd280504d4ee898d5d9d)





#### `public override string ToString()` 
[//]: # (class_pull_string_1_1_status_1ad85f76eff3e4a3d0d3e6d6181079e684)






# class VersionInfo 
[//]: # (class_pull_string_1_1_version_info)


Encapsulates version information for [PullString](#namespace_pull_string)'s Web API.



### Summary

 Members                        | Descriptions                                
--------------------------------|---------------------------------------------
`public const string ApiBaseUrl` | The public-facing endpoint of the [PullString](#namespace_pull_string) Web API.

### Members

#### `public const string ApiBaseUrl` 
[//]: # (class_pull_string_1_1_version_info_1a1a9df55f353b05fca5229fd8d04d9d61)

The public-facing endpoint of the [PullString](#namespace_pull_string) Web API.




