# Unity SDK for the PullString Web API

## Overview

This repository provides a module to access the PullString Web API.

The PullString Web API lets you add text or audio conversational capabilities to your apps, based upon content that you write in the PullString Author environment and publish to the PullString Platform.

## Quickstart

The primary classes for interacting with the PullString SDK are the Conversation, Request, and Response objects. Below is a simple example showing how to start a conversation with the PullString Web API. It will print the initial content under the default Activity for your Project. We've provided the API key and Project ID for the example **Rock, Paper, Scissors** chatbot, but of course, you can change `API_KEY` and `PROJECT_ID` to user our own Project ID and API key. You can find these values in the settings for you project in you account on **[pullstring.com](http://pullstring.com)**

```csharp
using PullString;

...

const string MY_API_KEY = "9fd2a189-3d57-4c02-8a55-5f0159bff2cf";
const string MY_PROJECT_ID = "e50b56df-95b7-4fa1-9061-83a7a9bea372";

...

var conversation = new Conversation();
var request = new Request() {
    ApiKey = MY_API_KEY
};

conversation.OnResponseReceived += (Response response) => {
    foreach(var output in response.Outputs)
    {
        var dialog = (DialogOutput)output;
        Debug.Log(dialog.Text);
    }
};

conversation.Begin(MY_PROJECT_ID, request);

// > Do you want to play Rock, Paper, Scissors?
```
Open the *Example* scene in `Assets/Scenes` for another example demonstrating how to use the SDK to hold a conversation. This scene once again connects to the **Rock, Paper, Scissors** bot, but it shows how to use both text and speech input.

*Note that to run the example on iOS, you'll need to add the `NSMicrophoneUsageDescription` key to the `Info.plist` and a description of how the mic will be used.*

## Importing

To use the SDK in your own project, either copy the contents of `Assets/Scripts/PullString` into the project or right-click the directory within the Example scene to export a Unity package.

## Documentation

Documentation for this SDK can be found in the `docs` directory. In addition, the PullString Web API specification can be found at:

> http://docs.pullstring.com/docs/api

For more information about the PullString Platform, refer to:

> http://pullstring.com

## Testing

The SDK's unit tests can be found in the UnitTest scene. In order to open the scene and run the tests, be sure to import the [Unity Test Tools](https://www.assetstore.unity3d.com/en/#!/content/13802). Once, imported, open the Integration Test Runner under `Unity Test Tools > Integration Test Runner` and click Run All.
