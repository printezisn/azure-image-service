# Azure Image Service

An image service designed for the Azure cloud platform. Its main purpose is to upload images to Azure storage and create resized variations. It's a function app with the following components:

- A function to upload an image to an Azure storage blob container.
- A function that gets triggered when an image is uploaded and puts messages in an Azure storage queue, in order to create resized variations of the image.
- A function that gets trigger when a message arrives to the Azure storage queue and creates a resized variation of an image.

## Application Settings

The function app requires the following application settings:

- **AzureWebJobsStorage**: The connection string to the Azure storage account.
- **MainImageContainer**: The name of the blob container that stores the images.
- **QueueName**: The name of the queue that holds messages to create image variations.
- **BaseImageUrl**: The base URL prefix of the images.
- **Sizes**: The resize variations, in a comma-separated format (e.g. "256,128,64").
- **MaxFileSize**: The maximum allowed file size (in bytes).

## How to run locally

1. Set the **application settings** in the **src/ImageService.FunctionApp/local.settings.json** file.
1. Navigate to the **src/ImageService.FunctionApp** directory and run the `func start` command. However, you need to install the [Azure function core tools](https://github.com/Azure/azure-functions-core-tools) first. Alternatively, you can run it with docker:
   - `docker build -t imageservice .`
   - `docker run -it --rm -p 7071:7071 imageservice`

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
