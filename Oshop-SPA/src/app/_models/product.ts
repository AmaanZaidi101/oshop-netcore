import { Category } from './category';

export class Product {
    id: string;
    imageUrl: string;
    price: number;
    name: string;
    categoryId: string;
    soldOut: boolean;
    weight: number;
}
