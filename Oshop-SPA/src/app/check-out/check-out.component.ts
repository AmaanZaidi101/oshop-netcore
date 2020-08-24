import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Shipping } from '../_models/shipping';
import { ShippingService } from '../_services/shipping.service';
import { AlertifyService } from '../_services/alertify.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-check-out',
  templateUrl: './check-out.component.html',
  styleUrls: ['./check-out.component.css']
})
export class CheckOutComponent implements OnInit {

  updateMode = false;
  shippings: Shipping[];
  newShipping: Shipping = new Shipping();
  currentShipping: Shipping;
  editingCopy: Shipping;

  constructor(private authService: AuthService, private shippingService: ShippingService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.shippingService.getShippings(this.authService.decodedToken.nameid).subscribe(next => {
      this.shippings = next;
    });
    this.currentShipping = this.newShipping;
  }

  makePreferred(shipping: Shipping) {
    const original = this.shippings.find(x => x.isPreferred);
    original.isPreferred = false;
    shipping.isPreferred = true;
    this.shippingService.updateShipping(this.authService.decodedToken.nameid, shipping).subscribe(next => {
      this.alertify.success('Shipping preference changed');
    }, error => {
      this.alertify.error('Could not update shipping address');
      original.isPreferred = false;
      shipping.isPreferred = true;
    });
  }

  addShipping(f: NgForm) {
    this.shippingService.addShipping(this.authService.decodedToken.nameid, this.currentShipping).subscribe(next => {
      this.alertify.success('Shipping address added');
      this.shippings.push(next);
      f.resetForm();
    }, error => this.alertify.error('Could not add shipping address'));
  }

  deleteShipping(shipping: Shipping) {
    this.shippingService.deleteShipping(this.authService.decodedToken.nameid, shipping).subscribe(() => {
      this.alertify.message('Shipping address removed');
      this.shippings.splice(this.shippings.findIndex(x => x.id === shipping.id), 1);
    }, error => this.alertify.error('Could not remove shipping address'));
  }

  editShipping(shipping: Shipping) {
    this.updateMode = true;
    if (this.editingCopy) {
      if (this.editingCopy.id === shipping.id) {
        return;
      }
      const index = this.shippings.findIndex(x => x.id === this.editingCopy.id);
      this.shippings[index] = Object.assign({}, this.editingCopy);
    }
    this.currentShipping = shipping;
    this.editingCopy = Object.assign({}, shipping);
  }

  updateShipping(f: NgForm) {
    this.shippingService.updateShipping(this.authService.decodedToken.nameid, this.currentShipping).subscribe(next => {
      this.alertify.success('Shipping address updated');
      this.editingCopy = null;
      this.currentShipping = this.newShipping;
      this.updateMode = false;
      f.resetForm();
    }, error => this.alertify.error('Could not update shipping'));
  }

  addMode(f: NgForm) {
    if (this.editingCopy) {
      const index = this.shippings.findIndex(x => x.id === this.editingCopy.id);
      this.shippings[index] = Object.assign({}, this.editingCopy);
      this.editingCopy = null;
    }
    f.resetForm();
    this.currentShipping = this.newShipping;
    this.updateMode = false;
  }

}
