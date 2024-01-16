import { EventEmitter, Injectable } from '@angular/core';
import{HttpClient} from '@angular/common/http'
import { login, signUp } from './dataTypes';
import { BehaviorSubject, observeOn } from 'rxjs';
import {  Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ServicesSellerService {
  private baseUrl:string=`http://localhost:3000/seller`;
  isSellerLoggedIn=new BehaviorSubject<boolean>(false);
  isLoginError=new EventEmitter<boolean>(false)

  constructor(private http:HttpClient, private router:Router) { }
  userSignUp(data:signUp){
     this.http.post(this.baseUrl,data,{observe:'response'}).subscribe((result)=>{
      console.warn(result)
      if(result)
      {
        localStorage.setItem('seller',JSON.stringify(result.body))
        this.router.navigate(['seller-home'])
      }
    })
  }
  reloadSeller(){
    if(localStorage.getItem('seller')){
      this.isSellerLoggedIn.next(true)
      this.router.navigate(['seller-home'])

    }
  }
  userLogin(data: login) {
    this.http
      .get(`${this.baseUrl}?email=${data.email}&password=${data.password}`, { observe: 'response' })
      .subscribe((result) => {
        const responseBody = result.body;

        if (Array.isArray(responseBody) && responseBody.length === 1) {
          this.isLoginError.emit(false)
          localStorage.setItem('seller',JSON.stringify(result.body))
          this.router.navigate(['seller-home'])

        }
        else{
          console.warn("login failed");
          this.isLoginError.emit(true)
        }
      });
  }

  }

