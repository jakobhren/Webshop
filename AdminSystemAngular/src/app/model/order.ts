import { OrderItem } from "./order-item";

export interface Order {
    orderId: number;
    customeremail: string;
    orderDate: Date;
    orderTotal: number;
    orderItems: OrderItem[];
}
