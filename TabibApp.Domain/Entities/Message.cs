
public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public   string Name { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    public bool IsRead { get; set; }
    public string ReceiverId { get; set; } 
    public ApplicationUser Receiver { get; set; } 


}
