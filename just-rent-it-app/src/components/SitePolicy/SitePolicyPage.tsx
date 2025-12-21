import Link from "next/link";

export default function SitePolicy() {
  return (
    <div className="min-h-screen bg-white py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-4xl mx-auto bg-white rounded-2xl shadow-xl overflow-hidden">
        <div className="bg-gradient-to-br from-black to-neutral-800 text-white py-12 px-8 text-center">
          <h1 className="text-3xl sm:text-4xl font-bold mb-3 tracking-tight">
            תקנון האתר
          </h1>
          <p className="text-base sm:text-lg opacity-90 font-light">
            JustRentIt פלטפורמה שיתופית להשכרת שמלות
          </p>
        </div>

        <div className="py-10 px-8 sm:px-12">
          <div className="text-base leading-relaxed text-neutral-700 mb-10 p-6 bg-neutral-50 border-r-4 border-black rounded-lg">
            ברוכה הבאה ל-<strong>JustRentIt</strong> הפלטפורמה השיתופית המובילה
            להשכרת שמלות לאירועים.
            <br />
            האתר משמש מרחב אונליין שבו משתמשות יכולות להעלות שמלות להשכרה,{" "}
            ובמקביל לחפש את השמלה המושלמת לאירוע הבא שלהן.
          </div>

          <section className="mb-10">
            <h2 className="text-2xl font-bold text-black mb-6 pb-3 border-b-2 border-black inline-block">
              העלאת שמלה
            </h2>

            <div className="space-y-6">
              <div className="p-5 bg-neutral-50 rounded-xl hover:translate-x-[-4px] transition-transform duration-300">
                <h3 className="text-lg font-semibold text-neutral-900 mb-3 flex items-center">
                  <span className="w-2 h-2 bg-black rounded-full ml-2"></span>
                  פרטי התקשרות
                </h3>
                <p className="text-base leading-relaxed text-neutral-600">
                  הפרטים איתם נרשמת לאתר (שם, טלפון, אימייל) הם הפרטים שיועברו
                  למתעניינת בשמלה שלך, כדי שתוכל ליצור איתך קשר ישיר.
                  <br />
                  ניתן לעדכן את הפרטים בכל עת דרך אזור {""}
                  <Link href="/profile" className="text-blue-600 underline">
                    המשתמש
                  </Link>
                </p>
              </div>

              <div className="p-5 bg-neutral-50 rounded-xl hover:translate-x-[-4px] transition-transform duration-300">
                <h3 className="text-lg font-semibold text-neutral-900 mb-3 flex items-center">
                  <span className="w-2 h-2 bg-black rounded-full ml-2"></span>
                  עמלות
                </h3>
                <p className="text-base leading-relaxed text-neutral-600">
                  במידה ותתבצע השכרה או קנייה של השמלה דרך האתר - תחויבי בעמלה
                  של <strong className="text-black">15% ממחיר העסקה</strong>.
                  <br />
                  העמלה תשולם באמצעות העברה בנקאית או באמצעי תשלום אחר שיוגדר.
                  <br />
                  פרטים נוספים יימסרו בעת הצורך.
                  <br />
                  <br />
                  <strong className="text-black">
                    את אחראית לשלם את העמלה בהקדם האפשרי - אי תשלום ייחשב לגזל
                    גמור!
                  </strong>
                </p>
              </div>

              <div className="p-5 bg-neutral-50 rounded-xl hover:translate-x-[-4px] transition-transform duration-300">
                <h3 className="text-lg font-semibold text-neutral-900 mb-3 flex items-center">
                  <span className="w-2 h-2 bg-black rounded-full ml-2"></span>
                  אישור שמלה
                </h3>
                <p className="text-base leading-relaxed text-neutral-600">
                  לאחר העלאת שמלה, צוות האתר בודק ומאשר אותה.
                  <br /> עם האישור יישלח אלייך מייל הכולל קישור ישיר לשמלה שלך.
                </p>
              </div>
            </div>
          </section>

          <div className="h-px bg-gradient-to-l from-transparent via-black to-transparent my-8"></div>

          <section className="mb-10">
            <h2 className="text-2xl font-bold text-black mb-6 pb-3 border-b-2 border-black inline-block">
              התעניינות בשמלה
            </h2>

            <div className="space-y-6">
              <div className="p-5 bg-neutral-50 rounded-xl hover:translate-x-[-4px] transition-transform duration-300">
                <h3 className="text-lg font-semibold text-neutral-900 mb-3 flex items-center">
                  <span className="w-2 h-2 bg-black rounded-full ml-2"></span>
                  קבלת עדכון
                </h3>
                <p className="text-base leading-relaxed text-neutral-600">
                  כאשר משתמשת מתעניינת בשמלה שלך - תקבלי מהאתר הודעה על פנייה
                  חדשה.
                  <br /> בנוסף, יישלח אלייך מייל עם כל פרטי המתעניינת (שם,
                  טלפון, אימייל). <br />
                  <br />
                  לכל משתמשת יש מספר מוגבל של פניות - כדי למנוע עומס ולשמור על
                  איכות ההתעניינות.
                </p>
              </div>

              <div className="p-5 bg-neutral-50 rounded-xl hover:translate-x-[-4px] transition-transform duration-300">
                <h3 className="text-lg font-semibold text-neutral-900 mb-3 flex items-center">
                  <span className="w-2 h-2 bg-black rounded-full ml-2"></span>
                  חשיבות עדכון מצב השמלה
                </h3>
                <p className="text-base leading-relaxed text-neutral-600">
                  חשוב לעדכן האם השמלה הושכרה/נמכרה או עדיין פנויה.
                  <br />
                  בסוף כל חודש תקבלי מהאתר{" "}
                  <strong className="text-black">
                    סיכום חודשי על כל ההתעניינויות שהיו בשמלות שלך
                  </strong>
                  (במידה והיו),
                  <br />
                  ותעדכני האם יצרו איתך קשר והאם בוצעה השכרה/מכירה.
                  <br />
                  <br />
                  במידה ולא יתקבל עדכון לאורך זמן -{" "}
                  <strong className="text-black">השמלה עשויה לרדת מהאתר</strong>
                  ,
                  <br />
                  זאת כדי לשמור על אתר אמין ואקטואלי.
                </p>
              </div>
            </div>
          </section>

          <div className="h-px bg-gradient-to-l from-transparent via-black to-transparent my-8"></div>

          <section className="mb-10">
            <h2 className="text-2xl font-bold text-black mb-6 pb-3 border-b-2 border-black inline-block">
              במקרה של השכרה או קנייה
            </h2>

            <div className="p-5 bg-neutral-50 rounded-xl">
              <p className="text-base leading-relaxed text-neutral-600">
                אם בוצעה עסקה דרך האתר: <br />
                יישלח אלייך מייל הכולל הסבר מסודר לגבי תשלום העמלה (15%).
                <br />
                <br />
                שימי לב: אתר JustRentIt <strong>לא אחראי לכל נזק</strong> שייגרם
                לשמלה במהלך השכרה או שימוש.
              </p>
            </div>
          </section>

          <div className="h-px bg-gradient-to-l from-transparent via-black to-transparent my-8"></div>

          <section className="mb-10">
            <h2 className="text-2xl font-bold text-black mb-6 pb-3 border-b-2 border-black inline-block">
              למתעניינות בשמלות
            </h2>

            <div className="p-5 bg-neutral-50 rounded-xl">
              <p className="text-base leading-relaxed text-neutral-600">
                כאשר את מתעניינת בשמלה:
                <br /> פרטי ההתקשרות שלך (שם, טלפון, אימייל) יועברו לבעלת השמלה.{" "}
                <br />
                ניתן לעדכן את הפרטים בכל עת דרך אזור {""}
                <Link href="/profile" className="text-blue-600 underline">
                  המשתמש
                </Link>
                <br />
                את תקבלי מייל עם פרטי בעלת השמלה כדי שתוכלי ליצור קשר בצורה
                ישירה, נוחה ומהירה.
              </p>
            </div>
          </section>

          <div className="h-px bg-gradient-to-l from-transparent via-black to-transparent my-8"></div>

          <section className="mb-10">
            <h2 className="text-2xl font-bold text-black mb-6 pb-3 border-b-2 border-black inline-block">
              למה לבחור ב-JustRentIt?
            </h2>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mt-6">
              <div className="border-2 border-black rounded-xl p-5 text-center hover:bg-neutral-50 transition-colors duration-300">
                <p className="text-sm font-medium text-neutral-900">
                  העלאת שמלה ללא עלות
                </p>
              </div>
              <div className="border-2 border-black rounded-xl p-5 text-center hover:bg-neutral-50 transition-colors duration-300">
                <p className="text-sm font-medium text-neutral-900">
                  חשיפה לקהל רחב
                </p>
              </div>
              <div className="border-2 border-black rounded-xl p-5 text-center hover:bg-neutral-50 transition-colors duration-300">
                <p className="text-sm font-medium text-neutral-900">
                  תהליך פשוט וברור
                </p>
              </div>
              <div className="border-2 border-black rounded-xl p-5 text-center hover:bg-neutral-50 transition-colors duration-300">
                <p className="text-sm font-medium text-neutral-900">
                  חוויית שימוש עדכנית
                </p>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
