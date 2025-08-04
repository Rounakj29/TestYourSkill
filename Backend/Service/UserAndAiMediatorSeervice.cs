using Backend.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Backend.Service
{
    public class UserAndAiMediatorSeervice : IUserAndAiMediatorSeervice
    {
        private readonly IConfiguration _configuration;

        public UserAndAiMediatorSeervice(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> ProcessUserRequestAsync(UserRequests userRequest)
        {
            if (userRequest == null)
            {
                return "Invalid request: User request cannot be null.";
            }

            // Validate required fields (customize as needed)
            if (userRequest.TechStack == null || userRequest.TechStack.Count == 0)
            {
                return "Please specify at least one technology in your tech stack.";
            }

            try
            {
                // Build prompt from userRequest
                var prompt = BuildPromptFromUserRequest(userRequest);

                // Initialize Semantic Kernel with Azure OpenAI
                var openAiEndpoint = _configuration.GetRequiredSection("AZURE_OPENAI_ENDPOINT").Value;
                var openAiKey = _configuration.GetRequiredSection("AZURE_OPENAI_KEY").Value;

                IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    "gpt-4.1-nano",
                    openAiEndpoint,
                    openAiKey
                );
                Kernel kernel = kernelBuilder.Build();

                // Prepare chat history with user prompt
                ChatHistory history = [];
                history.AddUserMessage(prompt);

                var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                var response = await chatCompletionService.GetChatMessageContentAsync(
                    history,
                    kernel: kernel
                );
                Console.WriteLine(response.Content);
                return response.Content ?? "AI did not return a response.";
            }
            catch (Exception ex)
            {
                // Log exception as needed
                return $"An error occurred while processing your request: {ex.Message}";
            }
        }

        private string BuildPromptFromUserRequest(UserRequests userRequest)
        {
            // Customize the prompt as needed for your use case
            var techStack = string.Join(", ", userRequest.TechStack ?? []);
            var experience = userRequest.Experience.HasValue ? $"{userRequest.Experience} years" : "unspecified experience";
            var roleOrCompany = userRequest.RoleOrCompany ?? "unspecified role/company";
            var noOfQuestion = userRequest.NoOfQuestion.HasValue ? userRequest.NoOfQuestion.ToString() : "an unspecified number of";
            var additional = userRequest.AdditionalRequirements ?? "";
            var aiPrompt = $@"
You are an expert technical interviewer.

Goal: Generate a set of interview questions tailored to the following user input:
- Role/Company: {roleOrCompany}
- Experience: {experience}
- Technology Stack: {techStack}
- Number of Questions: {noOfQuestion}
- Additional Requirements: {additional}

Instructions:
- Create exactly {noOfQuestion} interview questions.
- Ensure a mix of question formats: short answer, long answer, multiple choice (MCQ), and coding questions.
- For each question, provide the following fields:
    - question_no (integer)
    - question (string)
    - type (one of: short, long, MCQ, Code, Others)
    - options (array of strings, only for MCQ)
    - code_snippet (string, only for Code type)
    - explanation (string, for long-form or complex questions)
    - difficulty (easy, medium, or hard)
    - tags (array of strings, optional)
    - answer: 
        - how_to_answer (string)
        - answer_text (string)
- Return only valid JSON in the following structure (no comments or extra text):

{{
  ""questions"": [
    {{
      ""question_no"": 1,
      ""question"": ""..."",
      ""type"": ""..."",
      ""options"": [""..."", ""...""],
      ""code_snippet"": ""..."",
      ""explanation"": ""..."",
      ""difficulty"": ""..."",
      ""tags"": [""..."", ""...""],
      ""answer"": {{
        ""how_to_answer"": ""..."",
        ""answer_text"": ""..."",
      }}
    }},
    ...
  ]
}}
";
            return aiPrompt;

           // return $"{aiPrompt}\n\n" +
             //      $"Generate {noOfQuestion} interview questions for a {roleOrCompany} with {experience} experience in {techStack}. " +
               //    $"Additionally: {additional}";

            //Sample userRequest: for C# 3yrs
            // {
            //     "TechStack": ["C#"],
            //     "Experience": 3,
            //     "RoleOrCompany": "Software Engineer",
            //     "NoOfQuestion": 5,
            //     "AdditionalRequirements": "Focus on .NET Core and ASP.NET"
            // }
        }
    }

    public interface IUserAndAiMediatorSeervice
    {
        Task<string> ProcessUserRequestAsync(UserRequests userRequest);
    }
}