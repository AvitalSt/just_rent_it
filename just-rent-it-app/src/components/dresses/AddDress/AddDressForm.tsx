"use client";

import React, { useEffect, useState } from "react";
import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import { addDress } from "@/services/dressService";
import { uploadImages } from "@/services/imagesService";

import DressDetailsFields from "./DressDetailsFields";
import DressSelectFields from "./DressSelectFields";
import DressImageFields from "./DressImageFields";

import { Button } from "@/components/ui/Button";
import { ErrorMessage } from "@/components/ui/ErrorMessage";

import { useAppSelector } from "@/store/hooks";
import axios from "axios";
import { DressImageDTO } from "@/models/DTOs/ImageDTO";
import { validateAddDress } from "@/utils/addDressValidation";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { SingleSelect } from "@/components/ui/filters/SingleSelect";
import { DressStateMap, SaleTypeMap } from "@/models/Enums/filtersMap";
import ConfirmModal from "@/components/ui/ConfirmModal";
import { useRouter } from "next/navigation";
import PolicySection from "./PolicySection";
import { clearDressesCache } from "@/services/dressCache";

export default function AddDressForm() {
  const { colors, sizes, cities, ageGroups, eventTypes } = useAppSelector(
    (state) => state.options
  );
  const router = useRouter();

  const [form, setForm] = useState<AddDressDTO>({
    Name: "",
    Description: "",
    Price: null,
    ColorIds: [],
    SizeIds: [],
    CityIds: [],
    AgeGroupIds: [],
    EventTypeIds: [],
    MainImage: "",
    SaleType: null,
    State: null,
    ImagePaths: [],
  });

  const [files, setFiles] = useState<File[]>([]);
  const [previewImages, setPreviewImages] = useState<string[]>([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [acceptedPolicy, setAcceptedPolicy] = useState(false);
  const [showAuthModal, setShowAuthModal] = useState(false);
  const [showSuccessModal, setShowSuccessModal] = useState(false);

  const user = useAppSelector((state) => state.user.user);
  const isLoggedIn = !!user;

  useEffect(() => {
    if (!user) {
      setShowAuthModal(true);
    }
  }, [user]);

  const handleChange = <K extends keyof AddDressDTO>(
    key: K,
    value: AddDressDTO[K]
  ) => {
    setForm((prev) => ({ ...prev, [key]: value }));
  };

  const handleSubmit = async () => {
    if (loading) return;
    if (!isLoggedIn) {
      setShowAuthModal(true);
      return;
    }
    if (!acceptedPolicy) {
      setLoading(false);
      return setError("עלייך לאשר את התקנון לפני הוספת שמלה.");
    }
    setLoading(true);

    try {
      setError("");

      const msg = validateAddDress(form, files);
      if (msg) {
        setError(msg);
        return;
      }

      const uploaded = await uploadImages(files);
      const paths = uploaded.map((u: DressImageDTO) => u.imagePath);

      const mainIdx = previewImages.findIndex((x) => x === form.MainImage);
      const main = mainIdx >= 0 ? paths[mainIdx] : paths[0];

      handleChange("ImagePaths", paths);
      handleChange("MainImage", main);

      await addDress({
        ...form,
        ImagePaths: paths,
        MainImage: main,
      });

      clearDressesCache();

      setShowSuccessModal(true);

      setForm({
        Name: "",
        Description: "",
        Price: null,
        ColorIds: [],
        SizeIds: [],
        CityIds: [],
        AgeGroupIds: [],
        EventTypeIds: [],
        MainImage: "",
        SaleType: null,
        State: null,
        ImagePaths: [],
      });

      setFiles([]);
      setPreviewImages([]);
      setAcceptedPolicy(false);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(
          err.response?.data?.message ||
            "אירעה שגיאה בשליחת הטופס. אנא נסי שוב."
        );
      } else {
        setError("אירעה שגיאה בלתי צפויה. אנא נסי שוב.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 py-4 px-2" dir="rtl">
      {showAuthModal && (
        <ConfirmModal
          message="עלייך להתחבר או להירשם לפני העלאת שמלה"
          onConfirm={() => {
            setShowAuthModal(false);
            router.push("/login");
          }}
          onCancel={() => setShowAuthModal(false)}
        />
      )}
      <div className="max-w-md mx-auto bg-white rounded-lg shadow-md p-4">
        <h1 className="text-3xl font-bold text-center mb-2 text-gray-900">
          הוספת שמלה חדשה
        </h1>

        <p className="text-center text-gray-500 mb-6 text-base">
          מלאי את כל הפרטים בקפידה
        </p>

        {error && <ErrorMessage message={error} />}
        {showSuccessModal && (
          <ConfirmModal
            message={`השמלה נוספה בהצלחה!
                  תקבלי עדכון למייל כשיאשרו אותה.`}
            onConfirm={() => {
              setShowSuccessModal(false);
              router.push("/");
            }}
          />
        )}
        <div className="bg-gray-50 rounded-lg p-2 space-y-4">
          <DressDetailsFields formData={form} handleChange={handleChange} />

          <DressSelectFields
            formData={form}
            handleChange={handleChange}
            colors={colors}
            sizes={sizes}
            cities={cities}
            ageGroups={ageGroups}
            eventTypes={eventTypes}
          />

          <UnifiedSelector
            label="מצב שמלה"
            required
            hasValue={form.State !== null}
            onClear={() => handleChange("State", null)}
            count={form.State ? 1 : 0}
          >
            <p className="text-xs text-gray-400 mb-2">נא לבחור אפשרות אחת</p>

            <SingleSelect
              options={Object.entries(DressStateMap).map(([value, label]) => ({
                value: Number(value),
                label,
              }))}
              selected={form.State}
              onChange={(v) => handleChange("State", v)}
            />
          </UnifiedSelector>

          <UnifiedSelector
            label="סוג מכירה"
            required
            hasValue={form.SaleType !== null}
            onClear={() => handleChange("SaleType", null)}
            count={form.SaleType ? 1 : 0}
          >
            <p className="text-xs text-gray-400 mb-2">נא לבחור אפשרות אחת</p>

            <SingleSelect
              options={Object.entries(SaleTypeMap).map(([value, label]) => ({
                value: Number(value),
                label,
              }))}
              selected={form.SaleType}
              onChange={(v) => handleChange("SaleType", v)}
            />
          </UnifiedSelector>

          <DressImageFields
            formData={form}
            handleChange={handleChange}
            files={files}
            setFiles={setFiles}
            previewImages={previewImages}
            setPreviewImages={setPreviewImages}
          />

          <PolicySection
            accepted={acceptedPolicy}
            onChange={setAcceptedPolicy}
          />

          <Button
            onClick={handleSubmit}
            className="w-full text-base py-3 rounded-lg shadow-md hover:shadow-lg"
            disabled={loading}
          >
            {loading ? "השמלה בהעלאה..." : "הוסף שמלה לאתר"}
          </Button>
        </div>
      </div>
    </div>
  );
}
