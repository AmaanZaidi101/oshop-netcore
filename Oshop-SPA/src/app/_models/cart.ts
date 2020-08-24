import { ProductItem } from './productItem';
import { Product } from './product';
import { CartItem } from './cartItem';

export class Cart {
    id: string;
    cartItems: CartItem[];
    totalQuantity: number;
    totalPrice: number;

    public Cart() {
        this.cartItems = [];
        this.id = '';
    }

    getItemQuantity(product: Product) {
        if (this.cartItems) {
            const element = this.cartItems.find(x => x.product.id === product.id );
            return element.quantity > 0 ? element.quantity : 0 ;
        }
        return 0;
    }
}
