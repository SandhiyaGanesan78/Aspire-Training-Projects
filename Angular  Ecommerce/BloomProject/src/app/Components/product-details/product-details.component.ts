import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { cart, product } from 'src/app/Models/products';
import { AuthService } from 'src/app/Services/auth.service';
import { CartService } from 'src/app/Services/cart.service';
import { ProductsService } from 'src/app/Services/products.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit{
  productData:any|undefined;
  productQuantity:any=1;
  removeCart=false;
  cartData:any|undefined;
  cartList: any[] | undefined;
  constructor(private activeroute: ActivatedRoute, private product: ProductsService,
    private auth:AuthService,
    private cart:CartService){

  }
  ngOnInit(): void {
    let productId= this.activeroute.snapshot.paramMap.get('productId');
    productId && this.product.getProduct(productId).subscribe((result)=>
    {
      this.productData=result;
      let cartData=localStorage.getItem('cartItems')
      if(productId && cartData){
        let items= JSON.parse(cartData);
        items=items.filter((item:any)=>
        productId===item.id.toString());
        if(items.length){
          this.removeCart=true
        }else{
          this.removeCart=false;
        }
      }
      let user =localStorage.getItem('token');
      if(user){
        this.product.cartData.subscribe((result)=>{
          let item= result.filter((item:any)=>productId?.toString()===item.productId?.toString())
          if(item.length){
            this.cartData=item[0];
            this.removeCart=true;
          }
        })
      }

    })
  }
  handleQuantity(val:any){
    if(this.productQuantity<20 && val==='plus')
    {
      this.productQuantity+=1;
    }
    else if(this.productQuantity>1 && val==='min'){
      this.productQuantity-=1;
    }
  }
  addToCart(){
    if (this.productData){
      this.productData.quantity=this.productQuantity;
      if(!localStorage.getItem('user')){
        this.product.addToCart(this.productData);
        this.removeCart=true;
      }else {
        let user =localStorage.getItem('user');
        let tokenid= user && JSON.parse(user).tokenid;
        let cartData: any={
          ...this.productData,
          productId:this.productData.id,
          tokenid
        }
        delete cartData.id;
      // User logged in, add item to the remote cart
      this.cart.addToCart(this.productData).subscribe((result) => {
        if (result) {
          // Success: Fetch the updated cart list from the server using the token
          this.cart.getCartItems().subscribe((cartList) => {
            // Do something with the fetched cart list
            this.cartList=cartList;
        console.log(this.cartList);
          });
          this.removeCart = true;
        }
      });
    }

  }
}
  removeToCart(productId:any){
    if (!this.auth.isLoggedIn()) {
      // User not logged in, remove the item from the local cart
      this.product.removeFromCart(productId);
    } else {
      // User logged in, remove the item from the remote cart
      this.cart.removeFromCart(this.cartData.id).subscribe((result) => {
        if (result) {
          // Success: Fetch the updated cart list from the server using the token
          this.cart.getCartItems().subscribe((cartList) => {
            // Do something with the fetched cart list
            this.cartList=cartList;
        console.log(this.cartList);
          });
        }
      });
    }

    this.removeCart = false;

    }

}
