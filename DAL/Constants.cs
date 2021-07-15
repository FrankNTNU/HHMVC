using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL
{
    public static class Constants
    {
        public static readonly AzureKeyCredential credentials = new AzureKeyCredential("ae345172fc554a92a1e6ffb5f02d8ec1");
        public static readonly Uri endpoint = new Uri("https://commmentsentimentanalyzer.cognitiveservices.azure.com/");
    }
}