// Copyright 2018 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using CommandLine;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;

namespace GoogleCloudSamples
{
    public static class Program
    {
        private static IamService iam;
        
        // [START iam_create_service_account]
        public static int CreateServiceAccount(string projectId, string name,
            string displayName)
        {
            var request = new CreateServiceAccountRequest
            {
                AccountId = name,
                ServiceAccount = new ServiceAccount
                {
                    DisplayName = displayName
                }
            };

            ServiceAccount serviceAccount = iam.Projects.ServiceAccounts
                .Create(request, "projects/" + projectId).Execute();

            Console.WriteLine("Created service account: " + serviceAccount.Email);
            return 0;
        }
        // [END iam_create_service_account]

        // [START iam_list_service_accounts]
        public static int ListServiceAccounts(string projectId)
        {
            ListServiceAccountsResponse response = iam.Projects.ServiceAccounts
                .List("projects/" + projectId).Execute();
            IList<ServiceAccount> serviceAccounts = response.Accounts;

            foreach (ServiceAccount account in serviceAccounts)
            {
                Console.WriteLine("Name: " + account.Name);
                Console.WriteLine("Display Name: " + account.DisplayName);
                Console.WriteLine("Email: " + account.Email);
                Console.WriteLine();
            }
            return 0;
        }
        // [END iam_list_service_accounts]

        // [START iam_rename_service_account]
        public static int RenameServiceAccount(string email, 
            string newDisplayName)
        {
            // First, get a ServiceAccount using List() or Get()
            string resource = "projects/-/serviceAccounts/" + email; 
            ServiceAccount serviceAccount = iam.Projects.ServiceAccounts
                .Get(resource).Execute();

            // Then you can update the display name
            serviceAccount.DisplayName = newDisplayName;
            serviceAccount = iam.Projects.ServiceAccounts.Update(
                serviceAccount, resource).Execute();

            Console.WriteLine($"Updated display name for {serviceAccount.Email} " +
                "to: " + serviceAccount.DisplayName);
            return 0;
        }
        // [END iam_rename_service_account]

        // [START iam_delete_service_account]
        public static int DeleteServiceAccount(string email)
        {
            string resource = "projects/-/serviceAccounts/" + email; 
            iam.Projects.ServiceAccounts.Delete(resource).Execute();

            Console.WriteLine("Deleted service account: " + email);
            return 0;
        }
        // [END iam_delete_service_account]

        public static void Main(string[] args)
        {
            GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                .CreateScoped(IamService.Scope.CloudPlatform);
            iam = new IamService(new IamService.Initializer
            {
                HttpClientInitializer = credential
            });

            Parser.Default.ParseArguments<
                CreateServiceAccountOptions,
                ListServiceAccountOptions,
                RenameServiceAccountOptions,
                DeleteServiceAccountOptions
                >(args).MapResult(
                (CreateServiceAccountOptions x) => CreateServiceAccount(
                    x.ProjectId, x.Name, x.DisplayName),
                (ListServiceAccountOptions x) => ListServiceAccounts(
                    x.ProjectId),
                (RenameServiceAccountOptions x) => RenameServiceAccount(
                    x.Email, x.DisplayName),
                (DeleteServiceAccountOptions x) => DeleteServiceAccount(
                    x.Email),
                error => 1);
        }
    }
}