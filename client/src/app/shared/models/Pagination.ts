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

    setPagination(pagination: Pagination) {
        pagination.pageIndex = this.pageIndex;
        pagination.pageSize = this.pageSize;
        pagination.count = this.count;
        pagination.totalPages = Math.ceil(this.count / this.pageSize);
    }
}