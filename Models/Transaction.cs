using System.Net.NetworkInformation;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace MyFleetManagerPro.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int VehicleID { get; set; }
        public double Spent { get; set; }
        public double Poured { get; set; }
        public int OdometerReading { get; set; }
        public string Merchant { get; set; }

        public static string CurrentImage { get; set; } = string.Empty;

        public static Dictionary<string, string> ProcessTransaction(string imageLocation)
        {
            Dictionary<string, string> transactionData = new Dictionary<string, string>();
            transactionData["TransactionDate"] = "2000-01-01";
            transactionData["MerchantName"] = "Shell";
            transactionData["Quantity"] = "10";
            transactionData["Spent"] = "100";

            try
            {
                string endpoint = "https://receipt-scan-mtr.cognitiveservices.azure.com/";
                string key = "b6f1ca80620842c6898bea71b3aa0a1d";

                AzureKeyCredential credential = new AzureKeyCredential(key);
                DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint), credential);


                if (imageLocation != null)
                {
                    Stream fileStream = File.OpenRead(imageLocation);
                    AnalyzeDocumentOperation operation = client.AnalyzeDocument(WaitUntil.Completed, "prebuilt-receipt", fileStream);

                    AnalyzeResult receipts = operation.Value;

                    foreach (AnalyzedDocument receipt in receipts.Documents)
                    {
                        if (receipt.Fields.TryGetValue("MerchantName", out DocumentField merchantNameField))
                        {
                            if (merchantNameField.FieldType == DocumentFieldType.String)
                            {
                                string merchantName = merchantNameField.Value.AsString();

                                transactionData["MerchantName"] = merchantName;

                            }
                        }

                        if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField transactionDateField))
                        {
                            if (transactionDateField.FieldType == DocumentFieldType.Date)
                            {
                                DateTimeOffset transactionDate = transactionDateField.Value.AsDate();

                                transactionData["TransactionDate"] = transactionDate.ToString();
                            }
                        }

                        if (receipt.Fields.TryGetValue("Items", out DocumentField itemsField))
                        {
                            if (itemsField.FieldType == DocumentFieldType.List)
                            {

                                foreach (DocumentField itemField in itemsField.Value.AsList())
                                {


                                    if (itemField.FieldType == DocumentFieldType.Dictionary)
                                    {
                                        IReadOnlyDictionary<string, DocumentField> itemFields = itemField.Value.AsDictionary();

                                        if (itemFields.TryGetValue("Description", out DocumentField itemDescriptionField))
                                        {
                                            if (itemDescriptionField.FieldType == DocumentFieldType.String)
                                            {
                                                string itemDescription = itemDescriptionField.Value.AsString();


                                            }
                                        }

                                        if (itemFields.TryGetValue("Quantity", out DocumentField itemQuantityField))
                                        {
                                            if (itemQuantityField.FieldType == DocumentFieldType.Double)
                                            {
                                                double itemQuantity = itemQuantityField.Value.AsDouble();

                                                transactionData["Quantity"] = itemQuantity.ToString();
                                            }
                                        }

                                        if (itemFields.TryGetValue("TotalPrice", out DocumentField itemTotalPriceField))
                                        {
                                            if (itemTotalPriceField.FieldType == DocumentFieldType.Double)
                                            {
                                                double itemTotalPrice = itemTotalPriceField.Value.AsDouble();


                                            }
                                        }
                                    }
                                }
                            }


                        }

                        if (receipt.Fields.TryGetValue("Total", out DocumentField totalField))
                        {
                            if (totalField.FieldType == DocumentFieldType.Double)
                            {
                                double total = totalField.Value.AsDouble();

                                transactionData["Spent"] = total.ToString();
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"No Image Loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
            }

            return transactionData;
        }
    }


}

