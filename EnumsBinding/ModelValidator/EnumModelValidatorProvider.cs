using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EnumsBinding.ModelValidator
{
    public class EnumModelValidatorProvider : IModelValidatorProvider
    {
        public void CreateValidators(ModelValidatorProviderContext context)
        {
            if (context.ModelMetadata.IsEnum)
            {
                context.Results.Add(new ValidatorItem()
                {
                    Validator = new EnumModelValidator(),
                    IsReusable = true,
                });
            }
        }
    }
}
