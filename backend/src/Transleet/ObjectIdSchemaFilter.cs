using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Transleet
{
    public class ObjectIdSchemaFilter: ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(ObjectId))
            {
                return;
            }
            schema.Type = "string";
        }
    }
}
