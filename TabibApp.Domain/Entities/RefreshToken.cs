namespace TabibApp.Application.Dtos;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    bool IsExpired => DateTime.UtcNow >= ExpiresOn;

    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; } // Nullable DateTime
    public bool IsActive => RevokedOn == null && !IsExpired;
}