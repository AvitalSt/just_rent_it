"use client";

import { ReactNode } from "react";
import { Provider } from "react-redux";
import { store } from "@/store";

interface ProviderWrapperProps {
  children: ReactNode;
}

export const ProviderWrapper: React.FC<ProviderWrapperProps> = ({ children }) => {
  return <Provider store={store}>{children}</Provider>;
};
