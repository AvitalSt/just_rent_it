"use client";

import { useEffect, useState } from "react";
import { DressDTO } from "@/models/DTOs/DressDTO";
import { DressImageDTO } from "@/models/DTOs/ImageDTO";
import { uploadImages } from "@/services/imagesService";
import { updateDress } from "@/services/dressService";
import {
  AgeGroupDTO,
  CityDTO,
  ColorDTO,
  EventTypeDTO,
  SizeDTO,
} from "@/models/DTOs/OptionsDTO";
import { UpdateDressDTO } from "@/models/DTOs/UpdateDressDTO";
import { clearDressesCache } from "@/services/dressCache";

export function useDressEdit(
  dress: DressDTO,
  onSaved: () => void,
  options: {
    colors: ColorDTO[];
    sizes: SizeDTO[];
    cities: CityDTO[];
    ageGroups: AgeGroupDTO[];
    eventTypes: EventTypeDTO[];
  }
) {
  const { colors, sizes, cities, ageGroups, eventTypes } = options;

  const [form, setForm] = useState({
    name: "",
    price: null as number | null,
    description: "",
    ColorIDs: [] as number[],
    SizeIDs: [] as number[],
    CityIDs: [] as number[],
    AgeGroupIDs: [] as number[],
    EventTypeIDs: [] as number[],
    saleType: dress.saleType,
    state: dress.state,
    status: dress.status,
  });

  const [removeImages, setRemoveImages] = useState<number[]>([]);
  const [selectedMainImage, setSelectedMainImage] = useState<string>(
    dress.mainImage
  );
  const [newFiles, setNewFiles] = useState<File[]>([]);
  const [newPreview, setNewPreview] = useState<string[]>([]);

  const [loading, setLoading] = useState(false);

  useEffect(() => {
    //מקבלת רשימת אובייקטים ורשימת מחרוזות ואז עושה filter מסננת רק את האויקיבטים שרושמים בשמלה ו-map מחזירה רק את הID שלהם
    const mapByNames = (
      list: { id: number; nameHebrew: string }[],
      selected: string[]
    ) => list.filter((i) => selected.includes(i.nameHebrew)).map((i) => i.id);

    setForm({
      name: dress.name,
      price: dress.price,
      description: dress.description,
      //נכנס רשימת ID של השמלה הזו
      ColorIDs: mapByNames(colors, dress.colors),
      SizeIDs: sizes
        .filter((s) => dress.sizes.includes(s.name))
        .map((s) => s.id),
      CityIDs: mapByNames(cities, dress.cities),
      AgeGroupIDs: mapByNames(ageGroups, dress.ageGroups),
      EventTypeIDs: mapByNames(eventTypes, dress.eventTypes),
      saleType: dress.saleType,
      state: dress.state,
      status: dress.status,
    });
  }, [dress, colors, sizes, cities, ageGroups, eventTypes]);

  const handleRemoveImage = (img: DressImageDTO) => {
    if (!img || typeof img.imageID !== "number") return;
    setRemoveImages((prev) => [...prev, img.imageID]);
  };

  //מאפשר לבחור תמונה ראשית - בין אם תמונה קיימת/תמונה חדשה
  const handleSetMain = (img: DressImageDTO | string) => {
    if (typeof img === "string") setSelectedMainImage(img);
    else setSelectedMainImage(img.imagePath);
  };

  const onAddImages = (files: FileList | null) => {
    if (!files) return;
    const arr = Array.from(files);
    setNewFiles((prev) => [...prev, ...arr]);
    //כל קובץ הופף לקובץ זמני שמוצג במסך
    const previews = arr.map((f) => URL.createObjectURL(f));
    setNewPreview((prev) => [...prev, ...previews]);
  };

  const handleSave = async () => {
    if (loading) return;

    setLoading(true);
    try {
      let uploadedPaths: string[] = [];

      if (newFiles.length > 0) {
        //השרת שלנו מחזיר ניתיבם לא קבצים!
        const uploaded = await uploadImages(newFiles, dress.dressID);
        uploadedPaths = uploaded.map((u) => u.imagePath);
      }

      //תמונות קיימות שלא נמחקו
      const existingImages = dress.images
        .filter((img) => !removeImages.includes(img.imageID))
        .map((img) => img.imagePath);

      //שילוב של התמונות הקיימות כרגע עם מה שהעולה
      const allImagesAfterEdit = [...existingImages, ...uploadedPaths];

      if (allImagesAfterEdit.length === 0) {
        alert("שמלה חייבת לכלול לפחות תמונה אחת.");
        setLoading(false);
        return;
      }

      //להסיר תווים לא טובים קורה כשהקובץ נשמר בעברית
      let finalMain = selectedMainImage?.replace(/\u200f/g, "");

      //some האם יש לפחות איבר אחד שעומד על התנאים
      const mainRemoved = dress.images.some(
        (i) => i.imagePath === finalMain && removeImages.includes(i.imageID)
      );

      if (mainRemoved) {
        finalMain = uploadedPaths[0] ?? existingImages[0] ?? "";
      }

      if (newPreview.includes(selectedMainImage)) {
        const idx = newPreview.indexOf(selectedMainImage);

        finalMain =
          uploadedPaths[idx] ??
          uploadedPaths[0] ??
          existingImages[0] ??
          finalMain;
      }

      if (!finalMain) finalMain = allImagesAfterEdit[0];

      const dto: UpdateDressDTO = {
        Name: form.name,
        Description: form.description,
        ColorIDs: form.ColorIDs,
        SizeIDs: form.SizeIDs,
        CityIDs: form.CityIDs,
        AgeGroupIDs: form.AgeGroupIDs,
        EventTypeIDs: form.EventTypeIDs,
        SaleType: form.saleType,
        State: form.state,
        Status: form.status,
        MainImage: finalMain,
        AddImagePaths: uploadedPaths,
        RemoveImageIds: removeImages,
      };

      if (form.price && form.price > 0) dto.Price = form.price;

      await updateDress(dress.dressID, dto);
      clearDressesCache();
      onSaved();
    } catch (error) {
      alert("אירעה שגיאה בשמירת הנתונים.");
    } finally {
      setLoading(false);
    }
  };

  return {
    form,
    setForm,
    removeImages,
    handleRemoveImage,
    selectedMainImage,
    handleSetMain,
    newPreview,
    newFiles,
    onAddImages,
    setNewPreview,
    setNewFiles,
    handleSave,
    loading
  };
}
