using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

class SerializatorAndDesierializator
{
    static void Main(string[] args)
    {
        var student = new Student
        {
            FirstName = "Шурик",
            LastName = "Густов",
            BirthDate = new DateTime(1924, 04, 03),
            Grades = new List<Subject>
            {
                new Subject { Name = "Физра", Grade = 5 },
                new Subject { Name = "Информатика", Grade = 5 }
            }
        };

        try
        {
            ValidateStudent(student);

            string json = JsonService.Serialize(student);
            Console.WriteLine("Serialized JSON:");
            Console.WriteLine(json);

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "legend_info.json");
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Data saved to: {filePath}");

            Console.WriteLine("Reading data from file...");
            string jsonFromFile = File.ReadAllText(filePath);

            var deserializedStudent = JsonService.Deserialize(jsonFromFile);

            Console.WriteLine("\nDeserialized Student Record:");
            Console.WriteLine($"First Name: {deserializedStudent?.FirstName}");
            Console.WriteLine($"Last Name: {deserializedStudent?.LastName}");
            Console.WriteLine($"Birth Date: {deserializedStudent?.BirthDate:yyyy-MM-dd}");

            Console.WriteLine("Subjects:");
            foreach (var subject in deserializedStudent?.Grades ?? Array.Empty<Subject>())
            {
                Console.WriteLine($"  {subject.Name}: {subject.Grade}");
            }
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation Error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }

    static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            throw new ValidationException("First Name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(student.LastName))
        {
            throw new ValidationException("Last Name cannot be empty.");
        }
        if (student.BirthDate > DateTime.Now)
        {
            throw new ValidationException("Birth Date cannot be in the future.");
        }
        if (student.Grades == null || student.Grades.Count == 0)
        {
            throw new ValidationException("At least one subject must be specified.");
        }
    }
}
