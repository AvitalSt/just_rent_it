using System.Net;
using JustRentItAPI.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Response = JustRentItAPI.Models.DTOs.Response;

namespace JustRentItAPI.Services.Classes
{
    public class MailService : IMailService
    {
        private readonly string _sendGridApiKey;
        private readonly string _From;
        private readonly string _smtpNoReply;

        private readonly string _baseUrl;

        public MailService(IConfiguration config)
        {
            _From = config["MailSettings:From"];
            _sendGridApiKey = config["MailSettings:SendGridApiKey"];
            _smtpNoReply = config["MailSettings:NoReply"];

            _baseUrl = config["FrontendUrl"];
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body, string? fromEmail = null)
        {
            var client = new SendGridClient(_sendGridApiKey);

            var senderEmail = string.IsNullOrEmpty(fromEmail) ? _From : fromEmail;
            var from = new EmailAddress(senderEmail, "Just Rent It dress");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", body);

            try
            {
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    return new Response { IsSuccess = true, Message = "Sent", StatusCode = HttpStatusCode.OK };
                }

                var errorBody = await response.Body.ReadAsStringAsync();
                return new Response { IsSuccess = false, Message = "API Error", StatusCode = response.StatusCode };
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }

        public async Task SendDressDeletedAsync(string email, string firstName, string dressName)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(dressName))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {firstName},<br/>

                            רצינו לעדכן שהשמלה שלך <strong>""{dressName}""</strong> נמחקה מהמערכת.<br/>

                            אם מדובר בטעות, או במידה שתרצי להחזיר אותה - אפשר ליצור איתנו קשר בכל זמן.<br/>

                            בברכה,<br>
                            <strong>Just Rent It dress</strong>

                            </div>";

            await SendEmailAsync(
                email,
                $"עדכון בנוגע לשמלה שלך - {dressName}",
                body
            );
        }

        public async Task SendDressActivatedAsync(string email, string firstName, string dressName, int dressId)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(dressName) ||
                dressId <= 0)
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {firstName},<br/>
                            <br/>
                            שמחים לעדכן שהשמלה שלך <strong>""{dressName}""</strong> אושרה כעת והועלתה לאתר! 🎉<br/>
                            <br/>
                            היא זמינה כעת לצפייה על ידי כל משתמשי האתר.<br>
                            במידה ומשתמש יתעניין בשמלה שלך - תקבלי על כך עדכון ישירות למייל.<br/>
                            <br/>
                            תוכלי לראות את השמלה בלינק:<br>
                            <a href='{_baseUrl}/dresses/{dressId}' style='color:#000; font-weight:bold;'>לחצי כאן לצפייה בשמלה</a><br/>
                            <br/>
                            אם יש שינוי שתרצי לבצע בשמלה (מחיר, תמונות, פרטים) - ניתן לערוך אותה בכל זמן.<br/>
                            <br/>
                            <strong>חשוב לדעת:</strong><br>
                            במקרה של השכרה או קנייה דרך האתר, ישנה עמלה של <strong>15%</strong> ממחיר העסקה.<br/>
                            <br/>
                            בברכה,<br>
                            <strong>Just Rent It dress</strong>
                            </div>";

            await SendEmailAsync(
                email,
                $"השמלה שלך אושרה והועלתה לאתר - {dressName}",
                body
            );
        }


        public async Task SendOwnerFollowUpAsync(string ownerEmail, string ownerName, string interestedName, string dressName, int dressId)
        {
            if (string.IsNullOrWhiteSpace(ownerEmail) ||
                string.IsNullOrWhiteSpace(ownerName) ||
                string.IsNullOrWhiteSpace(interestedName) ||
                string.IsNullOrWhiteSpace(dressName) ||
                dressId <= 0)
                return;

            var subject = $"עדכון לגבי השמלה \"{dressName}\" באתר Just Rent It dress";

            var dressUrl = $"{_baseUrl}/dresses/{dressId}";

            var body = $@"
                        <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>
                            <br/>
                            שלום {ownerName},
                            <br/>
                            <br/>
                            שמנו לב ש{interestedName} התעניינה בשמלה שלך באתר.<br/>
                            <br/>
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dressName}
                            </a>
                            <br/><br/>
                            כחלק משיתוף הפעולה, נשמח לדעת האם חל עדכון לגבי מצב השמלה - האם הושכרה או נמכרה?
                            <br/>
                            <br/>
                             בברכה,<br>
                            <strong>Just Rent It dress</strong>
                        </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }

        public async Task SendUserFollowUpAsync(string userEmail, string userName, string dressName, int dressId)
        {
            if (string.IsNullOrWhiteSpace(userEmail) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(dressName) ||
                dressId <= 0)
                return;

            var subject = "שמלה שהתעניינת בה באתר Just Rent It dress";

            var dressUrl = $"{_baseUrl}/dresses/{dressId}";

            var body = $@"
                        <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>
                            שלום {userName},
                            <br/>
                            <br/>
                            ראינו שהתעניינת בשמלה ""{dressName}"" באתר שלנו.<br/>                         
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dressName}
                            </a>
                            <br/>
                            <br/>
                            נשמח לשמוע אם יצא לך להשכיר או לקנות שמלה דרך האתר,<br/>
                            ולדעת איך הייתה לך החוויה באתר שלנו :)
                            <br/>
                            <br/>
                             בברכה,<br>
                            <strong>Just Rent It dress</strong>
                        </div>";

            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task SendUserInterestAsync(string userEmail, string userFirstName, string dressName, int dressId, string ownerFirstName, string ownerLastName, string ownerEmail, string ownerPhone)
        {
            if (string.IsNullOrWhiteSpace(userEmail) ||
                string.IsNullOrWhiteSpace(userFirstName) ||
                string.IsNullOrWhiteSpace(dressName) ||
                dressId <= 0 ||
                string.IsNullOrWhiteSpace(ownerFirstName) ||
                string.IsNullOrWhiteSpace(ownerLastName) ||
                string.IsNullOrWhiteSpace(ownerEmail) ||
                string.IsNullOrWhiteSpace(ownerPhone))
                return;

            var dressUrl = $"{_baseUrl}/dresses/{dressId}";

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>
                            שלום {userFirstName},<br/>
                            תודה על ההתעניינות בשמלה {dressName} ✨
                            <br/>
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dressName}
                            </a>
                            <br/>
                            <br/>
                            <strong>פרטי הקשר של בעלת השמלה:</strong><br>
                            • שם: {ownerFirstName} {ownerLastName}<br>
                            • אימייל: {ownerEmail}<br>
                            • טלפון: {ownerPhone}<br/>
                            <br/>
                            <strong>בבקשה, כשאת יוצרת קשר עם בעלת השמלה צייני שהגעת דרך האתר Just Rent It dress</strong>.<br/>
                            <br/>
                            נשמח לשמוע ולהתעדכן מה קורה עם השמלה אהבת? השכרת? ספרי לנו! <br>
                            אם משהו לא ברור או שיש לך שאלה, אני כאן לכל דבר.<br/>
                            <br/>
                             בברכה,<br>
                            <strong>Just Rent It dress</strong>
                            </div>";

            await SendEmailAsync(
                userEmail,
                "פרטי השמלה שבחרת ב-Just Rent It dress",
                body
            );
        }

        public async Task SendOwnerInterestAsync(string ownerEmail, string ownerFirstName, string userFirstName, string userLastName, string userEmail, string userPhone, string dressName, int dressId, string? message)
        {
            if (string.IsNullOrWhiteSpace(ownerEmail) ||
                string.IsNullOrWhiteSpace(ownerFirstName) ||
                string.IsNullOrWhiteSpace(userFirstName) ||
                string.IsNullOrWhiteSpace(userLastName) ||
                string.IsNullOrWhiteSpace(userEmail) ||
                string.IsNullOrWhiteSpace(userPhone) ||
                string.IsNullOrWhiteSpace(dressName) ||
                dressId <= 0)
                return;

            var dressUrl = $"{_baseUrl}/dresses/{dressId}";

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>
                            שלום {ownerFirstName},<br/>
                            <br/>
                            רצינו לעדכן אותך ש־{userFirstName} {userLastName} התעניינה בשמלה שלך ""{dressName}"" וצפויה ליצור איתך קשר בהמשך.<br/>
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dressName}
                            </a>
                            <br/>
                            <br/>
                            <strong>פרטי המתעניינת:</strong><br>
                            • שם: {userFirstName} {userLastName}<br>
                            • אימייל: {userEmail}<br>
                            • טלפון: {userPhone}<br>
                            {(string.IsNullOrWhiteSpace(message) ? "" : $"• הודעה שצירפה: {message}<br>")}<br>
                            נשמח שתעדכני אותנו מה קורה בהמשך האם יצרתן קשר? האם השמלה הושכרה?<br/>
                            במידה ולא נקבל עדכון מצידך, תישלח אלייך תזכורת אוטומטית.<br>
                            אם לא יתקבל עדכון גם לאחר התזכורת, השמלה עשויה לרדת מהאתר באופן זמני עד לקבלת מידע נוסף.<br/><br/>
                            <strong>חשוב לדעת:</strong><br>
                            במקרה של השכרה דרך האתר, תחול עמלה של 15% ממחיר ההשכרה,<br/> אותה יש להעביר בהעברה בנקאית. פרטי החשבון יימסרו במקרה של השכרה.<br/>
                            <br/>
                            לכל שאלה או צורך בעזרה אנחנו כאן בשבילך.<br/>
                            <br/>
                            בברכה,<br>
                            <strong>Just Rent It dress</strong>
                            </div>";

            await SendEmailAsync(
                ownerEmail,
                $"עדכון מאתר Just Rent It dress" +
                $" – התעניינות חדשה בשמלה שלך {dressName}",
                body
            );
        }

        public async Task SendPaymentAsync(string ownerEmail, string ownerName)
        {
            if (string.IsNullOrWhiteSpace(ownerEmail) ||
                string.IsNullOrWhiteSpace(ownerName))
                return;

            string subject = "הודעה על השכרת השמלה – יש להעביר את העמלה";

            string body = $@"
                            <div style='direction: rtl; text-align: right; font-family: Arial, sans-serif; font-size: 15px;'>

                               שלום {ownerName},
                               <br/>
                               <br/>
                              איזה כיף! מישהי השכירה את השמלה שלך דרך האתר שלנו Just Rent It dress! ✨
                               <br/>                           
                                בהתאם לתנאי השימוש,  <br/>יש לבצע העברה של 15% ממחיר ההשכרה או ממחיר הקנייה.
                               <br/>
                                <p>
                                <b>פרטי הבנק להעברה:</b><br/>
                                בנק מזרחי סניף: 430<br/>
                                מספר חשבון: 446904<br/>
                                על שם: אביטל גולדרינג
                                </p>
                                <p><b>ניתן לבצע את התשלום גם דרך ביט:</b><br/>
                                058-3130909
                                </p>
                                נשמח לקבל צילום מסך לאישור העברה.<br/>                            
                               <br/>
                                בברכה,<br>
                            <strong>Just Rent It dress</strong>
                            </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }


        public async Task SendOwnerMonthlySummaryAsync(string ownerEmail, string ownerName, List<(string DressName, string DressUrl, List<string> InterestedNames)> dresses)
        {
            if (string.IsNullOrWhiteSpace(ownerEmail) ||
                string.IsNullOrWhiteSpace(ownerName) ||
                dresses == null || dresses.Count == 0)
                return;

            var body = BuildOwnerSummaryEmail(ownerName, dresses);

            await SendEmailAsync(
                ownerEmail,
                "סיכום חודשי - התעניינויות בשמלות שלך באתר Just Rent It dress",
                body
            );
        }

        public async Task SendUserMonthlySummaryAsync(string userEmail, string userName, List<(string Name, string Url)> dresses)
        {
            if (string.IsNullOrWhiteSpace(userEmail) ||
                string.IsNullOrWhiteSpace(userName) ||
                dresses == null || dresses.Count == 0)
                return;

            var body = BuildUserSummaryEmail(userName, dresses);

            await SendEmailAsync(
                userEmail,
                "השמלות שהתעניינת בהן החודש באתר Just Rent It dress",
                body
            );
        }

        private string BuildOwnerSummaryEmail(string ownerName, List<(string DressName, string DressUrl, List<string> InterestedNames)> dressData)
        {
            if (dressData == null || dressData.Count == 0)
                return string.Empty;
            // בניית השורות לכל שמלה
            //nbsp Non-Breaking Space מייצר רווח
            //string.Join מחברת מחזורו
            var lines = dressData.Select(d =>
                $"• בשמלה <a href='{d.DressUrl}' style='font-weight:bold;'>{d.DressName}</a> התעניינו:<br/>" +
                string.Join("<br/>", d.InterestedNames.Select(n => $"&nbsp;&nbsp;&nbsp;&nbsp;- {n}"))
            );

            var listHtml = string.Join("<br/><br/>", lines);

            return $@"
                    <div dir='rtl' style='font-family: Arial; font-size: 16px; line-height: 1.8;'>
                        שלום {ownerName},
                        <br/>
                        <br/>
                        ראינו שהחודש היו התעניינויות חדשות בשמלות שלך ✨<br/>
                        וריכזנו לך כאן את כולן במקום אחד:
                        <br/>
                        <br/>
                        {listHtml}
                        <br/>
                        <br/>
                        נשמח אם תעדכני אותנו אם מישהי יצרה איתך קשר ואם משהו התקדם.<br/>
                        תודה רבה על שיתוף הפעולה!
                        <br/>
                        בברכה,<br>
                            <strong>Just Rent It dress</strong>
                    </div>";
        }

        private string BuildUserSummaryEmail(string userName, List<(string Name, string Url)> dresses)
        {
            if (dresses == null || dresses.Count == 0)
                return string.Empty;

            var listHtml = string.Join("<br/>", dresses.Select(d => $"• <a href='{d.Url}' style='font-weight:bold;'>{d.Name}</a>"));

            return $@"
                    <div dir='rtl' style='font-family: Arial; font-size: 16px; line-height: 1.6;'>
                        שלום {userName},
                        <br/>
                        <br/>
                        ראינו שהחודש התעניינת בכמה שמלות דרך האתר שלנו ✨<br/>
                        וריכזנו לך כאן את כולן:
                        <br/>
                        <br/>
                        {listHtml}
                        <br/>
                        <br/>
                        נשמח לשמוע אם יצא לך לשכור או לקנות אחת מהשמלות דרך האתר,<br/>
                        ולשמוע איך הייתה לך החוויה אצלנו :)
                        <br/>
                        <br/>
                         בברכה,<br>
                            <strong>Just Rent It dress</strong>
                    </div>";
        }


        public async Task<Response> SendPasswordResetEmailAsync(string email, string firstName, string resetLink)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(resetLink))
                return new Response
                {
                    IsSuccess = false,
                    Message = "Missing email, name or reset link.",
                    StatusCode = HttpStatusCode.BadRequest
                };

            var subject = "בקשת איפוס סיסמה";
            var body = $@"
                        <div dir='rtl' style='font-family: Arial, sans-serif; text-align: right; line-height: 1.7;'>
                            <h2>שלום {firstName},</h2>
                            <p>קיבלת בקשה לאיפוס הסיסמה שלך.</p>
                            <p>לחצי על הקישור הבא כדי לעדכן את הסיסמה:</p>
                            <a href='{resetLink}' style='color:#0000EE; text-decoration: underline;'>לחץ כאן</a>
                            <p>הקישור יפוג בעוד 15 דקות.</p>
                            <p> בברכה,<br>
                            <strong>Just Rent It dress</strong></p>
                        </div>
                    ";
            return await SendEmailAsync(email, subject, body, _smtpNoReply);
        }
    }
}