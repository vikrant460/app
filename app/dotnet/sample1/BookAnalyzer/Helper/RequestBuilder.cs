using Microsoft.Extensions.AI;
using static BookAnalyzer.Helper.PromptBuilder;
namespace BookAnalyzer.Helper;
public static class RequestBuilder
{
    public static ChatOptions CreateRequestOptions()
    {
        return new ChatOptions
        {
            Temperature = 0.0f,
            ResponseFormat = ChatResponseFormat.Json
        };
    }

    public static List<ChatMessage> CreateRequest(string extractedText)
    {
        return new List<ChatMessage>
        {
            new(ChatRole.System, "You are a precise data extractor. You MUST format responses strictly as JSON with key names: 'Summary', 'CoreReview', and 'KeyQuotes'."),
            new(ChatRole.User,  extractedText.Length <= 10000 ? BuildPrompt(extractedText) : BuildPrompt(extractedText.Substring(0, 10000)))
        };
    }
}