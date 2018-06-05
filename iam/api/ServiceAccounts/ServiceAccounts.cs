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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;

public static class ServiceAccounts
{
    private static GoogleCredential credential; 
    private static IamService iam;
    private static string projectId;

    public static void Main(string[] args)
    {
        credential = GoogleCredential.GetApplicationDefault()
            .CreateScoped(IamService.Scope.CloudPlatform);
        iam = new IamService(new IamService.Initializer
        {
            HttpClientInitializer = credential
        });
        projectId = "projects/awkoren-iam";

        CreateServiceAccount();
        ListServiceAccounts();
    }

    public static void CreateServiceAccount() 
    {
        var request = new CreateServiceAccountRequest
        {
            AccountId = "sa-name",
            ServiceAccount = new ServiceAccount
            {
                DisplayName = "sa-display-name"
            }
        };

        ServiceAccount serviceAccount = iam.Projects.ServiceAccounts
            .Create(request, projectId).Execute();

        Console.WriteLine("Created service account: " + serviceAccount.Email);
    }

    public static void ListServiceAccounts()
    {
        ListServiceAccountsResponse response = iam.Projects.ServiceAccounts
            .List(projectId).Execute();
        IList<ServiceAccount> serviceAccounts = response.Accounts;

        foreach (ServiceAccount account in serviceAccounts)
        {
            Console.WriteLine("Name: " + account.DisplayName);
            Console.WriteLine("Email: " + account.Email);
            Console.WriteLine();
        }
    }

    public static void RenameServiceAccount()
    {
        
    }

    public static void DeleteServiceAccount()
    {
        iam.Projects.ServiceAccounts.Delete("").Execute();
    }
}
