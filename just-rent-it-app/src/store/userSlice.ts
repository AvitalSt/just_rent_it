import { UserRole } from "@/models/Enums/UserRole";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

//מגדירים אובייקט של "יוזר
interface User {
  userID: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: UserRole;
  createdDate: string;
  updateAt: string;
  wishlistDressIds: number[];
}

//מגידירם את הסטייט- או שיש שדות או שזה "נאל"
interface UserState {
  user: User | null | undefined;
}

//כשאין משתמש הערך הדיפוליטיבי זה "נאל"
const initialState: UserState = {
  user: undefined,
};

export const userSlice = createSlice({
  name: "user", //שם ה"סלייס משמש לזיהוי ב"סטור
  initialState,
  reducers: {
    //הפוקנציה שמעדכנת את ה"סטיט
    setUser: (state, action: PayloadAction<{ user: User }>) => {
      state.user = action.payload.user;
    },
    clearUser: (state) => {
      state.user = null;
    },
    setWishlistDressIds: (state, action: PayloadAction<number[]>) => {
      if (state.user) {
        state.user.wishlistDressIds = action.payload;
      }
    },
    toggleWishlist: (state, action: PayloadAction<number>) => {
      const id = action.payload;
      if (!state.user) return;

      const list = state.user.wishlistDressIds;

      if (list.includes(id)) {
        state.user.wishlistDressIds = list.filter((x) => x !== id);
      } else {
        state.user.wishlistDressIds.push(id);
      }
    },
  },
});

export const { setUser, clearUser, setWishlistDressIds, toggleWishlist } =
  userSlice.actions;
export default userSlice.reducer;
