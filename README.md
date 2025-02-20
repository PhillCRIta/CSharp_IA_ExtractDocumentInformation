# AI examples with Azure

In those examples we explore some capabilities of Azure's enviroment. \
Azure offer a plenty of functionalities to use  Aritificial Intelligence into process workflows, for example for image classification, extract data from images, opereration on text like type of written text, summarizing of text and many other.
I have been concentrated on Azure Cognitive services, but Azure offer also OpenAI functions (like ChatGPT), wrappend into Azure's world.

Every examples are maden with Azure studio, below the list of the portals:
- LANGUAGE STUDIO https://language.cognitive.azure.com/ for text analisys, summaryzing , ect
- VISION STUDIO https://portal.vision.cognitive.azure.com/ for image processing and extract data from it, ect
- DOCUMENT STUDIO https://documentintelligence.ai.azure.com/studio for document processing and extract data from it
- https://ml.azure.com for create own model and train it, ect

## Extract info from receipt
This example shows the capability of Azure Document to extract data from a receipt. Nothing coordinate has been inserted for specify the position of source's data. All position of data has been managed from AI models that return the exact type of data and its meaning (for example company's name, total, each receipt's items)
