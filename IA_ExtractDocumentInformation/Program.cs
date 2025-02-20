using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using System.Collections;
using System.IO;

string endpint = "https://xx.cognitiveservices.azure.com/";
string apiKey = "xx";

var credential = new AzureKeyCredential(apiKey);

var client = new DocumentAnalysisClient(new Uri(endpint), credential);

Stream stream = new MemoryStream(File.ReadAllBytes(@"tempin\recepit.jpg"));

AnalyzeDocumentOperation operation = client.AnalyzeDocument(WaitUntil.Completed, "prebuilt-receipt", stream);

AnalyzeResult results = operation.Value;

List<string> extractValue = new List<string>();

foreach (AnalyzedDocument receipt in results.Documents)
{
	string merchantName, transactionDate, totalAmount = string.Empty;
	if (receipt.Fields.TryGetValue("MerchantName", out DocumentField _merchantName))
	{
		merchantName = _merchantName.Content.ToString();
		extractValue.Add("Esercente: " + merchantName);
	}
	if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField _transactionDate))
	{
		transactionDate = _transactionDate.Content.ToString();
		extractValue.Add("Data scontrino: " + transactionDate);
	}
	if (receipt.Fields.TryGetValue("Total", out DocumentField _totalAmount))
	{
		_totalAmount.Value.AsDouble();
		totalAmount = _totalAmount.Content.Replace("\n", " ");
		extractValue.Add("Totale: " + totalAmount);
	}
	extractValue.Add(System.Environment.NewLine);
	extractValue.Add("Elenco articoli: " + System.Environment.NewLine);
	if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
	{
		if (itemsField.FieldType == DocumentFieldType.List)
		{
			foreach (DocumentField itemField in itemsField.Value.AsList())
			{
				string Description = string.Empty;
				string TotalPrice = string.Empty;
				if (itemField.FieldType == DocumentFieldType.Dictionary)
				{
					IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();
					if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
					{
						if (itemDescriptionField.FieldType == DocumentFieldType.String)
						{
							string itemDescription = itemDescriptionField.Value.AsString();
							Description = $"  Description: '{itemDescription}', with confidence {itemDescriptionField.Confidence}";
							Description = itemDescription;
						}
					}
					if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
					{
						if (itemTotalPriceField.FieldType == DocumentFieldType.Double)
						{
							double itemTotalPrice = itemTotalPriceField.Value.AsDouble();
							TotalPrice = $"  Total Price: '{itemTotalPrice}', with confidence {itemTotalPriceField.Confidence}";
							TotalPrice = itemTotalPrice.ToString();
						}
					}
				}
				extractValue.Add(Description + "\t\t\t" + TotalPrice);
			}
		}
		extractValue.Add(System.Environment.NewLine);
		extractValue.Add(System.Environment.NewLine);
		extractValue.Add(System.Environment.NewLine);
	}

	Console.WriteLine("");

	File.WriteAllLines("fileout.txt", extractValue, System.Text.Encoding.UTF8);	
}