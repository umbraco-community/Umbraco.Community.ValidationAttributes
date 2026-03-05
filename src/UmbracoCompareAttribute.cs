using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Umbraco.Community.ValidationAttributes
{
    /// <summary>
    /// Specified that two properties data field value must match.
    /// </summary>
    public sealed class UmbracoCompareAttribute : CompareAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EqualToError";

        public new string ErrorMessageString { get; set; }
        public new string OtherPropertyDisplayName { get; set; }

        public UmbracoCompareAttribute(string otherProperty) : base(otherProperty) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessageString = ValidationAttributesService.DictionaryValue(DictionaryKey);
            
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto", ErrorMessageString);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto-other", $"*.{OtherProperty}");
        }
    }
}
