import {
  ColorDTO,
  SizeDTO,
  CityDTO,
  AgeGroupDTO,
  EventTypeDTO,
} from "@/models/DTOs/OptionsDTO";
import optionsAPI from "@/services/optionsService";
import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";

interface OptionsState {
  colors: ColorDTO[];
  sizes: SizeDTO[];
  cities: CityDTO[];
  ageGroups: AgeGroupDTO[];
  eventTypes: EventTypeDTO[];
  loading: boolean;
  error: string | null;
}

const initialState: OptionsState = {
  colors: [],
  sizes: [],
  cities: [],
  ageGroups: [],
  eventTypes: [],
  loading: false,
  error: null,
};

export const fetchOptions = createAsyncThunk(
  "options/fetchAll",
  async (_, thunkAPI) => {
    try {
      const [c, s, ci, ag, et] = await Promise.all([
        optionsAPI.getColors(),
        optionsAPI.getSizes(),
        optionsAPI.getCities(),
        optionsAPI.getAgeGroups(),
        optionsAPI.getEventTypes(),
      ]);

      return { c, s, ci, ag, et };
    } catch (err: any) {
      return thunkAPI.rejectWithValue("שגיאה בטעינת נתונים");
    }
  }
);

export const optionsSlice = createSlice({
  name: "options",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchOptions.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchOptions.fulfilled, (state, action) => {
        state.loading = false;

        state.colors = action.payload.c;
        state.sizes = action.payload.s;
        state.cities = action.payload.ci;
        state.ageGroups = action.payload.ag;
        state.eventTypes = action.payload.et;
      })
      .addCase(fetchOptions.rejected, (state, action) => {
        state.loading = false;
        state.error = (action.payload as string) || "שגיאה";
      });
  },
});

export default optionsSlice.reducer;
