import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { product } from 'src/app/Models/products';
import { ProductsService } from 'src/app/Services/products.service';

@Component({
  selector: 'app-admin-add-product',
  templateUrl: './admin-add-product.component.html',
  styleUrls: ['./admin-add-product.component.css']
})
export class AdminAddProductComponent implements OnInit {
  addProductMessage:any;
  addProduct: FormGroup;
  constructor(private product: ProductsService, private formBuilder: FormBuilder)
{
  this.addProduct = this.formBuilder.group({
    name: ["", [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
    price: ["", [Validators.required]],
    category: ["", [Validators.required]],
    description: ["", [Validators.required, Validators.maxLength(500)]],
    image: ["", [Validators.required]]
  });

}
  ngOnInit(): void {
  }

  submit(data:any)
  {
     this.product.addProduct(data).subscribe((result)=>
     {
      console.warn(result)
      if(result){
        this.addProductMessage="Product is added successfully";
      }
     });
     setTimeout(()=>{
      this.addProductMessage=undefined
     },3000);
  }
}
