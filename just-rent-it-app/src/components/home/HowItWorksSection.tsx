export default function HowItWorksSection() {
  const steps = [
    { num: 1, title: "בחרי שמלה", desc: "עייני בקולקציה ובחרי את השמלה שהכי מתאימה לך." },
    { num: 2, title: "שלחי התעניינות", desc: "שלחי בקשת התעניינות, ומיד תקבלי למייל את פרטי בעלת השמלה." },
    { num: 3, title: "צרי קשר", desc: "תאמי מול בעלת השמלה את כל הפרטים, ותעדכני אותנו אם סגרתן השכרה." },
  ];

  return (
    <section className="py-24 px-6 bg-gray-50">
      <div className="max-w-6xl mx-auto text-center mb-20">
        <h2 className="text-4xl md:text-5xl font-extralight">איך זה עובד?</h2>
      </div>

      <div className="grid md:grid-cols-3 gap-15">
        {steps.map((step) => (
          <div key={step.num} className="text-center">
            <div className="w-12 h-12 mx-auto mb-6 bg-black text-white flex items-center justify-center text-2xl font-light">
              {step.num}
            </div>
            <h3 className="text-xl font-light mb-3">{step.title}</h3>
            <p className="text-sm text-gray-600 leading-relaxed">{step.desc}</p>
          </div>
        ))}
      </div>
    </section>
  );
}
