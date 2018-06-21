﻿// Copyright 2018 Google Inc.
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
    public class Program
    {
        private static IamService service;

        // [START iam_create_key]
        public static int CreateKey(string serviceAccountEmail)
        {
            ServiceAccountKey key = service.Projects.ServiceAccounts.Keys.Create(
                new CreateServiceAccountKeyRequest(),
                "projects/-/serviceAccounts/serviceAccountEmail/")
                .Execute();

            Console.WriteLine("Created key: " + key.Name);
            return 0;
        }
        // [END iam_create_key]

        // [START iam_list_keys]
        public static int ListKeys(string serviceAccountEmail)
        {
            IList<ServiceAccountKey> keys = service.Projects.ServiceAccounts.Keys
                .List($"projects/serviceAccounts/{serviceAccountEmail}/keys")
                .Execute().Keys;

            foreach (ServiceAccountKey key in keys)
            {
                Console.WriteLine("Name: " + key.Name);
            }
            return 0;
        }
        // [END iam_list_keys]

        // [START iam_delete_key]
        public static int DeleteKey(string fullKeyName)
        {
            service.Projects.ServiceAccounts.Keys.Delete(fullKeyName).Execute();
            Console.WriteLine("Deleted service account: " + fullKeyName);
            return 0;
        }
        // [END iam_delete_key]

        public static int Main(string[] args)
        {
            GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                .CreateScoped(IamService.Scope.CloudPlatform);
            service = new IamService(new IamService.Initializer
            {
                HttpClientInitializer = credential
            });

            return Parser.Default.ParseArguments<
                CreateKeyOptions,
                ListKeyOptions,
                DeleteKeyOptions
                >(args).MapResult(
                (CreateKeyOptions x) => CreateKey(
                    x.ServiceAccountEmail),
                (ListKeyOptions x) => ListKeys(
                    x.ServiceAccountEmail),
                (DeleteKeyOptions x) => DeleteKey(
                    x.FullKeyName),
                err => 1);
        }
    }
}
