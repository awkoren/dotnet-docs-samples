using System;
using Xunit;

namespace GoogleCloudSamples
{
    public class ServiceAccountTests
    {
        [Fact]
        public void TestServiceAccounts()
        {
            string projectId = Environment.GetEnvironmentVariable(
                "GOOGLE_PROJECT_ID");      
            var rand = new Random().Next(0, 1000);
            string name = "dotnet-test-" + rand;
            string email = $"{name}@{projectId}.iam.gserviceaccount.com";

            Program.Init();
            Program.CreateServiceAccount(projectId, name, "C# Test Account");
            Program.ListServiceAccounts(projectId);
            Program.RenameServiceAccount(email, "Updated C# Test Account");
            Program.DeleteServiceAccount(email);
        }
    }
}