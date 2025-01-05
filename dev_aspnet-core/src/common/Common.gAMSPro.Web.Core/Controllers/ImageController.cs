using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Text.Json;
using gAMSPro.ProcedureHelpers;
using Common.gAMSPro.Branchs.Dto;
using Common.gAMSPro.Consts;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.Images;
using Common.gAMSPro.Intfs.Images.Dto;
using Core.gAMSPro.Utils;
using PayPalCheckoutSdk.Orders;
using Aspose.Words.Drawing;
using System.DirectoryServices.Protocols;

namespace Common.gAMSPro.Web.Controllers
{
    public class ResultModel
    {
        public int Cars { get; set; }
        public int Motorcycles { get; set; }
    }


    //[DisableValidation]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImageController : CoreAmsProControllerBase
    {
        readonly IImageAppService imageAppService;
        
        public ImageController(IImageAppService imageAppService)
        {
            this.imageAppService = imageAppService;
        }

        [HttpPost]

        public async Task<ActionResult<ResultModel>> /*IActionResult*/ UploadImage(IFormFile imageFile)
        {

            // Define the path to save the image
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save the image
            string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName) + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }


            string relativePath = $"/images/{fileName}";

            string imageURL = "https://localhost:44301" + relativePath;
            string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string absolutePath = Path.Combine(rootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            byte[] imageArray = System.IO.File.ReadAllBytes(absolutePath);
            string encoded = Convert.ToBase64String(imageArray);
            byte[] data = Encoding.ASCII.GetBytes(encoded);
            string API_KEY = ""; // Your API Key
            string MODEL_ENDPOINT = "dataset/v"; // Set model endpoint
                                                 //return Ok(new { response = fileName });
                                                 // Construct the URL
            string uploadURL =
                    "https://detect.roboflow.com/vehicle-detection-zlvlt/10?api_key=HfGyyQyhxMF44ah674fk&confidence=20&overlap=30&format=json"
                + "&name=" + fileName;
            //return Ok(new { response = uploadURL });
            // Service Request Config
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Configure Request
            WebRequest request = WebRequest.Create(uploadURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            // Write Data
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // Get Response
            string responseContent = null;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            var jsonObject = JsonSerializer.Deserialize<object>(responseContent);

            // Extract the file name without the extension from the image path
            string imageFileName = Path.GetFileNameWithoutExtension(fileName);

            // Define the directory where the JSON file will be saved
            string jsonDirectory = Path.Combine(rootPath, "images"); // Save in the same directory as the image
            string jsonFilePath = Path.Combine(jsonDirectory, $"{imageFileName}.json");

            // Ensure the directory exists
            if (!Directory.Exists(jsonDirectory))
            {
                Directory.CreateDirectory(jsonDirectory);
            }

            // Serialize the object back to JSON string
            var options = new JsonSerializerOptions { WriteIndented = true }; // Pretty-print JSON
            string jsonString = JsonSerializer.Serialize(jsonObject, options);

            // Write the JSON string to the file
            //System.IO.File.WriteAllText(jsonFilePath, jsonString);
            var root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Initialize counters
            int carCount = 0;
            int motorcycleCount = 0;

            // Count cars and motorcycles
            foreach (var prediction in root.Predictions)
            {
                if (prediction.Class == "car")
                    carCount++;
                else if (prediction.Class == "motorcycle")
                    motorcycleCount++;
            }

            // Create the result object
            var result = new
            {
                cars = carCount,
                motorcycles = motorcycleCount
            };

            string[] sParts = imageFileName.Split('_');
            string regionName = sParts[0];
            string dateString = sParts[1];

            // Serialize the result to JSON and save to a file
            string resultJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            //System.IO.File.WriteAllText(jsonFilePath, resultJson);
            await imageAppService.VehicleDetection_Ins("Ô tô", regionName, carCount, relativePath, dateString);
            await imageAppService.VehicleDetection_Ins("Xe máy", regionName, motorcycleCount, relativePath, dateString);
            //return await imageAppService.Image_Insert( location, dateString, relativePath, carCount, motorcycleCount);
            //return Ok(new { message = "JSON file saved successfully.", filePath = $"/images/{imageFileName}.json" });          }
            return Ok(result);
        }
        public class Root
        {
            public List<Prediction> Predictions { get; set; }
        }
        public class Prediction
        {
            public string Class { get; set; }
        }
    }
}