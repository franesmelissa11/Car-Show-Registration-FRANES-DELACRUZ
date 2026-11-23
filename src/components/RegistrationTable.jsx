import StatusBadge from './StatusBadge';
import PaymentBadge from './PaymentBadge';

const formatCurrency = (amount) =>
  `₱${Number(amount ?? 0).toLocaleString('en-PH', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  })}`;

const formatDateTime = (isoString) => {
  if (!isoString) return '—';
  return new Intl.DateTimeFormat('en-US', {
    month: 'short',
    day: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: true,
  }).format(new Date(isoString));
};

export default function RegistrationTable({
  items,
  loading,
  error,
  onRetry,
  onClearFilters,
  rowsCount = 6,
}) {
  if (error) {
    return (
      <div className="border-y border-slate-200 bg-rose-50/50 px-6 py-10 text-center">
        <p className="text-sm font-semibold text-rose-800">Cannot reach the API...</p>
        <p className="mt-1 text-xs text-rose-600">{error}</p>
        <button
          type="button"
          onClick={onRetry}
          className="mt-4 rounded-md border border-rose-300 bg-white px-4 py-1.5 text-xs font-semibold text-rose-700 shadow-2xs transition hover:bg-rose-50"
        >
          Try again
        </button>
      </div>
    );
  }

  if (!loading && items.length === 0) {
    return (
      <div className="border-y border-slate-200 bg-white px-6 py-10 text-center">
        <p className="text-sm font-semibold text-slate-800">No registrations match your filters</p>
        <p className="mt-1 text-xs text-slate-500">
          Adjust or reset your active search and dropdown filters to see results.
        </p>
        <button
          type="button"
          onClick={onClearFilters}
          className="mt-4 rounded-md border border-slate-300 bg-white px-4 py-1.5 text-xs font-semibold text-slate-700 transition hover:bg-slate-50"
        >
          Clear filters
        </button>
      </div>
    );
  }

  return (
    <div className="overflow-x-auto border-y border-slate-200">
      {loading && (
        <div className="bg-slate-50 px-4 py-1.5 text-xs text-slate-500 border-b border-slate-200">
          Loading registrations...
        </div>
      )}

      <table className="min-w-full divide-y divide-slate-200 text-left text-sm">
        <thead className="bg-white text-xs font-semibold text-slate-700">
          <tr>
            <th scope="col" className="px-4 py-3">Reg. No.</th>
            <th scope="col" className="px-4 py-3">Date</th>
            <th scope="col" className="px-4 py-3">Participant</th>
            <th scope="col" className="px-4 py-3">Car</th>
            <th scope="col" className="px-4 py-3">Type</th>
            <th scope="col" className="px-4 py-3">Status</th>
            <th scope="col" className="px-4 py-3 text-right">Fee / Paid</th>
            <th scope="col" className="px-4 py-3">Payment</th>
          </tr>
        </thead>

        <tbody className="divide-y divide-slate-200 bg-white">
          {loading
            ? Array.from({ length: Math.min(rowsCount, 6) }).map((_, index) => (
                <tr key={index} className="animate-pulse">
                  <td className="px-4 py-3.5"><div className="h-3.5 w-20 rounded bg-slate-200" /></td>
                  <td className="px-4 py-3.5"><div className="h-3.5 w-28 rounded bg-slate-200" /></td>
                  <td className="px-4 py-3.5">
                    <div className="h-3.5 w-32 rounded bg-slate-200" />
                    <div className="mt-1.5 h-2.5 w-24 rounded bg-slate-100" />
                  </td>
                  <td className="px-4 py-3.5">
                    <div className="h-3.5 w-32 rounded bg-slate-200" />
                    <div className="mt-1.5 h-2.5 w-16 rounded bg-slate-100" />
                  </td>
                  <td className="px-4 py-3.5"><div className="h-3.5 w-16 rounded bg-slate-200" /></td>
                  <td className="px-4 py-3.5"><div className="h-5 w-20 rounded-md bg-slate-200" /></td>
                  <td className="px-4 py-3.5"><div className="ml-auto h-3.5 w-24 rounded bg-slate-200" /></td>
                  <td className="px-4 py-3.5"><div className="h-5 w-16 rounded-md bg-slate-200" /></td>
                </tr>
              ))
            : items.map((row) => (
                <tr key={row.id} className="hover:bg-slate-50/80">
                  <td className="whitespace-nowrap px-4 py-3 text-xs font-medium text-slate-700">
                    {row.registrationNumber}
                  </td>
                  <td className="whitespace-nowrap px-4 py-3 text-xs text-slate-600">
                    {formatDateTime(row.registrationDate)}
                  </td>
                  <td className="px-4 py-3">
                    <div className="text-sm font-medium text-slate-900">
                      {row.participantName}
                    </div>
                    <div className="text-xs text-slate-400">
                      {row.contactNumber}
                    </div>
                  </td>
                  <td className="px-4 py-3">
                    <div className="text-sm text-slate-800">{row.carMakeModel}</div>
                    <div className="text-xs text-slate-400">{row.plateNumber}</div>
                  </td>
                  <td className="whitespace-nowrap px-4 py-3 text-xs text-slate-700">
                    {row.registrationType}
                  </td>
                  <td className="whitespace-nowrap px-4 py-3">
                    <StatusBadge status={row.status} />
                  </td>
                  <td className="whitespace-nowrap px-4 py-3 text-right text-xs">
                    <div className="font-medium text-slate-800">
                      {formatCurrency(row.registrationFee)}
                    </div>
                    <div className="text-slate-400">
                      {formatCurrency(row.amountPaid)}
                    </div>
                  </td>
                  <td className="whitespace-nowrap px-4 py-3">
                    <PaymentBadge paymentStatus={row.paymentStatus} />
                  </td>
                </tr>
              ))}
        </tbody>
      </table>
    </div>
  );
}