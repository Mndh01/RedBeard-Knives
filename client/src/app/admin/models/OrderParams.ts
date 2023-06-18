import { OrderStatus } from "src/app/user-profile/orders/OrderStatus";

export class OrderParams {
    buyerEmail: string;
    status: number = OrderStatus.Pending;
    orderBy: string = "id";
    startDate: Date;
    endDate: Date;
    pageIndex: number;
    pageSize: number;
}