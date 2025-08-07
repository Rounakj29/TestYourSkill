using Backend.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace Backend.Service
{
    public class UserAndAiMediatorService : IUserAndAiMediatorService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserAndAiMediatorService> _logger;
        private readonly string _llmModel = "gpt-4.1-nano";
        private string _aiResponse = string.Empty;
        public UserAndAiMediatorService(IConfiguration configuration, ILogger<UserAndAiMediatorService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> GenerateInterviewQuestionsAsync(UserRequests userRequest)
        {
            if (userRequest == null)
            {
                _logger.LogError("User request is null.");
                return "Invalid request: User request cannot be null.";
            }

            // Validate required fields (customize as needed)
            if (userRequest.TechStack == null || userRequest.TechStack.Count == 0)
            {
                _logger.LogError("Tech stack is empty in user request.");
                return "Please specify at least one technology in your tech stack.";
            }

            try
            {
                // Build prompt from userRequest
                var prompt = BuildPromptFromUserRequest(userRequest);

                _logger.LogInformation("Generated prompt for AI: {Prompt}", prompt);

                // Configuration Access (use strongly-typed access)
                var openAiEndpoint = _configuration["AZURE_OPENAI_ENDPOINT"];
                var openAiKey = _configuration["AZURE_OPENAI_KEY"];

                if (string.IsNullOrWhiteSpace(openAiEndpoint) || string.IsNullOrWhiteSpace(openAiKey))
                {
                    _logger.LogError("Azure OpenAI configuration is missing.");
                    return "AI service configuration is missing.";
                }

                IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddAzureOpenAIChatCompletion(
                    _llmModel,
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

                _logger.LogInformation("AI response: {Response}", response.Content);
                if (string.IsNullOrWhiteSpace(response.Content))
                {
                    _logger.LogWarning("AI response is empty or null.");
                    return "AI did not return a valid response.";
                }
                _aiResponse = response.Content;
                return response.Content ?? "AI did not return a response.";

            }
            catch (Exception ex)
            {
                // Log exception as needed
                _logger.LogError(ex, "Error processing user request: {Request}", userRequest);
                return $"An error occurred while processing your request: {ex.Message}";
            }
        }

        private string BuildPromptFromUserRequest(UserRequests userRequest)
        {
            List<string> difficultyLevel = ["Easy", "Medium", "Hard", "All"];

            // Customize the prompt as needed for your use case
            var techStack = string.Join(", ", userRequest.TechStack ?? []);
            var experience = userRequest.Experience.HasValue ? $"{userRequest.Experience} years" : "unspecified experience";
            var roleOrCompany = userRequest.RoleOrCompany ?? "unspecified role/company";
            //Convert difficulty level to string if provided, otherwise default to "All"
            var difficultyLevelString = userRequest.DifficultyLevel.HasValue && userRequest.DifficultyLevel.Value >= 1 && userRequest.DifficultyLevel.Value <= 4
                ? difficultyLevel[userRequest.DifficultyLevel.Value - 1]
                : "All";
            var questionType = userRequest.QuestionType ?? "All types"; // Default to "All types" if not specified
            var noOfQuestion = userRequest.NoOfQuestion.HasValue ? userRequest.NoOfQuestion.ToString() : "an unspecified number of";
            var additional = userRequest.AdditionalRequirements ?? "";
            var aiPrompt = $$$"""
                You are an expert technical interviewer.

                Goal: Generate a set of interview questions tailored to the following user input:
                - Role/Company: {roleOrCompany}
                - Experience: {experience}
                - Technology Stack: {techStack}
                - Difficulty Level: {difficultyLevelString}
                - Question Type: {questionType}
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
                  "questions": [
                    {{
                      "question_no": 1,
                      "question": "...",
                      "type": "...",
                      "options": ["...", "..."],
                      "code_snippet": "...",
                      "explanation": "...",
                      "difficulty": "...",
                      "tags": ["...", "..."],
                      "answer": {{
                        "how_to_answer": "...",
                        "answer_text": "..."
                      }}
                    }},
                    ...
                  ]
                }}
                """;
            return aiPrompt;
        }

        public Task<string> ExportQuestionsToPdfAsync(string aiResponse)
        {
            try
            {
                // Example: Validate JSON structure
                var jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(aiResponse);
                if (!jsonResponse.TryGetProperty("questions", out var questions) || questions.GetArrayLength() == 0)
                {
                    _logger.LogError("No questions found in AI response: {Response}", aiResponse);
                    return Task.FromResult("No questions found in the AI response.");
                }

                // PDF export logic would go here

                return Task.FromResult(aiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting AI response to PDF format.");
                return Task.FromResult("An error occurred while converting the response to PDF format.");
            }
        }
    }

    public interface IUserAndAiMediatorService
    {
        Task<string> GenerateInterviewQuestionsAsync(UserRequests userRequest);
        Task<string> ExportQuestionsToPdfAsync(string aiResponse);
    }
}