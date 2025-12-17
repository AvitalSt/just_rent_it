import { AddDressDTO } from "@/models/DTOs/AddDressDTO";

export const validateAddDress = (
  form: AddDressDTO,
  files: File[]
): string | null => {

  const missing =
    !form.Name?.trim() ||
    form.Price === null ||
    form.Price <= 0 ||
    form.SaleType === null ||
    form.State === null ||
    !form.ColorIds.length ||
    !form.SizeIds.length ||
    !form.CityIds.length ||
    !form.AgeGroupIds.length ||
    !form.EventTypeIds.length ||
    (!files.length && !form.ImagePaths.length);

  if (missing) return "נא למלא את כל הפרטים הדרושים";

  return null;
};
