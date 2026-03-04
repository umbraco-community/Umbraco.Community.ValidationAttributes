using Umbraco.Community.ValidationAttributes.Interfaces;
using Umbraco.Community.ValidationAttributes.Services;
using System.ComponentModel;

namespace Umbraco.Community.ValidationAttributes
{
    public sealed class UmbracoDisplayNameAttribute : DisplayNameAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; }

        public UmbracoDisplayNameAttribute(string dictionaryKey) : base()
        {
            DictionaryKey = dictionaryKey;
        }

        public override string DisplayName => ValidationAttributesService.DictionaryValue(DictionaryKey);
    }
}
