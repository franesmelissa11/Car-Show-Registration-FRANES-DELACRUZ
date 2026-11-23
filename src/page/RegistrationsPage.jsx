import { useState, useEffect, useCallback } from 'react';
import { getRegistrations, getSummary } from '../api/index.js';
import { useDebounce } from '../hooks/useDebounce.js';
import KpiCards from '../components/KpiCards.jsx';
import RegistrationFilters from '../components/RegistrationsFilter.jsx';
import RegistrationTable from '../components/RegistrationTable.jsx';
import Pagination from '../components/Pagination.jsx';
import NewRegistrationModal from '../components/NewRegistrationModal.jsx';

export default function RegistrationsPage() {
  const [searchInput, setSearchInput] = useState('');
  const [typeFilter, setTypeFilter] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);

  const debouncedName = useDebounce(searchInput, 400);

  const [summary, setSummary] = useState(null);
  const [summaryLoading, setSummaryLoading] = useState(true);

  const [data, setData] = useState({
    items: [],
    page: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 1,
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const loadSummary = useCallback(async () => {
    setSummaryLoading(true);
    try {
      const res = await getSummary();
      setSummary(res);
    } catch {
      // Handled via table error state if API is down
    } finally {
      setSummaryLoading(false);
    }
  }, []);

  const loadRegistrations = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const res = await getRegistrations({
        participantName: debouncedName,
        type: typeFilter,
        status: statusFilter,
        page,
        pageSize,
      });
      setData(res);
    } catch (err) {
      setError(err.message || 'Unable to reach the registration API.');
    } finally {
      setLoading(false);
    }
  }, [debouncedName, typeFilter, statusFilter, page, pageSize]);

  useEffect(() => {
    const timer = setTimeout(() => {
      void loadSummary();
    }, 0);

    return () => clearTimeout(timer);
  }, [loadSummary]);

  useEffect(() => {
    const timer = setTimeout(() => {
      void loadRegistrations();
    }, 0);

    return () => clearTimeout(timer);
  }, [loadRegistrations]);

  const handleSearchChange = (val) => {
    setSearchInput(val);
    setPage(1);
  };

  const handleTypeChange = (val) => {
    setTypeFilter(val);
    setPage(1);
  };

  const handleStatusChange = (val) => {
    setStatusFilter(val);
    setPage(1);
  };

  const handleClearFilters = () => {
    setSearchInput('');
    setTypeFilter('');
    setStatusFilter('');
    setPage(1);
  };

  const handlePageSizeChange = (size) => {
    setPageSize(size);
    setPage(1);
  };

  const handleRefreshAll = () => {
    loadSummary();
    loadRegistrations();
  };

  return (
    <div className="min-h-screen bg-slate-100/70 py-5 px-4 sm:px-6 lg:px-8">
      <div className="mx-auto max-w-6xl space-y-4">
        {/* Top Header Bar */}
        <header className="flex items-center justify-between rounded-lg border border-slate-300 bg-white px-4 py-3">
          <div className="flex items-center gap-3">
            <div className="flex h-9 w-9 items-center justify-center rounded border border-slate-300 bg-slate-50 text-slate-700">
              <svg className="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <path d="M19 17h2c.6 0 1-.4 1-1v-3c0-.9-.7-1.7-1.5-1.9C18.7 10.6 16 10 16 10s-1.3-1.4-2.2-2.3c-.5-.4-1.1-.7-1.8-.7H5c-.6 0-1.1.4-1.4.9l-1.4 2.9A3.7 3.7 0 0 0 2 12v4c0 .6.4 1 1 1h2" />
                <circle cx="7" cy="17" r="2" />
                <path d="M9 17h6" />
                <circle cx="17" cy="17" r="2" />
              </svg>
            </div>
            <div>
              <h1 className="text-lg font-bold leading-tight text-slate-900">
                Car Show 2027
              </h1>
              <p className="text-xs text-slate-400">
                AutoVista Events · Sorsogon City Coliseum
              </p>
            </div>
          </div>

          <button
            type="button"
            onClick={() => setIsModalOpen(true)}
            className="rounded-md border border-slate-300 bg-slate-900 px-3.5 py-1.5 text-xs font-semibold text-white transition hover:bg-slate-800"
          >
            + New Registration
          </button>
        </header>

        {/* KPI Cards Row */}
        <KpiCards summary={summary} loading={summaryLoading} />

        {/* Unified Registrations Container */}
        <section
          aria-label="Registrations"
          className="rounded-lg border border-slate-300 bg-white"
        >
          <div className="flex flex-col gap-3 px-4 py-3.5 lg:flex-row lg:items-center lg:justify-between">
            <h2 className="text-base font-bold text-slate-900">Registrations</h2>

            <RegistrationFilters
              searchInput={searchInput}
              onSearchChange={handleSearchChange}
              typeFilter={typeFilter}
              onTypeChange={handleTypeChange}
              statusFilter={statusFilter}
              onStatusChange={handleStatusChange}
              onClearFilters={handleClearFilters}
            />
          </div>

          <RegistrationTable
            items={data.items}
            loading={loading}
            error={error}
            onRetry={handleRefreshAll}
            onClearFilters={handleClearFilters}
            rowsCount={pageSize}
          />

          {!error && data.totalCount > 0 && (
            <Pagination
              page={data.page}
              pageSize={data.pageSize}
              totalCount={data.totalCount}
              totalPages={data.totalPages}
              loading={loading}
              onPageChange={setPage}
              onPageSizeChange={handlePageSizeChange}
            />
          )}
        </section>
      </div>

      <NewRegistrationModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={handleRefreshAll}
      />
    </div>
  );
}
