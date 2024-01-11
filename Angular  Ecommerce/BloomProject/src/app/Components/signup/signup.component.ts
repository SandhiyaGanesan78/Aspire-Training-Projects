import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators,FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import validateForm from 'src/app/Helpers/validateForm';
import { AuthService } from 'src/app/Services/auth.service';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  signupForm!:FormGroup;

  constructor(private formBuilder:FormBuilder,
    private auth:AuthService,
     private route:Router,private toast:NgToastService){

  }
  ngOnInit(): void {
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
  }
  OnSignup(){
    if(this.signupForm.valid){
      console.log(this.signupForm.value);
      this.auth.signUp(this.signupForm.value).subscribe({
        next:(result)=>{
          this.toast.success({detail:"Success",summary:result.message,duration:3000,position:'topCenter'})
          this.signupForm.reset();
          this.route.navigate(['login'])
        },
        error:(error)=>{
          this.toast.error({detail:"Error",summary:"Something went wrong",duration:3000,position:'topCenter'})
        }
      })
    }
    else{
      console.log("form is not valid")
      validateForm.validateForm(this.signupForm);
      alert("your form is invalid")

    }


    }




  }

