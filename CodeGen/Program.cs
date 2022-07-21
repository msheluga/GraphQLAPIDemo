// See https://aka.ms/new-console-template for more information


using GraphQLAPIDemo.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

CreateClasses();

static void CreateClasses()
{
    var dbSetProps = GetDbSetProperties(typeof(BooksContext));
    foreach(var prop in dbSetProps)
    {
        var entityType = prop.PropertyType.GenericTypeArguments[0];
        var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("GraphQLAPIDemo")).NormalizeWhitespace();
        @namespace.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));
        var classDeclaration = SyntaxFactory.ClassDeclaration(entityType.Name);
        var entityProps = entityType.GetProperties();
    }
    
}

static List<PropertyInfo> GetDbSetProperties(Type contextType)
{
    var dbSetProperties = new List<PropertyInfo>();
    var properties = contextType.GetProperties();

    foreach (var property in properties)
    {
        var setType = property.PropertyType;

        var isDbSet = setType.IsGenericType && (typeof (DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()) || setType.GetInterface(typeof (DbSet<>).FullName) != null);


        if (isDbSet)
        {
            dbSetProperties.Add(property);
        }
    }

    return dbSetProperties;

}
