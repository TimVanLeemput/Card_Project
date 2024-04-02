[![copyright: TVL](https://img.shields.io/badge/Copyright-TVL-yellow.svg)](https://pluz21.itch.io/)

# Card_Creator

## Overview
Card_Creator is a Unity project that enables developers to easily create cards as scriptable objects for use in their projects.

## Getting Started
To create a new card, follow these steps:
1. Right-click in the Project window.
2. Navigate to Create -> New Card.
3. The card name will be generated automatically, but you need to set the following attributes:
   - Resource type
   - Skills
   - Attack and defense values (if needed)
   - Resource cost
   - Card type

## AI Helper Tool
Card_Creator includes an AI Helper tool for generating flavor text and images for your cards.

### Flavor Text Generation
To generate flavor text:
1. Obtain an OpenAI API Key from [OpenAI platform](https://platform.openai.com/api-keys).
2. Log in using your OpenAI account credentials.
3. Navigate to the Flavor Text Generation tab.
4. Drag and drop one of your Scriptable Object Cards into the "Card Scriptable Object to collect for flavor text prompt" field.
5. Optionally, add keywords to tweak the result.
6. Adjust the AI temperature using the slider.
7. Click on the "Generate Text" button to create flavor texts.
8. All flavor texts will be saved under individual folders with the card name in the "Resources-AI_Texts" path.
9. Select the desired flavor text from the dropdown menu to set it for the card scriptable object.

### Image Generation
To generate images:
1. Log in to OpenAI using the credentials tab.
2. Navigate to the Image Generation tab inside the AI_Card_Tool_EditorWindow.
3. Unfold the BaseTemplate from the hierarchy and locate the AI_Art GameObject.
4. Drag and drop this GameObject into the "GameObject to place image" field.
5. Enter a prompt.
6. Click on "Generate Image".
7. Wait a few seconds for the image to appear on the card image panel.
8. Optionally, open the generated image in your browser.

## Prefab Generator
To generate prefabs:
1. Open the Tools menu.
2. Click on Prefab Generator.
3. Drag and Drop the BaseTemplate card into the Prefab create.
4. Add the folder name, subfolder name, and prefab name.
5. Click on "Generate".

## Dependencies:
- Microsoft.AspNet.WebApi.Client
- Microsoft.AspNet.WebApi.Core
- Microsoft.Bcl.AsyncInterfaces
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Abstractions
- Microsoft.Extensions.Configuration.Binder
- Microsoft.Extensions.DependencyInjection.Abstractions
- Microsoft.Extensions.Http
- Microsoft.Extensions.Logging
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Options
- Microsoft.Extensions.Primitives
- Newtonsoft.Json
- OpenAI
- System.Buffers
- System.Interactive.Async
- System.Linq.Async
- System.Memory
- System.Numerics.Vectors
- System.Runtime.CompilerServices.Unsafe
- System.Threading.Tasks.Extensions

## Acknowledgment

Used part of the GoDoIt wrapper : 
https://github.com/OkGoDoIt/OpenAI-API-dotnet

