import { Product } from './product';

export interface ProductItem {
    id: string;
    quantity: number;
    totalPrice: number;
    product: Product;
}
