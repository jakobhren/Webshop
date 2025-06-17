import { Component, OnInit } from '@angular/core';
import { Category } from '../model/category';
import { CategoryComponent } from "../category/category.component";
import { CategoryService } from '../services/category.service';
import { EbookService } from '../services/ebook.service';
import { Ebook } from '../model/ebook';
import { EbookComponent } from "../ebook/ebook.component";
import { CommonModule } from '@angular/common';
import { EbookListComponent } from '../ebook-list/ebook-list.component';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [CategoryComponent, EbookComponent, CommonModule, EbookListComponent],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.css'
})
export class CategoryListComponent implements OnInit {
categories: Category[] = [];
ebooks: Ebook[] = [];
filteredEbooks: Ebook[] = [];


 constructor(private categoryService: CategoryService, private ebookService: EbookService) {}
 
   ngOnInit(): void {
     this.categoryService.getCategories().subscribe(categories => {
       this.categories = categories;
     });


     this.ebookService.getEbooks().subscribe(ebooks => {
      this.ebooks = ebooks;
      //this.filteredEbooks = ebooks; // initially show all
    });
   }

   filterByCategory(categoryId: number): void {
    this.filteredEbooks = this.ebooks.filter(e => e.categoryId === categoryId);
  }
 
 /* {
      categoryId: 1,
      categoryName: 'Science Fiction'
      },

      {
        categoryId: 2,
        categoryName: 'Romance'
      },

        {
          categoryId: 3,
          categoryName: 'Thriller'
          }
   ]; 

   */
}
