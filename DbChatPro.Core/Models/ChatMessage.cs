namespace DBChatPro.Models
{
    public class ChatMessage
    {
        public string Role { get; set; }
        public string Text { get; set; }

        public ChatMessage(string role, string text)
        {
            Role = role;
            Text = text;
        }
    }
}
