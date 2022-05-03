using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MongoDB.Bson;

namespace Transleet
{
    public class ObjectIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(ObjectId))
            {
                return new BinderTypeModelBinder(typeof(ObjectIdModelBinder));
            }
            return null;
        }
    }

    public class ObjectIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string? value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            if (!ObjectId.TryParse(value, out var id))
            {
                bindingContext.ModelState.TryAddModelError(
                    modelName, "Invalid ObjectID.");

                return Task.CompletedTask;
            }
            bindingContext.Result=ModelBindingResult.Success(id);
            return Task.CompletedTask;
        }
    }
}
