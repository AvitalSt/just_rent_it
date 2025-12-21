export const dressesCache: Record<string, any> = {};
export const totalResultsCache: Record<string, number> = {};
export const maxPriceCache: Record<string, number> = {};

export const clearDressesCache = () => {
  Object.keys(dressesCache).forEach(k => delete dressesCache[k]);
  Object.keys(totalResultsCache).forEach(k => delete totalResultsCache[k]);
  Object.keys(maxPriceCache).forEach(k => delete maxPriceCache[k]);
};
