
public class ChatbotMessage
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsFromChatbot { get; set; } 

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}