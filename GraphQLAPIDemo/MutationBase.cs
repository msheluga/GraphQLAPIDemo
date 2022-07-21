using AutoMapper;
using GraphQLAPIDemo.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQLAPIDemo
{
    public abstract class MutationBase
    {
        protected async Task<T?> UpdateEntity<T>(BooksContext context, IMapper mapper, object input, params object[] keyValues)
            where T : class
        {
            var entity = await context.Set<T>().FindAsync(keyValues);
            if(entity != null)
            {
                mapper.Map(input, entity);
                context.Update(entity);
                context.SaveChanges();
                return entity;
            }
            return null;
        }
        protected async Task<T?> UpdateEntity<T>(BooksContext context, object input, params object[] keyValues)
            where T : class
        {
            var entity = await context.Set<T>().FindAsync(keyValues);
            if (entity != null)
            {
                var keys = context.Model.FindEntityType(entity.GetType()).FindPrimaryKey().Properties.Select(p => p.Name);
                PartialUpdateEntity(input, entity, keys);
                context.Update(entity);
                context.SaveChanges();
                return entity;
            }
            return null;
        }

        protected void PartialUpdateEntity(object inputTypeObject, object dbEntityObject, IEnumerable<string> ignoredProps)
        {
            var inputObjectProperties = inputTypeObject.GetType().GetProperties();
            var dbEntityPropertiesMap = dbEntityObject.GetType().GetProperties().ToDictionary(x => x.Name);
            foreach (var inputObjectProperty in inputObjectProperties)
            {
                if (ignoredProps.Contains(inputObjectProperty.Name))
                {
                    continue;
                }
                //Optional Properties
                if (inputObjectProperty.PropertyType.Name == "Optional`1")
                {
                    dynamic? hasValue = inputObjectProperty?.PropertyType?.GetProperty("HasValue")?.GetValue(inputObjectProperty?.GetValue(inputTypeObject));
                    if (hasValue != null && hasValue == true)
                    {
                        var value = inputObjectProperty?.PropertyType?.GetProperty("Value")?.GetValue(inputObjectProperty.GetValue(inputTypeObject));
                        //If the field was passed as null deliberately to set null in the column, setting it to the default value of the db type in this case.
                        dbEntityPropertiesMap[inputObjectProperty.Name].SetValue(dbEntityObject, value ?? default);

                    }
                }
                //Required Properties
                else
                {
                    var value = inputObjectProperty.GetValue(inputTypeObject);
                    //If the field was passed as null deliberately to set null in the column, setting it to the default value of the db type in this case.
                    dbEntityPropertiesMap[inputObjectProperty.Name].SetValue(dbEntityObject, value ?? default);
                }
            }
        }
    }
}
