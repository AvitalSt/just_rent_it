"use client";

import { CheckboxField } from "@/components/ui/CheckboxField";
import { PolicySectionProps } from "@/models/types/dress/PolicySection.types";

export default function PolicySection({ accepted, onChange }: PolicySectionProps) {
  return (
    <div className="bg-white border border-gray-200 rounded-lg p-5 space-y-4">
      <div className="flex items-center gap-2 pb-3 border-b border-gray-200">
        <h3 className="font-semibold text-gray-900">תקנון האתר</h3>
      </div>

      <div className="bg-gray-50 rounded-lg p-4 space-y-3 text-sm text-gray-700 leading-relaxed">
        <ul className="list-disc pr-4 space-y-2">
          <li>
            אני מאשרת שבמידה ותתבצע השכרה/מכירה דרך האתר, אעביר{" "}
            <strong>15% ממחיר העסקה</strong> לאתר JustRentIt.
          </li>

          <li>
            אני יודעת שבכל פעם שמישהי מתעניינת בשמלה שלי{" "}
            <strong>אקבל עדכון למייל</strong>.
          </li>

          <li>
            אני יודעת שבסוף כל חודש אקבל מהאתר{" "}
            <strong>
              סיכום חודשי על כל ההתעניינויות שהיו בשמלות שלי (במידה והיו)
            </strong>
            , ואעדכן האם בוצעה השכרה/מכירה. <br />
            <strong>ללא עדכון - השמלה עשויה לרדת מהאתר.</strong>
          </li>
        </ul>
      </div>

      <div className="pt-2">
        <a
          href="/site-policy"
          target="_blank"
          rel="noopener noreferrer"
          className="underline text-xs"
        >
          תקנון האתר
        </a>

        <CheckboxField
          id="termsCheck"
          label="אני מאשרת את תקנון האתר ומתחייבת לעמוד בתנאיו"
          required
          checked={accepted}
          onChange={onChange}
        />
      </div>
    </div>
  );
}
