using Newtonsoft.Json.Linq;
using Scriban;
using Scriban.Syntax;

namespace VirtoCommerce.NotificationsModule.LiquidRenderer.Filters
{
    public static class TranslationFilter
    {
        public static string Translate(TemplateContext context, string path, string language = null)
        {
            var localizationResources = (JObject)context.GetValue(new ScriptVariableGlobal("localizationResources"));
            var key = string.IsNullOrEmpty(language) ? path : $"{language}.{path}";
            return (localizationResources?.SelectToken(key) ?? key).ToString();
        }
    }
}
