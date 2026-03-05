using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Umbraco.Community.ValidationAttributes.Helpers;
using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;

namespace Umbraco.Community.ValidationAttributes;

public class UmbracoRequiredIfAttribute : ValidationAttribute, IClientModelValidator, IUmbracoValidationAttribute
{
    private string PropertyName { get; set; }
    private object DesiredValue { get; set; }

    public UmbracoRequiredIfAttribute(string propertyName, object desiredValue) : base()
    {
        PropertyName = propertyName;
        DesiredValue = desiredValue;
    }

    public UmbracoRequiredIfAttribute(string propertyName, object desiredValue, string dictionaryKey) : base()
    {
        PropertyName = propertyName;
        DesiredValue = desiredValue;
        DictionaryKey = dictionaryKey;
    }

    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var instance = context.ObjectInstance;
        var type = instance.GetType();
        var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance, null);

        if (propertyValue?.ToString() == DesiredValue?.ToString())
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(ErrorMessage ?? $"{context.DisplayName} is required.");
            }
        }

        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }


        ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
        AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
        AttributeHelper.MergeAttribute(context.Attributes, "data-val-requiredif", ErrorMessage);
        AttributeHelper.MergeAttribute(context.Attributes, "data-val-requiredif-dependentproperty", PropertyName);
        AttributeHelper.MergeAttribute(context.Attributes, "data-val-requiredif-desiredvalue", DesiredValue?.ToString());
    }

    public string DictionaryKey { get; set; } = "RequiredIfError";
}