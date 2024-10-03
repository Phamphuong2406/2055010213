namespace Project_Summer.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message); //hàm gửi email
        //email:mail nhận, subject: tiêu đề mail,message:nội dung
    }
}
