using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnumsBinding.ModelBinding
{
    public class EnumModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            // if pure enum
            if (bindingContext.ModelType.IsEnum)
            {

                var value = valueProviderResult.FirstValue;

                // Check if the argument value is null or empty
                if (string.IsNullOrEmpty(value))
                {
                    return Task.CompletedTask;
                }


                if (ParseEnumValue(bindingContext.ModelType, value, out var enumVal))
                {
                    bindingContext.Result = ModelBindingResult.Success(enumVal);
                }
                else
                {

                    // Non-enum value arguments result in model state errors
                    bindingContext.ModelState.TryAddModelError(modelName, $"Value '{value}' is out of range");
                }

            }
            else // arrays, enumerables
            {   
                if (valueProviderResult.Length == 0)
                {
                    return Task.CompletedTask;
                }

                var modelType = bindingContext.ModelType.IsGenericType ?
                    bindingContext.ModelType.GenericTypeArguments.First()
                    : bindingContext.ModelType.GetElementType();

                // create an array
                var listOfValues = Array.CreateInstance(modelType, valueProviderResult.Length);

                // fill 
                var index = 0;
                foreach (var value in valueProviderResult.Values)
                {
                    if (!ParseEnumValue(modelType, value, out var enumVal))
                    {
                        // Non-enum value arguments result in model state errors
                        bindingContext.ModelState.TryAddModelError(modelName, $"Value '{value}' is out of range");
                    }
                    else
                    {
                        listOfValues.SetValue(enumVal, index);
                    }

                    index++;
                }

                // check for errors
                if (bindingContext.ModelState.ErrorCount == 0)
                {
                    bindingContext.Result = ModelBindingResult.Success(listOfValues);
                }
            }

            return Task.CompletedTask;
        }

        private static bool ParseEnumValue(Type modelType, string value, out object enumVal)
        {
            // parse case insensitive string values
            var isParsed = Enum.TryParse(modelType, value, true, out enumVal);

            // int value of enum
            var isDefined = Enum.IsDefined(modelType, value);

            return isParsed || isDefined;
        }
    }
}
