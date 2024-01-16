import { Component, OnInit } from '@angular/core';
import { ServicesSellerService } from '../services-seller.service';
import { signUp } from '../dataTypes';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-seller-auth',
  templateUrl: './seller-auth.component.html',
  styleUrls: ['./seller-auth.component.css']
})
export class SellerAuthComponent implements OnInit {
  showLogin=false;
  authError:string='';
  signupForm: FormGroup;
  loginForm:FormGroup;
  constructor(private seller:ServicesSellerService, private formBuilder:FormBuilder){
    this.signupForm=this.formBuilder.group({
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
    this.loginForm=this.formBuilder.group({
      password:["",[
        Validators.required]],
      email:["",[Validators.required,Validators.email]]
    })
  }
  ngOnInit(): void {
    this.seller.reloadSeller()
  }

signUp(data:signUp):void{
  console.warn(data)
  this.seller.userSignUp(data)

}
Login(data:signUp):void{
  console.warn(data)
  this.seller.userLogin(data);
  this.seller.isLoginError.subscribe((isError)=>{
    if(isError){
      this.authError="Email or Password is Incorrect"
    }
  })

}
openLogin()
{
  this.showLogin=true
}
openSignup(){
  this.showLogin=false
}
}
