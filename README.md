# KafkaWriter

## Overview

The Kafka Writer is a simple console app meant to publish the contents of a JSON file to a Kafka topic and then consume that topic until the key of that message is found. 

The main use case of this tool is confirming the health of a Kafka topic - that it can be both written to, and read from.

## Setup

1. Open `Settings\appsettings1.json` or copy it into a new file under the `Settings\` folder. 
	1. If you copy it, make sure your new file is set to "Copy Always" in `Properties` under the "Copy to Output Directory?" setting
2. Modify your app settings for both the publisher and consumer with your Kafka topic details. 
3. Under the `Messages\` folder, either modify or copy an existing `messageX.json` file with your desired JSON payload.
4. Modify your selected `appsettings` file with the name of your selected `message` file.
	1. Be sure to include the `Messages\` path.
5. Under the `GetHost()` function of `Program.cs`, modify the `.AddJsonFile(...)` function to include your selected `appsettings` file. 
	1. Be sure to include the `Settings/` file path, i.e. `Settings/appsettings1.json`
    
## How to Run

1. Once setup is done, you should just be able to run the app from Visual studio and watch the command-line go!

## Troubleshooting

1. Since the app is so simple, unfortunately most troubleshooting will come down to checking your Kafka settings. 