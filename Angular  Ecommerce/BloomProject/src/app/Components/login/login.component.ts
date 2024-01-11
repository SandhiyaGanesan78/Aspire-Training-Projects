import { Token } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators,FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import validateForm from 'src/app/Helpers/validateForm';
import { AuthService } from 'src/app/Services/auth.service';
import { CartService } from 'src/app/Services/cart.service';
import { ProductsService } from 'src/app/Services/products.service';
import { UserStoreService } from 'src/app/Services/user-store.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm1!: FormGroup;
  public role: any="";
  cartList: any[] | undefined;
  constructor(private formBuilder:FormBuilder, private auth:AuthService,
    private route:Router, private toast:NgToastService, private userStore:UserStoreService,private product:ProductsService,
    private cart: CartService ){

  }
  ngOnInit(): void {
    this.loginForm1=this.formBuilder.group({
      password:["",[
        Validators.required]],
      email:["",[Validators.required,Validators.email]]
    });
    this.userStore.getRoleFromStore().subscribe(value=>{
      const rolefromtoken= this.auth.getRoleFromToken();
      this.role=value|| rolefromtoken;
    })
  }
  OnLogin() {
    if(this.loginForm1.valid){
      this.auth.login(this.loginForm1.value).subscribe({
        next:(result)=>{
          this.auth.storeToken(result.token);
          this.auth.userLogin(result.token)
          const tokenPayload=this.auth.decodeToken();
          this.userStore.setFullNameForStore(tokenPayload.email);
          this.userStore.setRoleForStore(tokenPayload.role);
          this.toast.success({detail:"Success",summary:result.message,duration:3000,position:'topCenter'})
          if(this.role==='Admin'){
            this.route.navigate(['admin/home'])
          }else if(this.role==='User'){
            this.route.navigate([''])
          }

        },
        error:(error)=>{
          this.toast.error({detail:"Error",summary:"Something went wrong",duration:3000,position:'topCenter'})

        }
      })
    }
    else{
      console.log("form is not valid")
      validateForm.validateForm(this.loginForm1);
      alert("your form is")
    }
  }
  cartItemsToRemoteCart() {
    const data = localStorage.getItem('cartItems');

    if (data) {
      const cartDataList: any[] = JSON.parse(data);

      cartDataList.forEach((product: any, index) => {
        const cartData: any = {
          ...product,
          productId: product.id,
          userId: null // We won't set userId here as it will be set on the server side using the user token
        };

        delete cartData.id;

        setTimeout(() => {
          this.cart.addToCart(cartData).subscribe((result) => {
            if (result) {
              console.warn('Data is stored in db');
            }
          });
        }, 500);

        if (cartDataList.length === index + 1) {
          localStorage.removeItem('cartItems');
        }
      });
    }

    setTimeout(() => {
      // Fetch updated cart list from the server
      this.cart.getCartItems().subscribe((cartList) => {
        this.cartList=cartList;
        console.log(this.cartList);
      });
    }, 2000);
  }
}


