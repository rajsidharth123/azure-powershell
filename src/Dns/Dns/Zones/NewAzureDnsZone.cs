﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Collections;
using System.Management.Automation;
using Microsoft.Azure.Commands.Dns.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using ProjectResources = Microsoft.Azure.Commands.Dns.Properties.Resources;

namespace Microsoft.Azure.Commands.Dns
{
    /// <summary>
    /// Creates a new zone.
    /// </summary>
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "DnsZone", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium), OutputType(typeof(DnsZone))]
    public class NewAzureDnsZone : DnsBaseCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The full name of the zone (without a terminating dot).")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The resource group in which to create the zone.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Alias("Tags")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A hash table which represents resource tags.")]
        public Hashtable Tag { get; set; }

        public override void ExecuteCmdlet()
        {
            WriteWarning("The output object type of this cmdlet will be modified in a future release.");


            if (Name.EndsWith("."))
            {
                Name = Name.TrimEnd('.');
                WriteWarning(string.Format("Modifying zone name to remove terminating '.'.  Zone name used is \"{0}\".", Name));
            }

            ConfirmAction(
                ProjectResources.Progress_CreatingNewZone,
                this.Name,
            () =>
            {
                DnsZone result = DnsClient.CreateDnsZone(Name, ResourceGroupName, Tag);
                WriteVerbose(ProjectResources.Success);
                WriteVerbose(string.Format(ProjectResources.Success_NewZone, Name, ResourceGroupName));
                WriteObject(result);
            });
        }
    }
}
