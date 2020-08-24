import { BrowserModule, HAMMER_GESTURE_CONFIG, HammerGestureConfig } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HttpClientModule } from '@angular/common/http';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';

import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { HomeComponent } from './home/home.component';
import { ProductsComponent } from './products/products.component';
import { JwtModule } from '@auth0/angular-jwt';
import { ProductCardComponent } from './product-card/product-card.component';
import { ProductQuantityComponent } from './product-quantity/product-quantity.component';
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
import { HasRoleDirective } from './_directives/hasRole.directive';
import { UserManagementComponent } from './user-management/user-management.component';
import { UserManagementResolver } from './_resolvers/user-management.resolver';
import { RolesModalComponent } from './roles-modal/roles-modal.component';
import { AdminProductsComponent } from './admin-products/admin-products.component';
import { AdminProductsResolver } from './_resolvers/admin-products.resolver';

export function tokenGetter() {
   return localStorage.getItem('token');
}

export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      ProductsComponent,
      ProductCardComponent,
      ProductQuantityComponent,
      ProductFormComponent,
      CartComponent,
      CheckOutComponent,
      CartSummaryComponent,
      OrderSuccessComponent,
      OrderDetailComponent,
      MyOrdersComponent,
      AdminOrdersComponent,
      RegisterComponent,
      UserManagementComponent,
      RolesModalComponent,
      AdminProductsComponent,
      HasRoleDirective
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      HttpClientModule,
      AppRoutingModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      ButtonsModule.forRoot(),
      PaginationModule.forRoot(),
      BsDatepickerModule.forRoot(),
      ModalModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   entryComponents: [
      RolesModalComponent
   ],
   providers: [
      ProductResolver,
      ProductsResolver,
      CartResolver,
      OrderDetailResolver,
      MyOrdersResolver,
      AdminOrdersResolver,
      UserManagementResolver,
      AdminProductsResolver,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
