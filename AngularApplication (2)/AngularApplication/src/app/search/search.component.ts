import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  searchResult:undefined| product[];
  constructor(private activeRoute: ActivatedRoute, private product:ProductsService){

  }
  ngOnInit(): void {
    let query= this.activeRoute.snapshot.paramMap.get('query');
    console.warn(query);
    query && this.product.searchProduct(query).subscribe((result)=>{
      this.searchResult=result

    })

  }

}
