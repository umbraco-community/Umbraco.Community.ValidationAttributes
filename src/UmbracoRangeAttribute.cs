using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Umbraco.Community.ValidationAttributes
{
    public sealed class UmbracoRangeAttribute : RangeAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RangeError";

        public string WarningMessage { get; set; } = "RangeWarning";

        public UmbracoRangeAttribute(int minimum, int maximum) : base(minimum, maximum) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);

            string customConfig = ValidationAttributesService.GetConfigKey(Constants.Configuration.RangeInputType);
            if (!string.IsNullOrEmpty(customConfig))
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", customConfig.ToLower(), replaceExisting: true);
            }
            else
            {
                AttributeHelper.MergeAttribute(context.Attributes, "type", "range", replaceExisting: true);
            }

            AttributeHelper.MergeAttribute(context.Attributes, "data-val-required", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range", ValidationAttributesService.DictionaryValue(WarningMessage));
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range-min", Minimum.ToString());
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-range-max", Maximum.ToString());
        }
    }
}
