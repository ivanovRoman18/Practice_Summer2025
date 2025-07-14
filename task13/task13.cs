using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Subject
{
    public string? Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Subject>? Grades { get; set; }
}

public static class JsonService
{
    private static readonly JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new DateOnlyJsonConverter() }
    };

    public static string Serialize(Student student)
    {
        return JsonSerializer.Serialize(student, _options);
    }

    public static Student? Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<Student>(json, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, Format, null);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            const string dateFormat = "yyyy-MM-dd";
            writer.WriteStringValue(value.ToString(dateFormat));
        }
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
