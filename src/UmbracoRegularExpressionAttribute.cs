using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Umbraco.Community.ValidationAttributes
{
    /// <summary>
    /// Specifies that a data field value in ASP.net Dynamic Data must match the specified regular expression.
    /// </summary>
    public sealed class UmbracoRegularExpressionAttribute : RegularExpressionAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RegexError";

        public UmbracoRegularExpressionAttribute(string pattern): base(pattern) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-regex", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-regex-pattern", Pattern);
        }
    }
}
