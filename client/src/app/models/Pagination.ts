export interface Pagination {
    pageIndex: number;
    pageSize: number;
    count: number;
    totalPages: number;
}

export class PaginatedResponse<T> {
    data?: T;
    pageIndex?: number;
    pageSize?: number;
    count?: number;
}

export class PaginatedResult<T> {
    data?: T;
    pagination?: Pagination = { pageIndex: 0, pageSize: 0, count: 0, totalPages: 0};
}