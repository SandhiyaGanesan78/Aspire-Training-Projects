import { Injectable } from "@angular/core";
import { Router, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../Services/auth.service";
import { NgToastService } from "ng-angular-popup";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(private auth:AuthService, private router:Router, private toast:NgToastService) { }
  canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {

   if(this.auth.isLoggedIn()){
    return true;
   }
   else{
    this.toast.error({detail:"Error",summary:"Please login first",position:"topCenter"});
    this.router.navigate(['login'])
    return false;
   }
}
}
