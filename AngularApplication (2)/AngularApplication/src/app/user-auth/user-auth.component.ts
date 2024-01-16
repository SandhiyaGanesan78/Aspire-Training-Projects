import { Component, OnInit } from '@angular/core';
import { cart, login, product, signUp } from '../dataTypes';
import { UserService } from '../user.service';
import { ProductsService } from '../products.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ServicesSellerService } from '../services-seller.service';


@Component({
  selector: 'app-user-auth',
  templateUrl: './user-auth.component.html',
  styleUrls: ['./user-auth.component.css']
})
export class UserAuthComponent implements OnInit{
  showLogin:boolean=true;
  authError:string="";
  signupForm:FormGroup;
  loginForm1:FormGroup;

  constructor(private user: UserService, private product:ProductsService, private formBuilder:FormBuilder,private seller:ServicesSellerService)
  {
    this.signupForm=formBuilder.group({
      name:["",[Validators.required,Validators.minLength(5),Validators.maxLength(25)]],
      password:["",[
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(15),
        Validators.pattern(
          '^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*]).{8,15}$'
        ),
      ]],
      email:["",[Validators.required,Validators.email]]
    });
    this.loginForm1=formBuilder.group({
      password:["",[
        Validators.required]],
      email:["",[Validators.required,Validators.email]]
    });
  }
  ngOnInit(): void {
    this.user.userAuthReload();
    }
  signUp(data: signUp){
this.user.userSignUp(data);
console.warn(data.name);
}

  Login(data:login){

    this.user.userLogin(data);
    this.user.invalidUserAuth.subscribe((result)=>{
      console.warn(result);
      if(result){
        this.authError="user not found"
      }
      this.user.Login(data)
      {
       this.user.isLoginError.subscribe((result)=>
        {
          if(result){this.authError="email or password invalid"}
       })
      }

    })
  }
  openSignUp(){
    this.showLogin=false

  }
  openLogin(){
    this.showLogin=true;

  }
  localCartToRemoteCart()
  {
    let data= localStorage.getItem('localCart');
    let user=localStorage.getItem('user');
    let userId= user&& JSON.parse(user).id;
    if(data){
      let cartDataList:product[]= JSON.parse(data);

    cartDataList.forEach((product:product,index)=>{
      let cartData:cart={
        ...product,productId:product.id,
        userId
      }
      delete cartData.id;
      setTimeout(()=>{
        this.product.addToCart(cartData).subscribe((result)=>{
          if(result){
            console.warn("data is stored in db");
          }
        })
      },500)
      if(cartDataList.length===index+1){
        localStorage.removeItem('localCart');
      }
    })
    }
    setTimeout(()=>
    {
      this.product.getCartList(userId)
    },2000);
  }

}
