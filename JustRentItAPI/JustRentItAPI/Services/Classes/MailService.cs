using System.Net;
using JustRentItAPI.Models.DTOs;
using JustRentItAPI.Models.Entities;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using MailKit.Net.Imap;

namespace JustRentItAPI.Services.Classes
{
    public class MailService : JustRentItAPI.Services.Interfaces.IMailService
    {
        private readonly string _smtpHost;//שרת שדרכו נשלחות המייל
        private readonly int _smtpPort;//הפורט המתאים

        private readonly string _imapHost;
        private readonly string _smtpPassword;

        private readonly string _smtpUser;
        private readonly int _imapPort;

        private readonly string _smtpNoReply;
        private readonly string _smtpPasswordNoReply;

        private readonly string _baseUrl;

        public MailService(IConfiguration config)
        {
            _smtpHost = config["MailSettings:Host"];
            _smtpPort = int.Parse(config["MailSettings:Port"]);

            _imapHost = config["MailSettings:ImapHost"];
            _imapPort = int.Parse(config["MailSettings:ImapPort"]);

            _smtpUser = config["MailSettings:User"];
            _smtpPassword = config["MailSettings:Password"];

            _smtpNoReply = config["MailSettings:No-Reply"];
            _smtpPasswordNoReply = config["MailSettings:PasswordNo-Reply"];

            _baseUrl = config["FrontendUrl"];
        }


        /*        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body, string? fromEmail = null)
                {
                    var senderEmail = fromEmail ?? _smtpUser;
                    var senderPassword = (senderEmail == _smtpNoReply) ? _smtpPasswordNoReply : _smtpPassword;

                    var message = new MimeMessage
                    {
                        Subject = subject,
                        Body = new TextPart("html") { Text = body }
                    };

                    message.From.Add(new MailboxAddress("Just Rent It dress", senderEmail));
                    message.To.Add(MailboxAddress.Parse(toEmail));

                    try
                    {
                        using (var smtpClient = new SmtpClient())
                        {
                            await smtpClient.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
                            await smtpClient.AuthenticateAsync(senderEmail, senderPassword);
                            await smtpClient.SendAsync(message);
                            await smtpClient.DisconnectAsync(true);
                        }

                      *//*  using (var imapClient = new ImapClient())
                        {
                            await imapClient.ConnectAsync(_imapHost, _imapPort, true);
                            await imapClient.AuthenticateAsync(senderEmail, senderPassword);

                            var sentFolder = imapClient.GetFolder(SpecialFolder.Sent);
                            await sentFolder.OpenAsync(FolderAccess.ReadWrite);
                            await sentFolder.AppendAsync(message, MessageFlags.Seen);

                            await imapClient.DisconnectAsync(true);
                        }*//*

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Email sent and saved successfully",
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("MAIL FAILED: " + ex);
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Email failed: " + ex.Message,
                            StatusCode = HttpStatusCode.InternalServerError
                        };
                    }

                }
        */
        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body, string? fromEmail = null)
        {
            var traceId = Guid.NewGuid().ToString("N");
            var senderEmail = fromEmail ?? _smtpUser;
            var senderPassword = (senderEmail == _smtpNoReply) ? _smtpPasswordNoReply : _smtpPassword;

            var message = new MimeMessage();
            message.MessageId = MimeKit.Utils.MimeUtils.GenerateMessageId();
            message.Date = DateTimeOffset.UtcNow;
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };
            message.From.Add(new MailboxAddress("Just Rent It dress", senderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));

            try
            {
                var ips = await System.Net.Dns.GetHostAddressesAsync(_smtpHost);
                Console.WriteLine($"[MAIL:{traceId}] DNS LOOKUP: {_smtpHost} resolved to {string.Join(", ", ips.Select(x => x.ToString()))}");
            }
            catch (Exception dnsEx)
            {
                Console.WriteLine($"[MAIL:{traceId}] DNS FAILED: {dnsEx.Message}");
            }

            Console.WriteLine($"[MAIL:{traceId}] START to={toEmail} from={senderEmail} host={_smtpHost}:{_smtpPort}");

            try
            {
                using var smtpClient = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput()));

                smtpClient.Timeout = 15000;
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await smtpClient.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.SslOnConnect);
                Console.WriteLine($"[MAIL:{traceId}] CONNECTED OK");

                await smtpClient.AuthenticateAsync(senderEmail, senderPassword);
                Console.WriteLine($"[MAIL:{traceId}] AUTH OK");

                await smtpClient.SendAsync(message);
                Console.WriteLine($"[MAIL:{traceId}] SENT OK");

                await smtpClient.DisconnectAsync(true);
                return new Response { IsSuccess = true, Message = "Sent", StatusCode = HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MAIL:{traceId}] FULL ERROR: {ex}");
                return new Response { IsSuccess = false, Message = ex.Message, StatusCode = HttpStatusCode.InternalServerError };
            }
        }
        public async Task SendDressDeletedAsync(Dress dress)
        {
            if (dress.User == null || string.IsNullOrWhiteSpace(dress.User.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {dress.User.FirstName},<br/>

                            רצינו לעדכן שהשמלה שלך <strong>""{dress.Name}""</strong> נמחקה מהמערכת.<br/>

                            אם מדובר בטעות, או במידה שתרצי להחזיר אותה - אפשר ליצור איתנו קשר בכל זמן.<br/>

                            בברכה,<br>
                            <strong>Just Rent It dress</strong>

                            </div>";

            await SendEmailAsync(
                dress.User.Email,
                $"עדכון בנוגע לשמלה שלך - {dress.Name}",
                body
            );
        }

        public async Task SendDressActivatedAsync(Dress dress)
        {
            if (dress.User == null || string.IsNullOrWhiteSpace(dress.User.Email))
                return;

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>

                            שלום {dress.User.FirstName},<br/>
                            <br/>
                            שמחים לעדכן שהשמלה שלך <strong>""{dress.Name}""</strong> אושרה כעת והועלתה לאתר! 🎉<br/>
                            <br/>
                            היא זמינה כעת לצפייה על ידי כל משתמשי האתר.<br>
                            במידה ומשתמש יתעניין בשמלה שלך - תקבלי על כך עדכון ישירות למייל.<br/>
                            <br/>
                            תוכלי לראות את השמלה בלינק:<br>
                            <a href='{_baseUrl}dresses/{dress.DressID}' style='color:#000; font-weight:bold;'>לחצי כאן לצפייה בשמלה</a><br/>
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
                dress.User.Email,
                $"השמלה שלך אושרה והועלתה לאתר - {dress.Name}",
                body
            );
        }


        public async Task SendOwnerFollowUpAsync(string ownerEmail, string ownerName, string interestedName, string dressName, int dressId)
        {
            var subject = $"עדכון לגבי השמלה \"{dressName}\" באתר Just Rent It dress";

            var dressUrl = $"{_baseUrl}dresses/{dressId}";

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
                            תודה מראש,<br/>
                            צוות Just Rent It dress
                        </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }

        public async Task SendUserFollowUpAsync(string userEmail, string userName, string dressName, int dressId)
        {
            var subject = "שמלה שהתעניינת בה באתר Just Rent It dress";

            var dressUrl = $"{_baseUrl}dresses/{dressId}";

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
                            תודה רבה,<br/>
                            צוות Just Rent It dress
                        </div>";

            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task SendUserInterestAsync(User owner, User user, Dress dress)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
                return;

            var dressUrl = $"{_baseUrl}dresses/{dress.DressID}";

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>
                            שלום {user.FirstName},<br/>
                            תודה על ההתעניינות בשמלה {dress.Name} ✨
                            <br/>
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dress.Name}
                            </a>
                            <br/>
                            <br/>
                            <strong>פרטי הקשר של בעלת השמלה:</strong><br>
                            • שם: {owner.FirstName} {owner.LastName}<br>
                            • אימייל: {owner.Email}<br>
                            • טלפון: {owner.Phone}<br/>
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
                user.Email,
                "פרטי השמלה שבחרת ב-Just Rent It dress",
                body
            );
        }

        public async Task SendOwnerInterestAsync(User owner, User user, Dress dress, string? message)
        {
            if (string.IsNullOrWhiteSpace(owner.Email))
                return;

            var dressUrl = $"{_baseUrl}dresses/{dress.DressID}";

            string body = $@"
                            <div style='font-family: Heebo, Arial, sans-serif; direction: rtl; text-align: right; line-height: 1.7;'>
                            שלום {owner.FirstName},<br/>
                            <br/>
                            רצינו לעדכן אותך ש־{user.FirstName} {user.LastName} התעניינה בשמלה שלך ""{dress.Name}"" וצפויה ליצור איתך קשר בהמשך.<br/>
                            <a href='{dressUrl}' target='_blank' style='color:#6b4eff;'>
                                {dress.Name}
                            </a>
                            <br/>
                            <br/>
                            <strong>פרטי המתעניינת:</strong><br>
                            • שם: {user.FirstName} {user.LastName}<br>
                            • אימייל: {user.Email}<br>
                            • טלפון: {user.Phone}<br>
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
                owner.Email,
                $"עדכון מאתר Just Rent It dress" +
                $" – התעניינות חדשה בשמלה שלך {dress.Name}",
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
                                בברכה,<br/>
                                צוות Just Rent It dress
                            </div>";

            await SendEmailAsync(ownerEmail, subject, body);
        }


        public async Task SendOwnerMonthlySummaryAsync(string ownerEmail, string ownerName, List<(string DressName, string DressUrl, List<string> InterestedNames)> dresses)
        {
            var body = BuildOwnerSummaryEmail(ownerName, dresses);

            await SendEmailAsync(
                ownerEmail,
                "סיכום חודשי - התעניינויות בשמלות שלך באתר Just Rent It dress",
                body
            );
        }

        public async Task SendUserMonthlySummaryAsync(string userEmail, string userName, List<(string Name, string Url)> dresses)
        {
            if (dresses == null || dresses.Count == 0)
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
                        <br/>
                        צוות Just Rent It dress
                    </div>";
        }

        private string BuildUserSummaryEmail(string userName, List<(string Name, string Url)> dresses)
        {
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
                        תודה רבה,<br/>
                        צוות Just Rent It dress
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
            return await SendEmailAsync(user.Email, subject, body, _smtpNoReply);
        }
    }
}