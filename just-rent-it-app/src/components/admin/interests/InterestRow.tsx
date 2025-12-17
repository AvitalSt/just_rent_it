import { Button } from "@/components/ui/Button";
import { InterestRowProps } from "@/models/types/interest/InterestRow.type";

export default function InterestRow({
  page,
  row, 
  sendingOwner,
  sendingUser,
  sendingPayment,
  updateField,
  sendOwnerMail,
  sendUserMail,
  sendOwnerPaymentMail,
}: InterestRowProps) {
  const id = row.interestID;

  return (
    <tr className="border-t" data-page={page}>
      <td className="p-3">{row.sentDate.slice(0, 10)}</td>

      <td className="p-3">
        {row.userName}
        <div className="text-xs text-gray-500">{row.userEmail}</div>
      </td>

      <td className="p-3">
        {row.ownerName}
        <div className="text-xs text-gray-500">{row.ownerEmail}</div>
      </td>

      <td className="p-3">{row.dressName}</td>

      <td className="p-3">
        <select
          className="border p-1 rounded"
          value={row.status}
          onChange={async (e) => {
            const value = e.target.value;
            await updateField(id, "status", value);

            if (value === "Paid" && row.paymentAmount === 0) {
              await updateField(id, "paymentAmount", 0);
            }
          }}
        >
          <option value="Pending">בהמתנה</option>
          <option value="Confirmed">אושרה השכרה</option>
          <option value="Cancelled">בוטל</option>
          <option value="Paid">שולם</option>
        </select>
      </td>

      <td className="p-3">
        {row.status === "Paid" ? (
          <input
            type="number"
            className="border p-1 rounded w-24"
            defaultValue={row.paymentAmount ?? 0}
            onBlur={(e) =>
              updateField(id, "paymentAmount", Number(e.target.value))
            }
          />
        ) : (
          <span className="text-gray-400 text-sm">—</span>
        )}
      </td>

      <td className="p-3">
        <textarea
          className="border p-1 rounded w-40"
          defaultValue={row.notes}
          onBlur={(e) => updateField(id, "notes", e.target.value)}
        />
      </td>

      <td className="p-3">
        <textarea
          className="border p-1 rounded w-40"
          defaultValue={row.ownerComment}
          onBlur={(e) => updateField(id, "ownerComment", e.target.value)}
        />
      </td>

      <td className="p-3">
        <textarea
          className="border p-1 rounded w-40"
          defaultValue={row.userComment}
          onBlur={(e) => updateField(id, "userComment", e.target.value)}
        />
      </td>

      <td className="flex flex-col gap-2 p-3">
        <Button
          disabled={sendingOwner[id]}
          onClick={() => sendOwnerMail(row)}
          className={sendingOwner[id] ? "opacity-50 cursor-not-allowed" : ""}
        >
          לבעלת השמלה <br /> מה קורה עם השמלה
          {row.ownerMailCount > 0 && ` (${row.ownerMailCount})`}
        </Button>

        <Button
          disabled={sendingUser[id]}
          onClick={() => sendUserMail(row)}
          className={sendingUser[id] ? "opacity-50 cursor-not-allowed" : ""}
        >
          למתעניינת <br /> מה קורה עם השמלה
          {row.userMailCount > 0 && ` (${row.userMailCount})`}
        </Button>

        <Button
          disabled={sendingPayment[id]}
          onClick={() => sendOwnerPaymentMail(row)}
          className={sendingPayment[id] ? "opacity-50 cursor-not-allowed" : ""}
        >
          פרטי תשלום <br /> לבעלת השמלה
        </Button>
      </td>
    </tr>
  );
}
