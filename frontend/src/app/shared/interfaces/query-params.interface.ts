export interface QueryParams {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  [key: string]: string | number | undefined;
}

