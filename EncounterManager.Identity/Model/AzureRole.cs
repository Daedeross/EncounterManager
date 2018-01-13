using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterManager.Identity.Model
{
    public class AzureRole : TableEntity, IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
