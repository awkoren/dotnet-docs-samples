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

public class ServiceAccountKeys
{
    private IamService iam;
    private string projectId;

    public ServiceAccountKeys(IamService iam, string projectId)
    {
        this.iam = iam;
        this.projectId = projectId;
    }

    public void CreateKey() 
    {
        string serviceAccount = "";

        ServiceAccountKey key = iam.Projects.ServiceAccounts.Keys.Create(
            new CreateServiceAccountKeyRequest(),
            serviceAccount).Execute();

        Console.WriteLine("Created key: " + key.PrivateKeyData);
    }

    public void ListKeys()
    {
        ListServiceAccountKeysResponse response = iam.Projects.ServiceAccounts
            .Keys.List(projectId).Execute();
        
        foreach (ServiceAccountKey key in response.Keys)
        {
            Console.WriteLine("Key: " + key.Name);
        }
    }

    public void DeleteKey()
    {
        string name = "";
        iam.Projects.ServiceAccounts.Keys.Delete(name).Execute();
    }
}
