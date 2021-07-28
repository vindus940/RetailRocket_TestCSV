using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
public class Program
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Product(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public const string PathToDoc = "C:/Users/Saint/Desktop/TaskRetail/yml.xml";
    public static void Main(string[] args)
    {
        GetXML();
        Research();
    }
    public static void GetXML()
    {
        string url = "https://www.googleapis.com/drive/v3/files/1sSR9kWifwjIP5qFWcyxGCxN0-MoEd_oo?alt=media&key=AIzaSyBsW_sj1GCItGBK0vl8hr9zu1I1vTI1Meo";
        string savePath = @"C:\Users\Saint\Desktop\TaskRetail\yml.xml";
        WebClient client = new WebClient();
        client.DownloadFile(url, savePath);
    }
    public static void Research()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var document = new XmlDocument();
        document.Load(PathToDoc);
        var xmlDoc = document.SelectNodes("/yml_catalog/shop/offers/offer");
        var count = xmlDoc.Count;
        var products = new List<Product>();
        Console.WriteLine($"Offers count: {count}");
        for (var i = 0; i < count; i++)
        {
            var element = xmlDoc.Item(i);
            var id = int.Parse(element.Attributes.GetNamedItem("id").Value);
            var name = element.SelectSingleNode("name").InnerText;
            var product = new Product(id, name);
            //Console.WriteLine($"Id: {id}, name: {name}");

            products.Add(product);
        }
            using (var writer = new StreamWriter("C:\\Users\\Saint\\Desktop\\TaskRetail\\file.csv", false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(products);
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = header => header.Header.ToLower()
            };

        using (var reader = new StreamReader("C:\\Users\\Saint\\Desktop\\TaskRetail\\file.csv", Encoding.UTF8))
        using (var csv = new CsvReader(reader, config))

        {
            var records = csv.GetRecords<Product>();

            foreach (var record in records)
            {
                Console.WriteLine($"{record.Id} {record.Name}");
            }
        }
    }
}