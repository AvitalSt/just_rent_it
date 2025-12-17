import { DressDTO } from "@/models/DTOs/DressDTO";
import { DressStateMap, SaleTypeMap } from "@/models/Enums/filtersMap";

export default function Details({ dress }: { dress: DressDTO }) {
  const sizeLabel = dress.sizes.length > 1 ? "מידות" : "מידה";
  const colorLabel = dress.colors.length > 1 ? "צבעים" : "צבע";
  const cityLabel = dress.cities.length > 1 ? "נמצאת בערים" : "נמצאת בעיר";

  return (
    <div className="bg-gray-50 rounded-lg p-4 mb-6 space-y-3 text-sm">

      <div>
        <span className="font-semibold text-gray-700"> {sizeLabel}: </span>
        <span className="text-gray-600">{dress.sizes.join(", ")}</span>
      </div>

      <div>
        <span className="font-semibold text-gray-700">{colorLabel}: </span>
        <span className="text-gray-600">{dress.colors.join(", ")}</span>
      </div>

      <div>
        <span className="font-semibold text-gray-700">השמלה {cityLabel}: </span>
        <span className="text-gray-600">{dress.cities.join(", ")}</span>
      </div>

      <div>
        <span className="font-semibold text-gray-700">השמלה: </span>
        <span className="text-gray-600">{SaleTypeMap[dress.saleType]}</span>
      </div>

      <div>
        <span className="font-semibold text-gray-700">מצב השמלה: </span>
        <span className="text-gray-600">{DressStateMap[dress.state]}</span>
      </div>

    </div>
  );
}
