using JustRentItAPI.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;

namespace JustRentItAPI.Services.Classes
{
    public class MailService : IMailService
    {

        private readonly string _smtpHost;//שרת שדרכו נשלחות המייל
        private readonly int _smtpPort;//הפורט המתאים
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;


        public MailService(IConfiguration config)
        {
            _smtpHost = config["MailSettings:Host"];
            _smtpPort = int.Parse(config["MailSettings:Port"]);
            _smtpUser = config["MailSettings:User"];
            _smtpPassword = config["MailSettings:Password"];
            _fromEmail = config["MailSettings:From"];
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {

                using var client = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
                    EnableSsl = true//קונקשן מאובטח
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Email sent successfully",
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = $"Failed to send email: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task SendDressDeletedAsync(Dress dress)
        {
            if (dress.User == null || string.IsNullOrWhiteSpace(dress.User.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {dress.User.FirstName},<br><br>

                            רצינו לעדכן שהשמלה שלך <strong>""{dress.Name}""</strong> נמחקה מהמערכת.<br><br>

                            אם מדובר בטעות, או במידה שתרצי להחזיר אותה - אפשר ליצור איתנו קשר בכל זמן.<br><br>

                            בברכה,<br>
                            <strong>Just Rent It</strong>

                            </div>";

            await SendEmailAsync(
                dress.User.Email,
                $"עדכון בנוגע לשמלה שלך - {dress.Name}",
                body
            );
        }

        public async Task SendDressActivatedAsync(Dress dress, string baseUrl)
        {
            if (dress.User == null || string.IsNullOrWhiteSpace(dress.User.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {dress.User.FirstName},<br><br>

                            שמחים לעדכן שהשמלה שלך <strong>""{dress.Name}""</strong> אושרה כעת והועלתה לאתר! 🎉<br><br>

                            היא זמינה כעת לצפייה על ידי כל משתמשי האתר.<br>
                            במידה ומשתמש יתעניין בשמלה שלך - תקבלי על כך עדכון ישירות למייל.<br><br>

                            תוכלי לראות את השמלה בלינק:<br>
                            <a href='{baseUrl}dresses/{dress.DressID}' style='color:#000; font-weight:bold;'>לחצי כאן לצפייה בשמלה</a><br><br>

                            אם יש שינוי שתרצי לבצע בשמלה (מחיר, תמונות, פרטים) - ניתן לערוך אותה בכל זמן.<br><br>

                            <strong>חשוב לדעת:</strong><br>
                            במקרה של השכרה או קנייה דרך האתר, ישנה עמלה של <strong>15%</strong> ממחיר העסקה.<br><br>

                            בברכה,<br>
                            <strong>Just Rent It</strong>

                            </div>";

            await SendEmailAsync(
                dress.User.Email,
                $"השמלה שלך אושרה והועלתה לאתר - {dress.Name}",
                body
            );
        }


        public async Task SendOwnerFollowUpAsync(string ownerEmail, string ownerName, string interestedName, string dressName)
        {
            var subject = $"עדכון לגבי השמלה \"{dressName}\" באתר JustRentIt";

            var body = $@"
                        <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>

                            <p>נשמח לקבל עדכון לגבי השמלה שלך באתר JustRentIt</p>

                            <p>שלום {ownerName},</p>

                            <p>
                                שמנו לב ש{interestedName} התעניינה בשמלה שלך באתר.<br/>
                                כחלק משיתוף הפעולה, נשמח לדעת האם חל עדכון לגבי מצב השמלה - האם הושכרה או נמכרה?
                            </p>

                            <p>
                                תודה מראש,<br/>
                                צוות JustRentIt
                            </p>

                        </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }

        public async Task SendUserFollowUpAsync(string userEmail, string userName, string dressName)
        {
            var subject = "שמלה שהתעניינת בה באתר JUST-RENT-IT";

            var body = $@"
                        <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>

                            <p>שלום {userName},</p>

                            <p>
                            ראינו שהתעניינת בשמלה ""{dressName}"" באתר שלנו.<br/>
                            נשמח לשמוע אם יצא לך להשכיר או לקנות שמלה דרך האתר,<br/>
                            ולדעת איך הייתה לך החוויה באתר שלנו :)
                            </p>

                            <p>
                            תודה רבה,<br/>
                            צוות JustRentIt
                            </p>

                        </div>";

            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task SendUserInterestAsync(User owner, User user, Dress dress)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {user.FirstName},<br><br>

                            תודה על ההתעניינות בשמלה {dress.Name} ✨<br><br>

                            <strong>פרטי הקשר של בעלת השמלה:</strong><br>
                            • שם: {owner.FirstName} {owner.LastName}<br>
                            • אימייל: {owner.Email}<br>
                            • טלפון: {owner.Phone}<br><br>

                            <strong>בבקשה, כשאת יוצרת קשר עם בעלת השמלה צייני שהגעת דרך האתר JUST-RENT-IT</strong>.<br><br>

                            נשמח לשמוע ולהתעדכן מה קורה עם השמלה אהבת? השכרת? ספרי לנו! <br>
                            אם משהו לא ברור או שיש לך שאלה, אני כאן לכל דבר.<br><br>

                            בברכה,<br>
                            <strong>Just Rent It dress</strong>

                            </div>";

            await SendEmailAsync(
                user.Email,
                "פרטי השמלה שבחרת ב-Just Rent It dress",
                body
            );
        }

        public async Task SendOwnerInterestAsync(User owner, User user, Dress dress, string? message)
        {
            if (string.IsNullOrWhiteSpace(owner.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {owner.FirstName},<br><br>

                            רצינו לעדכן אותך ש־{user.FirstName} {user.LastName} התעניינה בשמלה שלך ""{dress.Name}"" וצפויה ליצור איתך קשר בהמשך.<br><br>

                            <strong>פרטי המתעניינת:</strong><br>
                            • שם: {user.FirstName} {user.LastName}<br>
                            • אימייל: {user.Email}<br>
                            • טלפון: {user.Phone}<br>
                            {(string.IsNullOrWhiteSpace(message) ? "" : $"• הודעה שצירפה: {message}<br>")}<br>

                            נשמח שתעדכני אותנו מה קורה בהמשך האם יצרתן קשר? האם השמלה הושכרה?<br><br>

                            במידה ולא נקבל עדכון מצידך, תישלח אלייך תזכורת אוטומטית.<br>
                            אם לא יתקבל עדכון גם לאחר התזכורת, השמלה עשויה לרדת מהאתר באופן זמני עד לקבלת מידע נוסף.<br><br>

                            <strong>חשוב לדעת:</strong><br>
                            במקרה של השכרה דרך האתר, תחול עמלה של 15% ממחיר ההשכרה, אותה יש להעביר בהעברה בנקאית. פרטי החשבון יימסרו במקרה של השכרה.<br><br>

                            לכל שאלה או צורך בעזרה אנחנו כאן בשבילך.<br><br>

                            בברכה,<br>
                            <strong>Just Rent It dress</strong>

                            </div>";

            await SendEmailAsync(
                owner.Email,
                $"עדכון מאתר JUST-RENT-IT – התעניינות חדשה בשמלה שלך {dress.Name}",
                body
            );
        }

        public async Task SendPaymentAsync(string ownerEmail, string ownerName)
        {
            if (string.IsNullOrWhiteSpace(ownerEmail))
                return;

            string subject = "הודעה על השכרת השמלה – יש להעביר את העמלה";

            string body = $@"
                            <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>

                                <p>שלום {ownerName},</p>

                                <p>איזה כיף! מישהי השכירה את השמלה שלך דרך האתר שלנו JustRentIt! ✨</p>

                                <p>
                                בהתאם לתנאי השימוש, יש לבצע העברה של 15% ממחיר ההשכרה או ממחיר הקנייה.
                                </p>

                                <p><b>פרטי הבנק להעברה:</b><br/>
                                בנק מזרחי<br/>
                                סניף 430<br/>
                                מספר חשבון: 446904<br/>
                                על שם: אביטל גולדרינג
                                </p>

                                <p><b>ניתן לבצע את התשלום גם דרך ביט:</b><br/>
                                058-3130909
                                </p>

                                <p>
                                נשמח לקבל צילום מסך לאישור העברה.<br/>
                                אם יש שאלות או משהו לא ברור, ניתן לפנות אלינו בכתובת:<br/>
                                just.rent.it1@gmail.com
                                </p>

                                <p>
                                בברכה,<br/>
                                צוות JustRentIt
                                </p>

                            </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }


        public async Task SendOwnerMonthlySummaryAsync(string ownerEmail, string ownerName, List<(string DressName, List<string> InterestedNames)> dressData)
        {
            var body = BuildOwnerSummaryEmail(ownerName, dressData);

            await SendEmailAsync(
                ownerEmail,
                "סיכום חודשי - התעניינויות בשמלות שלך באתר JUST-RENT-IT",
                body
            );
        }

        public async Task SendUserMonthlySummaryAsync( string userEmail,string userName,List<string> dressNames)
        {
            if (dressNames == null || dressNames.Count == 0)
                return;

            var body = BuildUserSummaryEmail(userName, dressNames);

            await SendEmailAsync(
                userEmail,
                "השמלות שהתעניינת בהן החודש באתר JustRentIt",
                body
            );
        }

        private string BuildOwnerSummaryEmail(string ownerName, List<(string DressName, List<string> InterestedNames)> dressData)
        {
            // בניית השורות לכל שמלה
            //nbsp Non-Breaking Space מייצר רווח
            //string.Join מחברת מחזורו
            var lines = dressData.Select(d =>
                $"• בשמלה {d.DressName} התעניינו:<br/>" +
                string.Join("<br/>", d.InterestedNames.Select(n => $"&nbsp;&nbsp;&nbsp;&nbsp;- {n}"))
            );

            var listHtml = string.Join("<br/><br/>", lines);

            return $@"
                    <div dir='rtl' style='font-family: Arial; font-size: 16px; line-height: 1.8;'>

                        <p>שלום {ownerName},</p>

                        <p>
                            ראינו שהחודש היו התעניינויות חדשות בשמלות שלך ✨<br/>
                            וריכזנו לך כאן את כולן במקום אחד:
                        </p>

                        <p>
                            {listHtml}
                        </p>

                        <p>
                            נשמח אם תעדכני אותנו אם מישהי יצרה איתך קשר ואם משהו התקדם.<br/>
                            תודה רבה על שיתוף הפעולה!
                        </p>

                        <p>
                            צוות JustRentIt
                        </p>

                    </div>";
        }

        private string BuildUserSummaryEmail(string userName, List<string> dressNames)
        {
            var listHtml = string.Join("<br/>", dressNames.Select(d => $"• {d}"));

            return $@"
                    <div dir='rtl' style='font-family: Arial; font-size: 16px; line-height: 1.6;'>

                        <p>שלום {userName},</p>

                        <p>
                            ראינו שהחודש התעניינת בכמה שמלות דרך האתר שלנו ✨<br/>
                            וריכזנו לך כאן את כולן:
                        </p>

                        <p>
                            {listHtml}
                        </p>

                        <p>
                            נשמח לשמוע אם יצא לך לשכור או לקנות אחת מהשמלות דרך האתר,<br/>
                            ולשמוע איך הייתה לך החוויה אצלנו :)
                        </p>

                        <p>
                            תודה רבה,<br/>
                            צוות JustRentIt
                        </p>

                    </div>";
        }


        public async Task<Response> SendPasswordResetEmailAsync(User user, string resetLink)
        {
            var subject = "בקשת איפוס סיסמה";
            var body = $@"
                        <div dir='rtl' style='font-family: Arial, sans-serif; text-align: right; line-height: 1.7;'>
                            <h2>שלום {user.FirstName},</h2>
                            <p>קיבלת בקשה לאיפוס הסיסמה שלך.</p>
                            <p>לחצי על הקישור הבא כדי לעדכן את הסיסמה:</p>
                            <a href='{resetLink}' style='color:#0000EE; text-decoration: underline;'>לחץ כאן</a>
                            <p>הקישור יפוג בעוד 15 דקות.</p>
                            <p>בברכה,<br/>צוות Just Rent It</p>
                        </div>
                    ";
            return await SendEmailAsync(user.Email, subject, body);
        }
    }
}