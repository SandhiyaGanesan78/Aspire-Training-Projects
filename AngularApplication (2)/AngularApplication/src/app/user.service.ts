import { EventEmitter, Injectable } from '@angular/core';
import { login, signUp } from './dataTypes';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl:string=`http://localhost:3000/users`;
  invalidUserAuth=new EventEmitter<boolean>(false);
  isLoginError= new EventEmitter<boolean>(false);
  isUserLoggedIn = new BehaviorSubject<boolean>(false);
  Message: string | undefined;



  constructor(private http:HttpClient,private router:Router) { }
  userSignUp(user:signUp){
    this.http.post(this.baseUrl,user,{observe:'response'}).subscribe((result)=>
    {
      if(result){
      localStorage.setItem('user',JSON.stringify(result.body));
      this.router.navigate(['/']);
      }
    })
  }
  userAuthReload(){
    if(localStorage.getItem('user')){
      this.router.navigate(['/']);
    }
  }
  userLogin(data:login){
    this.http.get<signUp[]>(`${this.baseUrl}?email=${data.email}&password=${data.password}`,
    {observe:'response'}).subscribe((result)=>{
      if(result && result.body?.length){
        localStorage.setItem('user',JSON.stringify(result.body[0]));
        this.router.navigate(['/']);
        this.invalidUserAuth.emit(false)
      }
      else{
        this.invalidUserAuth.emit(true)
      }
    })
  }
  Login(data: login) {
    this.http
      .get(`http://localhost:3000/seller?email=${data.email}&password=${data.password}`, { observe: 'response' })
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

  adminlogin(data:login){
    this.http.get<signUp[]>(`http://localhost:3000/seller?email=${data.email}&password=${data.password}`,
    { observe: 'response' })
    .subscribe((result:any) =>{
      console.warn(result)
      if(result && result.body && result.body.length){
        this.isLoginError.emit(false)
        console.warn("Admin login done");
        this.Message="Admin logged in"


        this.isUserLoggedIn.next(true)
        localStorage.setItem('admin',JSON.stringify(result.body))
        this.router.navigate(['seller-home'])
        this.invalidUserAuth.emit(false)
      }
      else{
        this.isLoginError.emit(true)
        console.warn("login failed");
        this.invalidUserAuth.emit(true)
      }
    })
  }
}
