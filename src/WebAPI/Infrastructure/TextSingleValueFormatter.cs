using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;
using System.Text.Json;

namespace WebAPI.Infratructure
{
    public class TextSingleValueFormatter : InputFormatter
    {
        public TextSingleValueFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeNames.Text.Plain);
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            try
            {
                using (var reader = new StreamReader(context.HttpContext.Request.Body))
                {
                    string textSingleValue = await reader.ReadToEndAsync();
                    //Convert from string to target model type (this is the parameter type in the action method)
                    object model = JsonSerializer.Deserialize(textSingleValue,context.ModelType);
                    return InputFormatterResult.Success(model);
                }
            }
            catch (Exception ex)
            {
                context.ModelState.TryAddModelError("BodyTextValue", $"{ex.Message} ModelType={context.ModelType}");
                return InputFormatterResult.Failure();
            }
        }

        protected override bool CanReadType(Type type)
        {
            //Put whatever types you want to handle. 
            return type == typeof(string) ||
                type == typeof(int) ||
                type == typeof(DateTime);
        }
        public override bool CanRead(InputFormatterContext context)
        {
            return context.HttpContext.Request.ContentType == MediaTypeNames.Text.Plain;
        }
    }
}
