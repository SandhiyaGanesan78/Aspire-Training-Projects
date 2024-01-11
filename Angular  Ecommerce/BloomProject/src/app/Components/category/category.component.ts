import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from 'src/app/Services/products.service';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  products: any;

  constructor(private activeRoute: ActivatedRoute, private product:ProductsService){

  }
  ngOnInit(): void {
    let category= this.activeRoute.snapshot.paramMap.get('category');
    console.warn(category);
    category && this.product.getProducts(category).subscribe((result)=>{
      this.products=result
    });
  }

}
