export interface Paging<T> {
  Items: T[];
  PageSize: number;
  PageNumber: number;
  PagesCount: number;
  TotalCount: number;
}
