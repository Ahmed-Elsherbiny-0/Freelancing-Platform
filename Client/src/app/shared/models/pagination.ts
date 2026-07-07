export type Pagination<T> = {
  pageSize: number;
  pageIndex: number;
  count: number;
  data: T[];
};
export type WorkerParms = {
  pageIndex?: number | null;
  pageSize?: number | null;
  country?: string | null;
  rating?: number | null;
  search?: string | null;
};
export type JobParms = {
  pageIndex?: number | null;
  pageSize?: number | null;
  search?: string | null;
  budget?: number | null;
  status?: string | null;
};
export type OfferParms = {
  pageIndex?: number | null;
  pageSize?: number | null;
};
