using Xunit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class SerializationTests
{
    [Fact]
    public void CanSerializeAndDeserializeStudentRecord()
    {
        var originalStudent = new Student
        {
            FirstName = "Vivien",
            LastName = "Leigh",
            BirthDate = new DateTime(2006, 11, 05),
            Grades = new List<Subject>
            {
                new Subject { Name = "Algebra", Grade = 5 },
                new Subject { Name = "Literature", Grade = 4 }
            }
        };

        string json = JsonService.Serialize(originalStudent);
        Student? deserializedStudent = JsonService.Deserialize(json);
        if (deserializedStudent != null)
        {


            Assert.NotNull(deserializedStudent);
            Assert.Equal("Vivien", deserializedStudent.FirstName);
            Assert.Equal("Leigh", deserializedStudent.LastName);
            Assert.Equal(new DateTime(2006, 11, 05), deserializedStudent.BirthDate);
            Assert.Equal(2, deserializedStudent.Grades?.Count);
        }
    }

    [Fact]
    public void SerializationIgnoresNullByDefault()
    {
        var student = new Student
        {
            FirstName = "Роман",
            LastName = "Иванов",
            BirthDate = new DateTime(2006, 04, 13),
            Grades = null // Null Grades
        };

        string json = JsonService.Serialize(student);

        Assert.DoesNotContain("Grades", json);  
    }

    [Fact]
    public void DeserializationHandlesMissingData()
    {

        string json = @"{""FirstName"": ""Петя"", ""LastName"": ""Сидоров"", ""BirthDate"": ""1907-05-12""}";

        Student? student = JsonService.Deserialize(json);

        Assert.NotNull(student);
        Assert.Equal("Петя", student.FirstName);
        Assert.Equal("Сидоров", student.LastName);
        Assert.Equal(new DateTime(1907, 05, 12), student.BirthDate);
        Assert.Null(student.Grades); 
    }
}
