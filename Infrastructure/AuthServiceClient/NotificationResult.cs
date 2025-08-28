namespace Infrastructure.AuthServiceClient;

public class NotificationResult
{
    public string Error { get; set; }
    public bool IsSuccess { get; set; }

    public static NotificationResult Success()
        => new NotificationResult { IsSuccess = true };
    
    public static NotificationResult Failure(string error)
        => new NotificationResult { IsSuccess = false, Error = error };
}