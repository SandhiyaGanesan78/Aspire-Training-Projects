import { Injectable } from '@angular/core';
import{ HttpClient } from "@angular/common/http"
import { Router } from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt'

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl:string="http://localhost:5176/User/";
  private userPayload:any;


  constructor(private http: HttpClient,private route:Router) {
    this.userPayload=this.decodeToken();
   }
   signUp(user:any){
    return this.http.post<any>(`${this.baseUrl}register`,user);
   }
   login(user:any){
    return this.http.post<any>(`${this.baseUrl}authenticate`,user);

   }
   userLogin(data: any) {
    localStorage.setItem('user', JSON.stringify(data));
  }



   storeToken(tokenValue:any){
    localStorage.setItem('token',tokenValue)
   }
   getToken(){
    return localStorage.getItem('token')
   }
   isLoggedIn():boolean{
    return !!localStorage.getItem('token')
   }
   signOut()
   {
    localStorage.clear();
    this.route.navigate(['login'])
   }

   decodeToken(){
    const jwtHelper= new JwtHelperService();
    const token=this.getToken()!;
    console.log(jwtHelper.decodeToken(token))
    return jwtHelper.decodeToken(token)
   }
   getFullNameFromToken(){
    if(this.userPayload)
    {
      return this.userPayload.email;

    }

   }
   getRoleFromToken(){
    if(this.userPayload)
    {
      return this.userPayload.role;
    }
   }
}
