"use client";

import { useAppSelector } from "@/store/hooks";
import DressEditImages from "./DressEditImages";

import ColorFilter from "../DressFilter/Filter/ColorFilter";
import SizeFilter from "../DressFilter/Filter/SizeFilter";
import CityFilter from "../DressFilter/Filter/CityFilter";
import AgeGroupFilter from "../DressFilter/Filter/AgeGroupFilter";
import EventTypeFilter from "../DressFilter/Filter/EventTypeFilter";

import {
  SaleTypeMap,
  DressStateMap,
} from "@/models/Enums/filtersMap";

import { useDressEdit } from "@/hooks/useDressEdit";
import { Button } from "@/components/ui/Button";
import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { SingleSelect } from "@/components/ui/filters/SingleSelect";

import { DressEditFormProps } from "@/models/types/dress/DressEditForm.types";

export default function DressEditForm({
  dress,
  onCancel,
  onSaved,
}: DressEditFormProps) {
  const options = useAppSelector((s) => s.options);

  const {
    form,
    setForm,
    removeImages,
    handleRemoveImage,
    selectedMainImage,
    handleSetMain,
    newPreview,
    onAddImages,
    previewImage,
    setPreviewImage,
    setNewPreview,
    setNewFiles,
    handleSave,
  } = useDressEdit(dress, onSaved, options);
  const EMPTY_VALUE = -1;

  return (
    <div
      className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-lg"
      dir="rtl"
    >
      <div className="mb-8 pb-4 border-b-2 border-gray-200">
        <h2 className="text-3xl font-bold text-gray-800">עריכת שמלה</h2>
      </div>

      <div className="space-y-6">
        <div className="bg-gray-50 p-6 rounded-lg space-y-4">
          <div>
            <label className="block font-semibold text-gray-700 mb-2">שם</label>
            <input
              className="border border-gray-300 p-3 rounded-lg w-full"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              placeholder="הזן שם שמלה"
            />
          </div>

          <div>
            <label className="block font-semibold text-gray-700 mb-2">
              מחיר (₪)
            </label>
            <input
              type="number"
              className="border border-gray-300 p-3 rounded-lg w-full"
              value={form.price ?? ""}
              onChange={(e) =>
                setForm({
                  ...form,
                  price:
                    Number(e.target.value) > 0 ? Number(e.target.value) : null,
                })
              }
              placeholder="הזן מחיר"
            />
          </div>

          <div>
            <label className="block font-semibold text-gray-700 mb-2">
              תיאור
            </label>
            <textarea
              rows={4}
              className="border border-gray-300 p-3 rounded-lg w-full resize-none"
              value={form.description}
              onChange={(e) =>
                setForm({ ...form, description: e.target.value })
              }
              placeholder="תאר את השמלה..."
            />
          </div>
        </div>

        <div className="bg-gray-50 p-6 rounded-lg">
          <DressEditImages
            dress={dress}
            removeImages={removeImages}
            handleRemoveImage={handleRemoveImage}
            selectedMainImage={selectedMainImage}
            handleSetMain={handleSetMain}
            newPreview={newPreview}
            onAddImages={onAddImages}
            setPreviewImage={setPreviewImage}
            setNewPreview={setNewPreview}
            setNewFiles={setNewFiles}
          />
        </div>

        {previewImage && (
          <div
            className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50"
            onClick={() => setPreviewImage(null)}
          >
            <img
              src={previewImage}
              className="max-h-[90vh] max-w-[90vw] rounded shadow-lg"
              onClick={(e) => e.stopPropagation()}
            />
            <button
              className="absolute top-6 right-6 text-white bg-black bg-opacity-40 p-3 rounded-full"
              onClick={() => setPreviewImage(null)}
            >
              X
            </button>
          </div>
        )}

        <div className="bg-gray-50 p-6 rounded-lg space-y-4">
          <ColorFilter
            value={form.ColorIDs}
            onChange={(v) => setForm({ ...form, ColorIDs: v })}
          />

          <SizeFilter
            value={form.SizeIDs}
            onChange={(v) => setForm({ ...form, SizeIDs: v })}
          />

          <CityFilter
            value={form.CityIDs}
            onChange={(v) => setForm({ ...form, CityIDs: v })}
          />

          <AgeGroupFilter
            value={form.AgeGroupIDs}
            onChange={(v) => setForm({ ...form, AgeGroupIDs: v })}
          />

          <EventTypeFilter
            value={form.EventTypeIDs}
            onChange={(v) => setForm({ ...form, EventTypeIDs: v })}
          />

          <UnifiedSelector
            label="סוג מודעה"
            hasValue={form.saleType !== null}
            onClear={() => setForm({ ...form, saleType: EMPTY_VALUE })}
            count={form.saleType !== EMPTY_VALUE ? 1 : 0}
            required
          >
            <SingleSelect
              options={Object.entries(SaleTypeMap).map(([value, label]) => ({
                value: Number(value),
                label,
              }))}
              selected={form.saleType}
              onChange={(v) => setForm({ ...form, saleType: v })}
            />
          </UnifiedSelector>

          <UnifiedSelector
            label="מצב שמלה"
            hasValue={form.state !== null}
            onClear={() => setForm({ ...form, state: EMPTY_VALUE })}
            count={form.saleType !== EMPTY_VALUE ? 1 : 0}
            required
          >
            <SingleSelect
              options={Object.entries(DressStateMap).map(([value, label]) => ({
                value: Number(value),
                label,
              }))}
              selected={form.state}
              onChange={(v) => setForm({ ...form, state: v })}
            />
          </UnifiedSelector>
        </div>

        <div className="flex gap-4 pt-4">
          <Button onClick={handleSave}>שמירה</Button>
          <Button onClick={onCancel}>ביטול</Button>
        </div>
      </div>
    </div>
  );
}
