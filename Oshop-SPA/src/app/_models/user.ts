import { Cart } from './cart';

export interface User {
    id: string;
    userName: string;
    created: string;
    cart: Cart;
    gender: string;
    dateOfBirth: string;
    city: string;
    state: string;
    password: string;
    confirmPassword: string;
    email: string;
    roles?: string[];
}
