using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnumsBinding.ModelValidator
{
    public class EnumModelValidator : IModelValidator
    {
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var result = new List<ModelValidationResult>();

            var value = context.Model == null ? "0" : context.Model.ToString();

            // parse case insensitive string values
            var isParsed = Enum.TryParse(context.ModelMetadata.ModelType, value, true, out var enumVal);
            if (isParsed)
            {
                // check parsed enum value
                isParsed = Enum.IsDefined(context.ModelMetadata.ModelType, enumVal);
            }

            // can be numeric value of enum
            var isDefined = Enum.IsDefined(context.ModelMetadata.ModelType, value);

            if (isParsed || isDefined)
            {
                // everything is fine 
            }
            else
            {
                // Non-enum value results in model state errors
                result.Add(new ModelValidationResult(null, "Value is out of range"));
            }

            return result;
        }
    }
}
