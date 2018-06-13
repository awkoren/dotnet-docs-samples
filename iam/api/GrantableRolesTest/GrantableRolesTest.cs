using System;
using Xunit;

namespace GoogleCloudSamples
{
    public class GrantableRolesTest
    {
        [Fact]
        public void TestGrantableRoles()
        {
            // Main will throw an exception on fail
            string project = Environment.GetEnvironmentVariable(
                "GOOGLE_PROJECT_ID");
            string resource = "//cloudresourcemanager.googleapis.com/projects/" +
                project;
                 
            GrantableRoles.Main(new[] { resource });
        }
    }
}
