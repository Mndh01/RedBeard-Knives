export class Review {
    id: number = 0;
    body: string = '';
    createdAt: Date = new Date();
    authorName: string = '';
    authorId: number = 0;
    productId: number = 0;
    rating: number = 0;

    constructor(body?: string, productId?: number) {
        if (body && productId) {
            this.body = body;
            this.productId = productId;
        }
    }
}