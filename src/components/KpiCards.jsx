import { Users, Wallet, Coins, CheckSquare, UserCheck } from 'lucide-react';
import KpiCard from './KpiCard';

const formatPeso = (amount) =>
  `₱${Number(amount ?? 0).toLocaleString('en-PH', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  })}`;

const formatCount = (num) => Number(num ?? 0).toLocaleString('en-PH');

export default function KpiCards({ summary, loading }) {
  return (
    <section
      aria-label="Key Performance Indicators"
      className="grid grid-cols-1 gap-3.5 sm:grid-cols-2 lg:grid-cols-5"
    >
      <KpiCard
        title="Total Registrations"
        value={formatCount(summary?.totalRegistrations)}
        subtitle={`${formatCount(summary?.pending)} pending · ${formatCount(summary?.cancelled)} cancelled`}
        icon={Users}
        loading={loading}
      />
      <KpiCard
        title="Total Payments"
        value={formatPeso(summary?.totalPayments)}
        subtitle={`Billed: ${formatPeso(summary?.totalFees)}`}
        icon={Wallet}
        loading={loading}
      />
      <KpiCard
        title="Uncollected Payments"
        value={formatPeso(summary?.uncollectedPayments)}
        subtitle="Remaining balance"
        icon={Coins}
        loading={loading}
      />
      <KpiCard
        title="Confirmed"
        value={formatCount(summary?.confirmed)}
        subtitle="Verified entries"
        icon={CheckSquare}
        loading={loading}
      />
      <KpiCard
        title="Checked In"
        value={formatCount(summary?.checkedIn)}
        subtitle="On-site participants"
        icon={UserCheck}
        loading={loading}
      />
    </section>
  );
}