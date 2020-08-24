import { HomeComponent } from './home/home.component';
import { Routes } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { ProductFormComponent } from './product-form/product-form.component';
import { ProductResolver } from './_resolvers/product.resolver';
import { ProductsResolver } from './_resolvers/products.resolver';
import { CartResolver } from './_resolvers/cart.resolver';
import { CartComponent } from './cart/cart.component';
import { CheckOutComponent } from './check-out/check-out.component';
import { CartSummaryComponent } from './cart-summary/cart-summary.component';
import { OrderSuccessComponent } from './order-success/order-success.component';
import { OrderDetailComponent } from './order-detail/order-detail.component';
import { OrderDetailResolver } from './_resolvers/order-detail.resolver';
import { MyOrdersComponent } from './my-orders/my-orders.component';
import { MyOrdersResolver } from './_resolvers/my-orders.resolver';
import { AdminOrdersComponent } from './admin-orders/admin-orders.component';
import { AdminOrdersResolver } from './_resolvers/admin-orders.resolver';
import { RegisterComponent } from './register/register.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserManagementComponent } from './user-management/user-management.component';
import { UserManagementResolver } from './_resolvers/user-management.resolver';
import { AdminProductsComponent } from './admin-products/admin-products.component';
import { AdminProductsResolver } from './_resolvers/admin-products.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  {
      path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [AuthGuard],
      children: [
        {
            path: 'products',
            component: ProductsComponent,
            resolve: { products: ProductsResolver },
          },
          {
            path: 'admin/product/:id',
            component: ProductFormComponent,
            resolve: { product: ProductResolver },
          },
          {
            path: 'admin/products',
            component: AdminProductsComponent,
            resolve: { products: AdminProductsResolver}
          },
          { path: 'cart', component: CartComponent },
          { path: 'check-out', component: CheckOutComponent },
          { path: 'cart-summary', component: CartSummaryComponent },
          { path: 'order-success/:id', component: OrderSuccessComponent },
          {
            path: 'my/orders/:id',
            component: OrderDetailComponent,
            resolve: { order: OrderDetailResolver },
          },
          {
            path: 'my/orders',
            component: MyOrdersComponent,
            resolve: { orders: MyOrdersResolver },
          },
          {
            path: 'admin/orders',
            component: AdminOrdersComponent,
            resolve: { adminOrders: AdminOrdersResolver },
            data: { roles: ['Admin', 'Moderator'] },
          },
          {
            path: 'admin/users',
            component: UserManagementComponent,
            resolve: { users: UserManagementResolver},
            data: {roles: ['Admin']}
          }
      ]
  }

];
