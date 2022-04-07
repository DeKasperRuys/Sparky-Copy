import { Component, OnInit, Inject, Input, ElementRef, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
declare var paypal;
@Component({
  selector: 'app-paypall-dialog',
  templateUrl: './paypall-dialog.component.html',
  styleUrls: ['./paypall-dialog.component.scss']
})
export class PaypallDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<PaypallDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.price = data.price;
  }


  @Input() price: number;
  @ViewChild('paypal', {static: true}) paypalElement: ElementRef;


  product = {
    price: 2.5, // price halen van database
    description: '', // halen van database
  };

  paidFor = false;

  ngOnInit() {
    this.product.price = this.price;
    console.log(this.price);
    paypal
    .Buttons({
      createOrder: (data, actions) => {
        return actions.order.create({
          purchase_units: [
            {
              description: this.product.description,
              amount: {
                value: this.product.price
              }
            }
          ]
        });
      },
      onApprove: async (data, actions) => {
        const order = await actions.order.capture();
        this.paidFor = true;
        this.dialogRef.close();
        console.log(order);
      },
      onError: err => {
        console.log(err);
      }
    })
    .render(this.paypalElement.nativeElement);
  }
}
