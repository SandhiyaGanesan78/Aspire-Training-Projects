import { Injectable } from "@angular/core";
import { UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { ServicesSellerService } from "./services-seller.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard {

  constructor(private seller:ServicesSellerService) { }
  canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
      if(localStorage.getItem('seller'))
      {
        return true
      }
    return this.seller.isSellerLoggedIn;
  }
}
