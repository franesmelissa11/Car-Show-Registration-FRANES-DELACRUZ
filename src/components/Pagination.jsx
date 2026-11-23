import { ChevronLeft, ChevronRight } from 'lucide-react';

export default function Pagination({
  page,
  pageSize,
  totalCount,
  totalPages,
  loading,
  onPageChange,
  onPageSizeChange,
}) {
  const start = totalCount === 0 ? 0 : (page - 1) * pageSize + 1;
  const end = Math.min(page * pageSize, totalCount);
  const pages = Array.from({ length: totalPages }, (_, idx) => idx + 1);

  return (
    <div className="flex flex-col gap-3 px-4 py-3.5 sm:flex-row sm:items-center sm:justify-between">
      {/* Left: < 1 2 3 > */}
      <div className="flex items-center gap-1.5">
        <button
          type="button"
          onClick={() => onPageChange(page - 1)}
          disabled={loading || page <= 1}
          aria-label="Previous page"
          className="inline-flex h-8 w-8 items-center justify-center rounded border border-slate-300 bg-white text-slate-700 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
        >
          <ChevronLeft className="h-4 w-4" />
        </button>

        {pages.map((p) => {
          const active = p === page;
          return (
            <button
              key={p}
              type="button"
              onClick={() => onPageChange(p)}
              disabled={loading}
              aria-current={active ? 'page' : undefined}
              className={`h-8 min-w-8 rounded border px-2 text-xs font-semibold transition disabled:cursor-not-allowed disabled:opacity-40 ${
                active
                  ? 'border-slate-400 bg-slate-200 text-slate-900'
                  : 'border-slate-300 bg-white text-slate-700 hover:bg-slate-50'
              }`}
            >
              {p}
            </button>
          );
        })}

        <button
          type="button"
          onClick={() => onPageChange(page + 1)}
          disabled={loading || page >= totalPages}
          aria-label="Next page"
          className="inline-flex h-8 w-8 items-center justify-center rounded border border-slate-300 bg-white text-slate-700 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-40"
        >
          <ChevronRight className="h-4 w-4" />
        </button>
      </div>

      {/* Right: Showing x-y of N + Per page */}
      <div className="flex flex-wrap items-center justify-between gap-4 text-xs text-slate-600 sm:justify-end">
        <span>
          Showing {start}–{end} of {totalCount}
        </span>

        <label className="inline-flex items-center gap-1.5">
          <span className="sr-only">Rows per page</span>
          <select
            value={pageSize}
            disabled={loading}
            onChange={(e) => onPageSizeChange(Number(e.target.value))}
            className="rounded border border-slate-300 bg-white px-2.5 py-1.5 text-xs font-medium text-slate-700 focus:border-slate-500 focus:outline-hidden disabled:opacity-45"
          >
            <option value={5}>Per page: 5</option>
            <option value={10}>Per page: 10</option>
            <option value={20}>Per page: 20</option>
          </select>
        </label>
      </div>
    </div>
  );
}