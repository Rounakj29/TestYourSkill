using Backend.Models;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {
        private readonly IUserAndAiMediatorService _mediatorService;
        private readonly string VerificationKey = "Tacos";
        public UserRequestController(IUserAndAiMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        // GET: api/<UserRequestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserRequestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserRequestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequests value)
        {
            if (value == null)
                return BadRequest("Request body cannot be null.");

            try
            {
                var result = await _mediatorService.GenerateInterviewQuestionsAsync(value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Optionally log here if you inject ILogger<UserRequestController>
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("VerifyAccess")]
        public Task<IActionResult> VerifyAccess(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return Task.FromResult<IActionResult>(BadRequest("Request body cannot be null."));
            }
            var result = value.Equals(VerificationKey, StringComparison.OrdinalIgnoreCase)
                ? "Access Granted"
                : "Access Denied";
            bool isAccessGranted = result.Equals("Access Granted", StringComparison.OrdinalIgnoreCase);

            return Task.FromResult<IActionResult>(isAccessGranted ? Ok(true) : Unauthorized(false));
        }

        [HttpPost("ExportToPdf ")]
        public async Task<IActionResult> ExportToPdf()
        {
            var value = @"
                    {
  ""questions"": [
    {
      ""question_no"": 1,
      ""question"": ""Explain the concept of async and await in C#. How do they help in improving application performance?"",
      ""type"": ""long"",
      ""options"": [],
      ""code_snippet"": """",
      ""explanation"": ""Async and await enable asynchronous programming in C# by allowing methods to run asynchronously, freeing up the calling thread (often the UI thread) to remain responsive. They help in improving application scalability and performance, especially in I/O-bound operations."",
      ""difficulty"": ""medium"",
      ""tags"": [""asynchronous programming"", ""performance"", ""C#""],
      ""answer"": {
        ""how_to_answer"": ""Describe asynchronous programming concepts in C#, the purpose of async and await keywords, and how they facilitate non-blocking operations."",
        ""answer_text"": ""Async and await are used to write asynchronous code that doesn't block the main thread. When a method marked with async awaits an operation, it pauses execution without blocking, allowing other tasks to run. Once the awaited task completes, execution resumes, leading to more responsive applications and efficient resource utilization.""
      }
    },
    {
      ""question_no"": 2,
      ""question"": ""What is the difference between an interface and an abstract class in C#? Provide examples of when you would use each."",
      ""type"": ""short"",
      ""options"": [],
      ""code_snippet"": """",
      ""explanation"": ""Interfaces define a contract without implementation, suitable for defining capabilities across unrelated classes. Abstract classes can provide base implementations and share state, ideal for related classes with common functionality."",
      ""difficulty"": ""medium"",
      ""tags"": [""object-oriented design"", ""inheritance"", ""C#""],
      ""answer"": {
        ""how_to_answer"": ""Explain the fundamental differences and provide practical scenarios or code examples illustrating their use cases."",
        ""answer_text"": ""An interface in C# declares methods and properties without implementation, allowing multiple classes to implement the same contract. An abstract class can contain both abstract and concrete members, enabling code reuse among related classes. Use interfaces for defining capabilities regardless of inheritance, and abstract classes when sharing code among related classes.""
      }
    },
    {
      ""question_no"": 3,
      ""question"": ""Which of the following is NOT a value type in C#?"",
      ""type"": ""MCQ"",
      ""options"": [""int"", ""double"", ""string"", ""bool""],
      ""code_snippet"": """",
      ""explanation"": ""In C#, value types directly contain data, whereas string is a reference type representing an object with its own memory management."",
      ""difficulty"": ""easy"",
      ""tags"": [""value types"", ""C#""],
      ""answer"": {
        ""how_to_answer"": ""Identify which options are value types and which is not, based on C# data types."",
        ""answer_text"": ""String is a reference type in C#, while int, double, and bool are value types.""
      }
    },
    {
      ""question_no"": 4,
      ""question"": ""Write a C# method that reads a list of integers and returns the list sorted in descending order."",
      ""type"": ""Code"",
      ""options"": [],
      ""code_snippet"": ""public List<int> SortDescending(List<int> numbers) {\n    // Your code here\n}"",
      ""explanation"": ""This question tests your ability to manipulate collections, use LINQ or sorting algorithms, and write clean, functional C# code."",
      ""difficulty"": ""medium"",
      ""tags"": [""collections"", ""LINQ"", ""sorting"", ""C#""],
      ""answer"": {
        ""how_to_answer"": ""Provide a complete method implementation that takes a list and sorts it in descending order."",
        ""answer_text"": ""public List<int> SortDescending(List<int> numbers) {\n    return numbers.OrderByDescending(n => n).ToList();\n}""
      }
    },
    {
      ""question_no"": 5,
      ""question"": ""Describe the concept of Dependency Injection (DI) in C#. What are its benefits and typical use cases?"",
      ""type"": ""long"",
      ""options"": [],
      ""code_snippet"": """",
      ""explanation"": ""Dependency Injection is a design pattern that promotes loose coupling by providing dependencies to classes from external sources, improving testability and maintainability."",
      ""difficulty"": ""hard"",
      ""tags"": [""design patterns"", ""DI"", ""software architecture""],
      ""answer"": {
        ""how_to_answer"": ""Explain DI principles, advantages, and give examples of scenarios where DI is beneficial."",
        ""answer_text"": ""Dependency Injection in C# involves supplying an object's dependencies from outside rather than creating them internally. It allows for easier testing, better modularity, and easier maintenance. Commonly implemented via constructor injection, setter injection, or through DI frameworks like Autofac or Microsoft.Extensions.DependencyInjection. It is used in scenarios such as service registration, decoupling components, and managing complex object graphs.""
      }
    }
  ]
}
            ";
            if (string.IsNullOrWhiteSpace(value))
            {
                return BadRequest("Request body cannot be null.");
            }
            var jsonResult = await _mediatorService.ExportQuestionsToPdfAsync(value);
            return Ok(jsonResult);
        }
        // PUT api/<UserRequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
