import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/Services/products.service';
import { faTrash, faEdit } from '@fortawesome/free-solid-svg-icons';
import { product } from 'src/app/Models/products';
import { Router } from '@angular/router';


@Component({
  selector: 'app-Admin-home',
  templateUrl: './Admin-home.component.html',
  styleUrls: ['./Admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  productList:any;
  productMessage:any;
  icon=faTrash;
  iconEdit=faEdit;
  constructor(private product:ProductsService,private route:Router) { }

  ngOnInit() {
    this.list();


}
updateProduct(id:any)
{
  this.product.getProduct(id).subscribe((result)=>{
    if(result){
      this.route.navigate(['admin-update-product', id])
    this.list();
  }
  });
}
deleteProduct(id:any){
  this.product.deleteProduct(id).subscribe((result)=>{
    if(result){
    this.productMessage="Product is deleted";
    this.list();
  }
  });
  setTimeout(()=>{
    this.productMessage=undefined;
  },3000);
  }
  list(){
    this.product.productlist().subscribe((result)=>{
      if(result){
        this.productList=result;
      }
    });
  }

}

