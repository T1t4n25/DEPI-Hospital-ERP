using HospitalERP.API.Common.Pagination;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HospitalERP.API.Common.Swagger;

public class SwaggerExampleFilter : IOperationFilter
{
    public void Apply(Microsoft.OpenApi.OpenApiOperation operation, OperationFilterContext context)
    {
        // Add examples to successful responses (200, 201)
        if (operation.Responses != null)
        {
            foreach (var response in operation.Responses.Where(r => r.Key == "200" || r.Key == "201"))
            {
                if (response.Value.Content != null)
                {
                    foreach (var content in response.Value.Content)
                    {
                        if (content.Value.Schema != null)
                        {
                            var example = GetExampleForResponse(context);
                            if (example != null)
                            {
                                // Convert to JsonNode for Swagger
                                var jsonString = JsonSerializer.Serialize(example, new JsonSerializerOptions
                                {
                                    WriteIndented = true
                                });
                                
                                try
                                {
                                    var jsonNode = JsonNode.Parse(jsonString);
                                    if (jsonNode != null)
                                    {
                                        content.Value.Example = jsonNode;
                                    }
                                }
                                catch
                                {
                                    // If parsing fails, skip this example
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private object? GetExampleForResponse(OperationFilterContext context)
    {
        // Try to get type from method return type
        if (context.MethodInfo.ReturnType != null)
        {
            var returnType = context.MethodInfo.ReturnType;
            
            // Handle Task<T> and ActionResult<T>
            if (returnType.IsGenericType)
            {
                var genericDef = returnType.GetGenericTypeDefinition();
                
                if (genericDef == typeof(Task<>))
                {
                    returnType = returnType.GetGenericArguments()[0];
                }
                else if (genericDef.Name == "ActionResult`1" || genericDef.FullName?.Contains("ActionResult") == true)
                {
                    returnType = returnType.GetGenericArguments()[0];
                }
            }
            
            // Handle PaginatedResponse<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(PaginatedResponse<>))
            {
                var itemType = returnType.GetGenericArguments()[0];
                return GetPaginatedExample(itemType);
            }
            
            // Handle regular DTOs
            if (returnType.IsClass && !returnType.IsPrimitive && returnType != typeof(string) && returnType != typeof(object))
            {
                return GetExampleForType(returnType);
            }
        }
        
        return null;
    }

    private object? GetExampleForType(Type type)
    {
        var examplesProviderType = typeof(ExamplesProvider);
        
        // Try to find a static property with a name matching the type
        var propertyName = GetExamplePropertyName(type);
        var property = examplesProviderType.GetProperty(propertyName, 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.Static);
        
        if (property != null && property.PropertyType == type)
        {
            return property.GetValue(null);
        }
        
        return null;
    }

    private string GetExamplePropertyName(Type type)
    {
        var typeName = type.Name;
        return typeName.Replace("Dto", "") + "Example";
    }

    private object? GetPaginatedExample(Type itemType)
    {
        try
        {
            var examplesProviderType = typeof(ExamplesProvider);
            var propertyName = GetPaginatedExamplePropertyName(itemType);
            var property = examplesProviderType.GetProperty(propertyName,
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static);
            
            if (property != null)
            {
                return property.GetValue(null);
            }
        }
        catch
        {
            // Fallback
        }
        
        return null;
    }

    private string GetPaginatedExamplePropertyName(Type itemType)
    {
        var typeName = itemType.Name;
        var baseName = typeName.Replace("DetailDto", "").Replace("ListDto", "").Replace("Dto", "");
        
        var pluralName = baseName switch
        {
            "Patient" => "Patients",
            "Department" => "Departments",
            "Service" => "Services",
            "Employee" => "Employees",
            "Appointment" => "Appointments",
            "MedicalRecord" => "MedicalRecords",
            "Medication" => "Medications",
            "Inventory" => "Inventory",
            "Invoice" => "Invoices",
            _ => baseName + "s"
        };
        
        return pluralName + "PaginatedExample";
    }
}
