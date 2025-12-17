import InterestRow from "./InterestRow";
import { InterestTableProps } from "@/models/types/interest/InterestTable.type";

export default function InterestTable({
  list,
  pageSize,
  sendingOwner,
  sendingUser,
  sendingPayment,
  updateField,
  sendOwnerMail,
  sendUserMail,
  sendOwnerPaymentMail,
}: InterestTableProps) {
  return (
    <table className="w-full bg-white border rounded-xl text-right">
      <thead className="bg-gray-100">
        <tr>
          <th className="p-3">תאריך</th>
          <th className="p-3">מתעניינת</th>
          <th className="p-3">בעלת שמלה</th>
          <th className="p-3">שמלה</th>
          <th className="p-3">סטטוס</th>
          <th className="p-3">מחיר</th>
          <th className="p-3">הערות בכללי</th>
          <th className="p-3">עדכון מבעלת השמלה</th>
          <th className="p-3">עדכון מהמתעניינת</th>
          <th className="p-3">שליחת הודעה</th>
        </tr>
      </thead>

      <tbody>
        {list.map((row, index) => {
          const page = Math.floor(index / pageSize) + 1;
          return (
            <InterestRow
              key={row.interestID}
              page={page}
              row={row}
              sendingOwner={sendingOwner}
              sendingUser={sendingUser}
              sendingPayment={sendingPayment}
              updateField={updateField}
              sendOwnerMail={sendOwnerMail}
              sendUserMail={sendUserMail}
              sendOwnerPaymentMail={sendOwnerPaymentMail}
            />
          );
        })}
      </tbody>
    </table>
  );
}
