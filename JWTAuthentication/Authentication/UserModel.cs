namespace JWTAuthentication.Authentication
{
    public class UserModel
    {
        public string UserName { get; set;}
        public string Password { get; set;}
        public string Email { get; set;}
        public string Date { get; set; } = DateTime.Today.ToString();


    }
}
