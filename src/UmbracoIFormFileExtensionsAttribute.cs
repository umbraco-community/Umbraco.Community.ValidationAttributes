using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using J2N.Collections.Generic;
using Lucene.Net.Analysis.Hunspell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;

namespace Umbraco.Community.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UmbracoIFormFileExtensionsAttribute : ValidationAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey {get; set;} = "FormFileExtensionsError";

        public string[] ValidFileTypes { get; set; }

        public UmbracoIFormFileExtensionsAttribute(string fileTypes)
        {
            ValidFileTypes = ParseFileTypes(fileTypes);
        }

        public UmbracoIFormFileExtensionsAttribute(string fileTypes, string dictionaryKey)
        {
            DictionaryKey = dictionaryKey;
            ValidFileTypes = ParseFileTypes(fileTypes);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            ErrorMessage = FormatErrorMessage(string.Join(", ", ValidFileTypes));
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-filetypes", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-filetypes-types", string.Join(',', ValidFileTypes));

            // input type="file" accept attribute 
            List<string> validExtensions = new List<string>();
            foreach (string type in ValidFileTypes)
            {
                validExtensions.Add($".{type}");
            }
            AttributeHelper.MergeAttribute(context.Attributes, "accept", string.Join(',', validExtensions));
        }

        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            bool isValid = true;
            
            if (file != null)
            {
                isValid = ValidFileTypes.Any(x => file.FileName.ToLower().EndsWith(x));
            }

            return isValid;
        }

        private string[] ParseFileTypes(string fileTypes)
        {
            return fileTypes.ToLower().Split(',');
        }
    }
}
