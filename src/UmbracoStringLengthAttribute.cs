using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Umbraco.Community.ValidationAttributes
{
    /// <summary>
    /// Specifies the minimum and maximum length of charactors that are allowed in a data field. 
    /// </summary>
    public sealed class UmbracoStringLengthAttribute : StringLengthAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "MinMaxLengthError";

        public UmbracoStringLengthAttribute(int maximumLength) : base(maximumLength) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-length", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-length-max", MaximumLength.ToString());
            if (MinimumLength > 0)
            {
                AttributeHelper.MergeAttribute(context.Attributes, "data-val-length-min", MinimumLength.ToString());
            }
        }
    }
}
