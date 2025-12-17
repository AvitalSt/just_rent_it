namespace JustRentItAPI.Validation
{
    static class EmailValidation
    {
        public static bool IsValidEmail(this string email)
        {
            if(string.IsNullOrEmpty(email))
                return false;
            try
            {
                //משתמשים בספריה שמייצגת כתובת מייל תקנית, הספריה מנסה לפרש את המזוחרת אם לא הצליח זורקת שיגאה
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address==email;
            }
            catch
            {
                return false;
            }
        }
    }
}
