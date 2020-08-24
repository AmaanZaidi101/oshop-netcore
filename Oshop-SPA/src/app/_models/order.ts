import { ProductItem } from './productItem';
import { Shipping } from './shipping';

export class Order {
    id: string;
    datePlaced?: Date;
    status: string;
    userId: string;
    productItems: ProductItem[];
    orderShipping: Shipping;
    orderShippingId: string;
}
