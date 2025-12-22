export const interestsCache: Record<string, any> = {};
export const totalCountCache: Record<string, number> = {};

export const clearInterestsCache = () => {
  Object.keys(interestsCache).forEach(k => delete interestsCache[k]);
  Object.keys(totalCountCache).forEach(k => delete totalCountCache[k]);
};
