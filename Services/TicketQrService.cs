using System.Text.Json;
using YourNamespace.Models;

namespace KauRestaurant.Services
{
    public class TicketQrService
    {
        public class TicketQrData
        {
            // Ticket information
            public string TicketId { get; set; }
            public string MealType { get; set; }
            // Removed ValidUntil property

            // User information
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string UserFullName { get; set; }

            // Order information
            public string OrderId { get; set; }
            public DateTime OrderDate { get; set; }

            // Security
            public string Signature { get; set; }
        }

        private readonly string _securityKey = "KAU_Restaurant_2025"; // In production, use a proper secure key

        public string GenerateTicketQrData(TicketQrData data)
        {
            // Generate a simple signature (for demonstration purposes)
            // In a production app, you would use a more robust signing mechanism
            data.Signature = GenerateSignature(data);

            // Serialize to JSON
            return JsonSerializer.Serialize(data);
        }

        private string GenerateSignature(TicketQrData data)
        {
            // Create a simple signature based on ticket info and security key
            // In production, use a proper cryptographic signing method
            // Removed ValidUntil from signature calculation
            string dataToSign = $"{data.TicketId}:{data.MealType}:{data.UserId}:{data.OrderId}:{_securityKey}";

            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(dataToSign);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public bool VerifyTicketQrData(string qrData)
        {
            try
            {
                var data = JsonSerializer.Deserialize<TicketQrData>(qrData);
                if (data == null) return false;

                // Extract the provided signature
                string providedSignature = data.Signature;

                // Clear signature for regenerating
                data.Signature = null;

                // Generate a new signature and compare
                string expectedSignature = GenerateSignature(data);

                return providedSignature == expectedSignature;
            }
            catch
            {
                return false;
            }
        }
    }
}
