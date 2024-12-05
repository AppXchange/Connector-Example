namespace Connector.App.v1.Employees;

using Json.Schema.Generation;
using System;
using System.Text.Json.Serialization;
using Xchange.Connector.SDK.CacheWriter;

/// <summary>
/// Data object that will represent an employee in the Xchange system. This will be converted to a JsonSchema, 
/// so add attributes to the properties to provide any descriptions, titles, ranges, max, min, etc... 
/// These types will be used for validation at runtime to make sure the objects being passed through the system 
/// are properly formed. The schema also helps provide integrators more information for what the values 
/// are intended to be.
/// </summary>
[PrimaryKey("id", nameof(Id))]
[Description("Data object representing an employee.")]
public class EmployeesDataObject
{
    [JsonPropertyName("id")]
    [Description("Primary key of the object")]
    public Guid Id { get; set; }

    [JsonPropertyName("employeeId")]
    [Description("Unique identifier of the employee in the system")]
    public int EmployeeId { get; set; }

    [JsonPropertyName("firstName")]
    [Description("First name of the employee")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    [Description("Last name of the employee")]
    public string LastName { get; set; }

    [JsonPropertyName("active")]
    [Description("Indicates if the employee is currently active")]
    public bool Active { get; set; }
}
