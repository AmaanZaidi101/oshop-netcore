import { Product } from './product';

export interface CartItem {
    id: string;
    quantity: number;
    totalPrice: number;
    product: Product;
    productId: string;
}
