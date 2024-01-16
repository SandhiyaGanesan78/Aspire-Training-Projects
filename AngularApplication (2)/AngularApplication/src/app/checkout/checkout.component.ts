import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../products.service';
import { cart, order } from '../dataTypes';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  totalPrice:number|undefined;
  cartData:cart[]|undefined;
  orderMessage:string|undefined;
  checkout:FormGroup;
  constructor(private product:ProductsService, private router:Router, private formBuilder:FormBuilder){
    this.checkout=this.formBuilder.group({
      email:["",[Validators.email,Validators.required]],
      address:["",[Validators.required, Validators.maxLength(150),Validators.minLength(10)]],
      contact:["",[Validators.required,Validators.pattern('^[7-9]\\d{9}$')]]
    })
  }
  ngOnInit(): void {
    this.product.currentCart().subscribe((result)=>{

      let price=0;
      this.cartData=result;
      result.forEach((item)=>{
        if(item.quantity){
          price=price+ (+item.price* +item.quantity)
        }
      })
      this.totalPrice=price+(price/10)+100-(price/10);

      console.warn(this.totalPrice);
    })
  }

  orderNow(data:{email:string,address:string,contact:string})
  {
    let user = localStorage.getItem('user');
    let userId=user && JSON.parse(user).id;
    if(this.totalPrice){
      let orderData:order={
        ...data,
        totalPrice:this.totalPrice,
        userId,
        id:undefined
      }
      this.cartData?.forEach((item)=>{
        setTimeout(() => {
          item.id &&this.product.deleteCartItems(item.id);
        },700);
      })
      this.product.orderNow(orderData).subscribe((result)=>
      {
        if(result){
          this.orderMessage="your order has been placed";
          setTimeout(() => {
            this.router.navigate(['/my-orders']);
            this.orderMessage=undefined;
          }, 4000);
        }
      })

    }
  }
}
