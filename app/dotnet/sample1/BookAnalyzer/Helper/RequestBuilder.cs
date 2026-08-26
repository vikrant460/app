using Microsoft.Extensions.AI;
using static BookAnalyzer.Helper.PromptBuilder;
namespace BookAnalyzer.Helper;
public class RequestBuilder
{
    private readonly string bookContent;

    public RequestBuilder(string bookContent)
    {
        this.bookContent = bookContent;
    }
    public  ChatOptions Options => new ChatOptions
        {
            Temperature = 0.0f,
            ResponseFormat = ChatResponseFormat.Json
        };

    public List<ChatMessage> Messages => new List<ChatMessage>
    {
        new ChatMessage(ChatRole.System, "You are a helpful assistant that analyzes books and provides summaries, core reviews, and key quotes."),
        new ChatMessage(ChatRole.User, BuildPrompt(bookContent))
    };
}
