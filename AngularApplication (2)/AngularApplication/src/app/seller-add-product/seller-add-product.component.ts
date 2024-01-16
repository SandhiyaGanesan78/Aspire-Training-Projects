import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';
import { FormBuilder,FormGroup, Validators,FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-seller-add-product',
  templateUrl: './seller-add-product.component.html',
  styleUrls: ['./seller-add-product.component.css']
})
export class SellerAddProductComponent implements OnInit {
  addProductMessage:string|undefined;
  addProduct: FormGroup;

  constructor(private product: ProductsService, private formBuilder: FormBuilder) {
    this.addProduct = this.formBuilder.group({
      name: ["", [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
      price: ["", [Validators.required]],
      category: ["", [Validators.required]],
      description: ["", [Validators.required, Validators.maxLength(500)]],
      image: ["", [Validators.required]],
      bestSeller: [false],
    });
  }

  ngOnInit(): void {
  }

  submit(data:product)
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
