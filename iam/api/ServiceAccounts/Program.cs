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
    [Verb("service-accounts-create", HelpText = "Creates a service account.")]
    internal class CreateServiceAccountOptions
    {
        [Option("project-id", HelpText = "The project Id.", Required = true)]
        public string ProjectId { get; set; }

        [Option("name", HelpText = "The service account's name.", Required = true)]
        public string Name { get; set; }

        [Option("display-name", HelpText = "The service account's friendly display name.")]
        public string DisplayName { get; set; }
    }

    [Verb("service-accounts-list", HelpText = "Lists service accounts.")]
    internal class ListServiceAccountOptions
    {
        [Option("project-id", HelpText = "The project Id.", Required = true)]
        public string ProjectId { get; set; }
    }

    [Verb("service-accounts-rename", HelpText = "Updates a service account's display name.")]
    internal class RenameServiceAccountOptions
    {
        [Option("project-id", HelpText = "The project Id.", Required = true)]
        public string ProjectId { get; set; }

        [Option("name", HelpText = "The service account name.", Required = true)]
        public string Name { get; set; }

        [Option("name", HelpText = "The servie account's new friendly display name.", Required = true)]
        public string DisplayName { get; set; }
    }

    [Verb("service-accounts-delete", HelpText = "Deletes a service account.")]
    internal class DeleteServiceAccountOptions
    {
        [Option("project-id", HelpText = "The project Id.", Required = true)]
        public string ProjectId { get; set; }

        [Option("name", HelpText = "The service account's name.", Required = true)]
        public string Name { get; set; }
    }

    public static class Program
    {
        private static IamService iam;

        public static int CreateServiceAccount(string projectId, string name,
            string displayName)
        {
            // [START iam_create_service_account]
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
            // [END iam_create_service_account]
            return 0;
        }

        public static int ListServiceAccounts(string projectId)
        {
            // [START iam_list_service_accounts]
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
            // [END iam_list_service_accounts]
            return 0;
        }

        public static int RenameServiceAccount(string projectId, string name,
            string newDisplayName)
        {
            // [START iam_rename_service_account]
            // First, get a ServiceAccount using List() Get()
            string email = $"{name}@{projectId}.iam.gserviceaccount.com";
            string resource = $@"projects/{projectId}/serviceAccounts/{email}";
            ServiceAccount serviceAccount = iam.Projects.ServiceAccounts
                .Get(resource).Execute();

            // Then you can update the display name
            serviceAccount.DisplayName = newDisplayName;
            serviceAccount = iam.Projects.ServiceAccounts.Update(
                serviceAccount, resource).Execute();

            Console.WriteLine($"Updated display name for {serviceAccount.Email} " +
                "to: " + serviceAccount.DisplayName);
            // [END iam_rename_service_account]
            return 0;
        }

        public static int DeleteServiceAccount(string projectId, string email)
        {
            // [START iam_delete_service_account]
            string resource = $@"projects/{projectId}/serviceAccounts/{email}";
            iam.Projects.ServiceAccounts.Delete(resource).Execute();

            Console.WriteLine("Deleted service account: " + email);
            // [END iam_delete_service_account]
            return 0;
        }

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
                    x.ProjectId, x.Name, x.DisplayName),
                (DeleteServiceAccountOptions x) => DeleteServiceAccount(
                    x.ProjectId, x.Name),
                error => 1);
        }
    }
}