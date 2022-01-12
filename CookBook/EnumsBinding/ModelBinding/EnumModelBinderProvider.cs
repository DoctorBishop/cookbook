using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnumsBinding.ModelBinding
{
    public class EnumModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.IsEnumerableType)
                return new EnumModelBinder();

            return null;
        }
    }
}
