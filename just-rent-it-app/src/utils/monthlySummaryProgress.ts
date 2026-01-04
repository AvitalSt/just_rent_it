export type MonthlyProgress = { daysToSplit: number; lastSentDay: number };

export function getMonthKey(fromUtc: string) {
  const d = new Date(fromUtc);
  const ym = `${d.getFullYear()}_${String(d.getMonth() + 1).padStart(2, "0")}`;
  return `monthlySummary_progress_${ym}`;
}

export function loadProgress(key: string): MonthlyProgress | null {
  try {
    const raw = localStorage.getItem(key);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function saveProgress(key: string, data: MonthlyProgress) {
  localStorage.setItem(key, JSON.stringify(data));
}
