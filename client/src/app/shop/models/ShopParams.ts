export class ShopParams {
    pageIndex = 1;
    pageSize = 10;
    categoryId = 0;
    sort = 'name';
    search: string = '';
    price: number;
    inStock: boolean; // TODO: implement in stock filter
    
    constructor(pageSize: number = 10) {
        this.pageSize = pageSize;
    }
}