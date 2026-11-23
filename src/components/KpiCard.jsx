export default function KpiCard({ title, value, subtitle, icon: Icon, loading = false }) {
  if (loading) {
    return (
      <div className="rounded-lg border border-slate-300 bg-white p-4 animate-pulse">
        <div className="flex items-start justify-between">
          <div className="h-4 w-28 rounded bg-slate-200" />
          <div className="h-7 w-7 rounded border border-slate-200 bg-slate-100" />
        </div>
        <div className="mt-3 h-7 w-24 rounded bg-slate-200" />
        <div className="mt-2 h-3 w-20 rounded bg-slate-100" />
      </div>
    );
  }

  return (
    <div className="flex flex-col justify-between rounded-lg border border-slate-300 bg-white p-4">
      <div className="flex items-start justify-between gap-2">
        <span className="text-sm font-medium text-slate-700">{title}</span>
        {Icon && (
          <span className="inline-flex h-7 w-7 shrink-0 items-center justify-center rounded border border-slate-300 bg-slate-50 text-slate-600">
            <Icon className="h-4 w-4" aria-hidden="true" />
          </span>
        )}
      </div>

      <div className="mt-3 text-2xl font-bold tracking-tight text-slate-900">
        {value}
      </div>

      {subtitle && (
        <p className="mt-1.5 text-xs text-slate-400">{subtitle}</p>
      )}
    </div>
  );
}